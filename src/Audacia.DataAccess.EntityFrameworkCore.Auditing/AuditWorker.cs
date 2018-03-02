using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.Core.Extensions;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    internal class AuditWorker<TDbContext>
        where TDbContext : DbContext
    {
        private class AuditEntryWrapper
        {
            public AuditEntry AuditEntry { get; set; }
            public ICollection<PropertyEntryWrapper> PropertyWrappers { get; set; }
        }

        private class PropertyEntryWrapper
        {
            public IPropertyAuditConfiguration Configuration { get; set; }
            public PropertyEntry PropertyEntry { get; set; }
            public object OldValue { get; set; }
            public object NewValue { get; set; }
            public bool IsModified { get; set; }
        }

        private readonly IAuditConfiguration<TDbContext> _configuration;
        private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;
        private readonly IEnumerable<IAuditSink> _sinks;
        private IDictionary<TriggerType, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>> _triggers;

        private readonly IDictionary<object, AuditEntryWrapper> _entityEntryWrappers =
            new Dictionary<object, AuditEntryWrapper>();

        public AuditWorker(IAuditConfiguration<TDbContext> configuration, TriggerRegistrar<TDbContext> triggerRegistrar,
            IEnumerable<IAuditSink> sinks)
        {
            _configuration = configuration;
            _triggerRegistrar = triggerRegistrar;
            _sinks = sinks;
        }

        public void Init()
        {
            //Register trigger
            _triggers = new Dictionary<TriggerType, Func<object, TriggerContext<TDbContext>, CancellationToken, Task>>
            {
                {TriggerType.Inserting, TransformToTrigger(InstertingAsync)},
                {TriggerType.Inserted, TransformToTrigger(InsertedAsync)},
                {TriggerType.Updating, TransformToTrigger(UpdatingAsync)},
                {TriggerType.Updated, TransformToTrigger(UpdatedAsync)},
                {TriggerType.Deleting, TransformToTrigger(DeletingAsync)}
            };

            foreach (var trigger in _triggers)
            {
                _triggerRegistrar.Register(trigger.Key, trigger.Value);
            }

            _triggerRegistrar.After += async (_, cancellationToken) =>
            {
                var auditEntries = _entityEntryWrappers.Values.Select(wrapper => wrapper.AuditEntry).ToList();

                if (_configuration.DoNotAuditIfNoChangesInTrackedProperties)
                {
                    auditEntries = auditEntries.Where(auditEntry => auditEntry.Properties.Any()).ToList();
                }

                foreach (var item in _sinks)
                {
                    await item.HandleAsync(auditEntries, cancellationToken);
                }
            };
        }

        #region helpers

        private Func<object, TriggerContext<TDbContext>, CancellationToken, Task> TransformToTrigger(
            Func<object, AuditContext<TDbContext>, CancellationToken, Task> auditAction)
            => (obj, triggerContext, cancellationToken) => auditAction(obj, new AuditContext<TDbContext>
            {
                Configuration = _configuration.Entities[obj.GetType()],
                TriggerContext = triggerContext
            }, cancellationToken);

        private AuditEntryWrapper PopulateEntryWrapper(object entity, AuditContext<TDbContext> context,
            AuditState state)
        {
            var type = entity.GetType();

            var entityEntry = new AuditEntry
            {
                FullName = type.FullName,
                ShortName = type.Name,
                FriendlyName = context.Configuration.FriendlyName,
                Strategy = context.Configuration.Strategy,
                State = state,
                Properties = new Dictionary<string, AuditEntryProperty>()
            };

            var propertyWrappers = from property in context.TriggerContext.EntityEntry.Properties
                where !property.Metadata.IsPrimaryKey()
                let configuration = context.Configuration.Properties[property.Metadata.Name]
                where !configuration.Ignore
                select new PropertyEntryWrapper
                {
                    OldValue = property.OriginalValue,
                    NewValue = property.CurrentValue,
                    IsModified = property.IsModified,
                    Configuration = configuration,
                    PropertyEntry = property
                };

            var wrapper = new AuditEntryWrapper
            {
                AuditEntry = entityEntry,
                PropertyWrappers = propertyWrappers.ToList()
            };

            _entityEntryWrappers[entityEntry] = wrapper;

            return wrapper;
        }

        private static AuditEntryProperty InitProperty(IPropertyAuditConfiguration propertyConfiguration)
        {
            return new AuditEntryProperty
            {
                Name = propertyConfiguration.Property.Name,
                FriendlyName = propertyConfiguration.FriendlyName
            };
        }

        private static async Task<string> ResolveFriendlyValueAsync(object entity, object value,
            IPropertyAuditConfiguration propertyConfiguration, DbContext context)
        {
            if (value == null)
            {
                return null;
            }

            if (propertyConfiguration.FriendlyValueLookupType != null)
            {
                var lookupObject = await context.FindAsync(propertyConfiguration.FriendlyValueLookupType, value);

                return propertyConfiguration.FriendlyValueFactory(lookupObject);
            }

            //If there is not a lookup type use the entity itself
            if (propertyConfiguration.FriendlyValueFactory != null)
            {
                return propertyConfiguration.FriendlyValueFactory(entity);
            }

            if (value.GetType().IsEnum)
            {
                var underlyingType = propertyConfiguration.Property.ClrType.GetUnderlyingTypeIfNullable();

                //If it's an enum do a lookup for friendly name
                if (underlyingType.IsEnum)
                {
                    return underlyingType.ToEnumDescriptionString(value);
                }
            }

            return value.ToString();
        }

        private static async Task PopulateNewValueAsync(AuditEntryProperty auditEntryProperty,
            PropertyEntryWrapper wrapper,
            object entity, AuditContext<TDbContext> context)
        {
            var newValue = wrapper.NewValue;
            auditEntryProperty.NewValue = newValue;
            auditEntryProperty.FriendlyNewValue = await ResolveFriendlyValueAsync(entity, newValue,
                wrapper.Configuration,
                context.TriggerContext.DbContext);
        }

        private static async Task PopulateOldValueAsync(AuditEntryProperty auditEntryProperty,
            PropertyEntryWrapper wrapper,
            object entity, AuditContext<TDbContext> context)
        {
            var oldValue = wrapper.OldValue;
            auditEntryProperty.OldValue = oldValue;
            auditEntryProperty.FriendlyOldValue = await ResolveFriendlyValueAsync(entity, oldValue,
                wrapper.Configuration,
                context.TriggerContext.DbContext);
        }

        private static void PopulatePrimaryKeys(AuditEntry auditEntry, AuditContext<TDbContext> context)
        {
            //Populate primary key values as these may have been DB generated so not present before
            var primaryKeyValues = from property in context.TriggerContext.EntityEntry.Properties
                where property.Metadata.IsPrimaryKey()
                select property.CurrentValue;

            auditEntry.PrimaryKeyValues = primaryKeyValues.ToArray();
        }

        #endregion

        #region inserts

        private Task InstertingAsync(object entity, AuditContext<TDbContext> context,
            CancellationToken cancellationToken)
        {
            if (context.Configuration.Ignore)
            {
                return Task.CompletedTask;
            }

            var wrapper = PopulateEntryWrapper(entity, context, AuditState.Added);

            foreach (var propertyWrapper in wrapper.PropertyWrappers)
            {
                //If we're in full more or there is a value
                if (context.Configuration.Strategy == AuditStrategy.Full || propertyWrapper.NewValue != null)
                {
                    wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] =
                        InitProperty(propertyWrapper.Configuration);
                }
            }

            return Task.CompletedTask;
        }

        private async Task InsertedAsync(object entity, AuditContext<TDbContext> context,
            CancellationToken cancellationToken)
        {
            if (context.Configuration.Ignore)
            {
                return;
            }

            var wrapper = _entityEntryWrappers[entity];

            //Populate description after insert so can access related entities if need be
            wrapper.AuditEntry.Description = context.Configuration.DescriptionFactory(entity);

            //Populate properties after insert so can access related entities if need be
            foreach (var propertyWrapper in wrapper.PropertyWrappers)
            {
                if (wrapper.AuditEntry.Properties.TryGetValue(propertyWrapper.Configuration.Property.Name,
                    out var auditEntryProperty))
                {
                    await PopulateNewValueAsync(auditEntryProperty, propertyWrapper, entity, context);
                }
            }

            //Populate primary key values as these may have been DB generated so not present before
            PopulatePrimaryKeys(wrapper.AuditEntry, context);
        }

        #endregion

        #region updates

        private async Task UpdatingAsync(object entity, AuditContext<TDbContext> context,
            CancellationToken cancellationToken)
        {
            if (context.Configuration.Ignore)
            {
                return;
            }

            var wrapper = PopulateEntryWrapper(entity, context, AuditState.Modified);

            foreach (var propertyWrapper in wrapper.PropertyWrappers)
            {
                //If we're in full more or there value has been modified
                if (context.Configuration.Strategy == AuditStrategy.Full || propertyWrapper.IsModified)
                {
                    var auditEntryProperty = InitProperty(propertyWrapper.Configuration);

                    //Populate here as may access old relationship
                    await PopulateOldValueAsync(auditEntryProperty, propertyWrapper, entity, context);

                    wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] = auditEntryProperty;
                }
            }
        }

        private async Task UpdatedAsync(object entity, AuditContext<TDbContext> context,
            CancellationToken cancellationToken)
        {
            if (context.Configuration.Ignore)
            {
                return;
            }

            var wrapper = _entityEntryWrappers[entity];

            //Populate description after update so can access new related entities if need be
            wrapper.AuditEntry.Description = context.Configuration.DescriptionFactory(entity);

            //Populate new values after insert so can access new related entities if need be
            foreach (var propertyWrapper in wrapper.PropertyWrappers)
            {
                if (wrapper.AuditEntry.Properties.TryGetValue(propertyWrapper.Configuration.Property.Name,
                    out var auditEntryProperty))
                {
                    await PopulateNewValueAsync(auditEntryProperty, propertyWrapper, entity, context);
                }
            }

            PopulatePrimaryKeys(wrapper.AuditEntry, context);
        }

        #endregion

        #region deleting

        private async Task DeletingAsync(object entity, AuditContext<TDbContext> context,
            CancellationToken cancellationToken)
        {
            if (context.Configuration.Ignore)
            {
                return;
            }

            var wrapper = PopulateEntryWrapper(entity, context, AuditState.Deleted);

            foreach (var propertyWrapper in wrapper.PropertyWrappers)
            {
                //If we're in full more or there value has been modified
                if (context.Configuration.Strategy == AuditStrategy.Full || propertyWrapper.OldValue != null)
                {
                    var auditEntryProperty = InitProperty(propertyWrapper.Configuration);

                    //Populate here as may access old relationship
                    await PopulateOldValueAsync(auditEntryProperty, propertyWrapper, entity, context);

                    wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] = auditEntryProperty;
                }
            }
        }

        #endregion
    }
}