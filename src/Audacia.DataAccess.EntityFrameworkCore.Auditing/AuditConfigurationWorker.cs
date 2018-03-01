using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.Core.Extensions;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    internal class AuditConfigurationWorker<TDbContext>
        where TDbContext : DbContext
    {
        #region helpers

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

        private readonly IDictionary<object, AuditEntryWrapper> _entityEntryWrappers =
            new Dictionary<object, AuditEntryWrapper>();

        private readonly AuditConfiguration<TDbContext> _configuration;

        public AuditConfigurationWorker(AuditConfiguration<TDbContext> configuration)
        {
            _configuration = configuration;
        }

        private AuditEntryWrapper PopulateEntryWrapper(object entity, AuditContext context, AuditState state)
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

        private static string ResolveFriendlyValue(object entity, object value,
            IPropertyAuditConfiguration propertyConfiguration, DbContext context)
        {
            if (value == null)
            {
                return null;
            }

            if (propertyConfiguration.FriendlyValueLookupType != null)
            {
                var lookupObject = context.Find(propertyConfiguration.FriendlyValueLookupType, value);

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

        private static void PopulateNewValue(AuditEntryProperty auditEntryProperty, PropertyEntryWrapper wrapper,
            object entity, AuditContext context)
        {
            var newValue = wrapper.NewValue;
            auditEntryProperty.NewValue = newValue;
            auditEntryProperty.FriendlyNewValue = ResolveFriendlyValue(entity, newValue,
                wrapper.Configuration,
                context.TriggerContext.DbContext);
        }

        private static void PopulateOldValue(AuditEntryProperty auditEntryProperty, PropertyEntryWrapper wrapper,
            object entity, AuditContext context)
        {
            var oldValue = wrapper.OldValue;
            auditEntryProperty.OldValue = oldValue;
            auditEntryProperty.FriendlyOldValue = ResolveFriendlyValue(entity, oldValue,
                wrapper.Configuration,
                context.TriggerContext.DbContext);
        }

        private static void PopulatePrimaryKeys(AuditEntry auditEntry, AuditContext context)
        {
            //Populate primary key values as these may have been DB generated so not present before
            var primaryKeyValues = from property in context.TriggerContext.EntityEntry.Properties
                where property.Metadata.IsPrimaryKey()
                select property.CurrentValue;

            auditEntry.PrimaryKeyValues = primaryKeyValues.ToArray();
        }

        #endregion

        #region inserts

        public void Insterting(object entity, AuditContext context)
        {
            if (context.Configuration.Ignore)
            {
                return;
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
        }

        public void Inserted(object entity, AuditContext context)
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
                    PopulateNewValue(auditEntryProperty, propertyWrapper, entity, context);
                }
            }

            //Populate primary key values as these may have been DB generated so not present before
            PopulatePrimaryKeys(wrapper.AuditEntry, context);
        }

        #endregion

        #region updates

        public void Updating(object entity, AuditContext context)
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
                    PopulateOldValue(auditEntryProperty, propertyWrapper, entity, context);

                    wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] = auditEntryProperty;
                }
            }
        }

        public void Updated(object entity, AuditContext context)
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
                    PopulateNewValue(auditEntryProperty, propertyWrapper, entity, context);
                }
            }

            PopulatePrimaryKeys(wrapper.AuditEntry, context);
        }

        #endregion

        #region deleting

        public void Deleting(object entity, AuditContext context)
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
                    PopulateOldValue(auditEntryProperty, propertyWrapper, entity, context);

                    wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] = auditEntryProperty;
                }
            }
        }

        #endregion
    }
}