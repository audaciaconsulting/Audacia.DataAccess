using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Audacia.CodeAnalysis.Analyzers.Helpers.MethodLength;
using Audacia.Core.Extensions;
using Audacia.DataAccess.EntityFrameworkCore.Triggers;
using Audacia.DataAccess.Model.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

// NOTE: Where the magic happens

/// <summary>
/// AuditConfigurationApplicant class contains functionality to hande audit entries.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
/// <typeparam name="TDbContext">Database context type.</typeparam>
internal class AuditConfigurationApplicant<TUserIdentifier, TDbContext>
    where TUserIdentifier : struct
    where TDbContext : DbContext
{
    private class AuditEntryWrapper
    {
        public AuditEntry<TUserIdentifier>? AuditEntry { get; set; }

        public ICollection<PropertyEntryWrapper>? PropertyWrappers { get; set; }
    }

    private class PropertyEntryWrapper
    {
        public IPropertyAuditConfiguration? Configuration { get; set; }

        public PropertyEntry? PropertyEntry { get; set; }

        public object OldValue { get; set; } = new object();

        public object NewValue { get; set; } = new object();

        public bool IsModified { get; set; }
    }

    private class AuditIntanceState
    {
        public ConcurrentDictionary<object, AuditEntryWrapper> EntityEntryWrappers { get; } =
            new ConcurrentDictionary<object, AuditEntryWrapper>();
    }

    private readonly TriggerRegistrar<TDbContext> _triggerRegistrar;
    private readonly IAuditConfiguration<TUserIdentifier, TDbContext> _configuration;

    private readonly IDictionary<TDbContext, AuditIntanceState> _auditStateDictionary =
        new ConcurrentDictionary<TDbContext, AuditIntanceState>();

    /// <summary>
    /// Sets values of triggerRegistrar and configuration.
    /// </summary>
    /// <param name="triggerRegistrar">Instance of triggerRegistrar.</param>
    /// <param name="configuration">Instance of configuration.</param>
    public AuditConfigurationApplicant(TriggerRegistrar<TDbContext> triggerRegistrar, IAuditConfiguration<TUserIdentifier, TDbContext> configuration)
    {
        _triggerRegistrar = triggerRegistrar;
        _configuration = configuration;
    }

    /// <summary>
    /// Contains beforeAsync and afterAsync event handlers.
    /// </summary>
    [MaxMethodLength(19)]
    internal void Apply()
    {
        _triggerRegistrar.BeforeAsync += (context, _) =>
        {
            _auditStateDictionary[context] = new AuditIntanceState();

            return Task.CompletedTask;
        };

        _triggerRegistrar.Register(TriggerType.Inserting, TransformToTrigger(InstertingAsync));
        _triggerRegistrar.Register(TriggerType.Inserted, TransformToTrigger(InsertedAsync));
        _triggerRegistrar.Register(TriggerType.Updating, TransformToTrigger(UpdatingAsync));
        _triggerRegistrar.Register(TriggerType.Updated, TransformToTrigger(UpdatedAsync));
        _triggerRegistrar.Register(TriggerType.Deleting, TransformToTrigger(DeletingAsync));

        _triggerRegistrar.AfterAsync += async (context, cancellationToken) =>
        {
            try
            {
                var auditEntries = _auditStateDictionary[context].EntityEntryWrappers.Values
                    .Select(wrapper => wrapper.AuditEntry).ToList();

                if (_configuration.DoNotAuditIfNoChangesInTrackedProperties && auditEntries?.Any() == true)
                {
                    auditEntries = auditEntries.Where(auditEntry => auditEntry!.Properties.Any()).ToList();
                }

                if (_configuration.SinkFactories != null) 
                {
                    foreach (var sinkFactory in _configuration.SinkFactories)
                    {
                        var sink = sinkFactory.Create(context);
                        if (sink != null && auditEntries != null)
                        {
                            await sink.HandleAsync(auditEntries!, cancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }
            finally
            {
                //Remove so does not cluttle up memory as this class will effectivly act as a singleton due to closures and TriggerRegistrar being a singleton
                _auditStateDictionary.Remove(context);   
            }
        };
    }

    private Func<object, TriggerContext<TDbContext>, CancellationToken, Task> TransformToTrigger(
        Func<object, AuditContext<TDbContext>, CancellationToken, Task> auditAction)
        => (obj, triggerContext, cancellationToken) => auditAction(
            obj,
            new AuditContext<TDbContext>(_configuration.Entities![obj.GetType()], triggerContext),
            cancellationToken);

    private AuditEntryWrapper PopulateEntryWrapper(object entity, AuditContext<TDbContext> context,
        AuditState state)
    {
        var type = entity.GetType();

        var entityEntry = new AuditEntry<TUserIdentifier>
        {
            FullName = type.FullName ?? string.Empty,
            ShortName = type.Name,
            FriendlyName = context.Configuration.FriendlyName,
            Strategy = context.Configuration.Strategy,
            State = state
        };
        
        if (_configuration.UserIdentifierFactory != null) 
        {
            entityEntry.UserIdentifier = _configuration.UserIdentifierFactory();
        }

        var propertyWrappers = from property in context.TriggerContext.EntityEntry.Properties
                               where !property.Metadata.IsPrimaryKey()
                               let configuration = context.Configuration.Properties[property.Metadata.Name]
                               where !configuration.Ignore
                               select new PropertyEntryWrapper
                               {
                                   OldValue = property.OriginalValue!,
                                   NewValue = property.CurrentValue!,
                                   IsModified = property.IsModified,
                                   Configuration = configuration,
                                   PropertyEntry = property
                               };

        var wrapper = new AuditEntryWrapper
        {
            AuditEntry = entityEntry,
            PropertyWrappers = propertyWrappers.ToList()
        };

        _auditStateDictionary[context.TriggerContext.DbContext].EntityEntryWrappers[entity] = wrapper;

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

    /// <summary>
    /// Look up properties of database context.
    /// </summary>
    /// <param name="entity">Entity type.</param>
    /// <param name="value">Object of the value.</param>
    /// <param name="propertyConfiguration"><see cref="IPropertyAuditConfiguration"/>.</param>
    /// <param name="context">Database context.</param>
    /// <returns>Task(string).</returns>
    [MaxMethodLength(13)]
    private static async Task<string?> ResolveFriendlyValueAsync(
        object entity,
        object value,
        IPropertyAuditConfiguration propertyConfiguration, DbContext context)
    {
        if (value == null)
        {
            return await Task.FromResult<string?>(null).ConfigureAwait(false);
        }

        if (propertyConfiguration.FriendlyValueLookupType != null)
        {
            var lookupObject = await context.FindAsync(propertyConfiguration.FriendlyValueLookupType, value).ConfigureAwait(false);
            if (lookupObject != null && propertyConfiguration.FriendlyValueFactory != null)
            {
                return propertyConfiguration.FriendlyValueFactory(lookupObject);
            }
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

    /// <summary>
    /// PopulateNewValueAsync method.
    /// </summary>
    /// <param name="auditEntryProperty">AuditEntry property <see cref="AuditEntryProperty"/>.</param>
    /// <param name="wrapper">Instance of wrapper <see cref="PropertyEntryWrapper"/>.</param>
    /// <param name="entity">Entity type.</param>
    /// <param name="context">Database context.</param>
    /// <returns>No return type.</returns>
    private static async Task PopulateNewValueAsync(
        AuditEntryProperty auditEntryProperty,
        PropertyEntryWrapper wrapper,
        object entity,
        AuditContext<TDbContext> context)
    {
        if (wrapper.Configuration != null)
        {
            var newValue = wrapper.NewValue;
            auditEntryProperty.NewValue = newValue;
            auditEntryProperty.FriendlyNewValue = await ResolveFriendlyValueAsync(
                entity,
                newValue,
                wrapper.Configuration,
                context.TriggerContext.DbContext).ConfigureAwait(false) ?? string.Empty;
        }
    }

    /// <summary>
    /// PopulateOldValueAsync method.
    /// </summary>
    /// <param name="auditEntryProperty">AuditEntry property <see cref="AuditEntryProperty"/>.</param>
    /// <param name="wrapper">Instance of wrapper <see cref="PropertyEntryWrapper"/>.</param>
    /// <param name="entity">Entity type.</param>
    /// <param name="context">Database context.</param>
    /// <returns>No return type.</returns>
    private static async Task PopulateOldValueAsync(
        AuditEntryProperty auditEntryProperty,
        PropertyEntryWrapper wrapper,
        object entity, AuditContext<TDbContext> context)
    {
        if (wrapper.Configuration != null)
        {
            var oldValue = wrapper.OldValue;
            auditEntryProperty.OldValue = oldValue;
            auditEntryProperty.FriendlyOldValue = await ResolveFriendlyValueAsync(
                entity,
                oldValue,
                wrapper.Configuration,
                context.TriggerContext.DbContext).ConfigureAwait(false) ?? string.Empty;
        }
    }

    private static void PopulatePrimaryKeys(AuditEntry<TUserIdentifier> auditEntry, AuditContext<TDbContext> context)
    {
        //Populate primary key values as these may have been DB generated so not present before
        var primaryKeyValues = from property in context.TriggerContext.EntityEntry.Properties
                               where property.Metadata.IsPrimaryKey()
                               select property.CurrentValue;

        auditEntry.PrimaryKeyValues = primaryKeyValues.ToArray();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    private Task InstertingAsync(object entity, AuditContext<TDbContext> context,
        CancellationToken cancellationToken)
    {
        if (context.Configuration.Ignore)
        {
            return Task.CompletedTask;
        }

        var wrapper = PopulateEntryWrapper(entity, context, AuditState.Added);

        foreach (var propertyWrapper in wrapper.PropertyWrappers ?? [])
        {
            //If we're in full more or there is a value
            if ((context.Configuration.Strategy == AuditStrategy.Full || propertyWrapper.NewValue != null) && propertyWrapper.Configuration != null && wrapper.AuditEntry != null)
            {
                wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] =
                    InitProperty(propertyWrapper.Configuration);
            }
        }

        return Task.CompletedTask;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    private async Task InsertedAsync(object entity, AuditContext<TDbContext> context,
        CancellationToken cancellationToken)
    {
        if (context.Configuration.Ignore)
        {
            return;
        }

        var wrapper = _auditStateDictionary[context.TriggerContext.DbContext].EntityEntryWrappers[entity];

        if (wrapper.AuditEntry != null)
        {
            //Populate description after insert so can access related entities if need be
            wrapper.AuditEntry.Description = context.Configuration.DescriptionFactory(entity);
        }
        else 
        {
            return;
        }

        //Populate properties after insert so can access related entities if need be
        foreach (var propertyWrapper in wrapper.PropertyWrappers ?? [])
        {
            if (propertyWrapper.Configuration?.Property != null 
                && wrapper.AuditEntry.Properties.TryGetValue(
                    propertyWrapper.Configuration.Property.Name,
                out var auditEntryProperty))
            {
                await PopulateNewValueAsync(auditEntryProperty, propertyWrapper, entity, context).ConfigureAwait(false);
            }
        }

        //Populate primary key values as these may have been DB generated so not present before
        PopulatePrimaryKeys(wrapper.AuditEntry, context);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    private async Task UpdatingAsync(object entity, AuditContext<TDbContext> context,
        CancellationToken cancellationToken)
    {
        if (context.Configuration.Ignore)
        {
            return;
        }

        var wrapper = PopulateEntryWrapper(entity, context, AuditState.Modified);

        foreach (var propertyWrapper in wrapper.PropertyWrappers ?? [])
        {
            //If we're in full more or there value has been modified
            if ((context.Configuration.Strategy == AuditStrategy.Full || propertyWrapper.IsModified) && propertyWrapper.Configuration != null && wrapper.AuditEntry != null)
            {
                var auditEntryProperty = InitProperty(propertyWrapper.Configuration);

                //Populate here as may access old relationship
                await PopulateOldValueAsync(auditEntryProperty, propertyWrapper, entity, context).ConfigureAwait(false);

                wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] = auditEntryProperty;
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    private async Task UpdatedAsync(object entity, AuditContext<TDbContext> context,
        CancellationToken cancellationToken)
    {
        if (context.Configuration.Ignore)
        {
            return;
        }

        var wrapper = _auditStateDictionary[context.TriggerContext.DbContext].EntityEntryWrappers[entity];

        if (wrapper.AuditEntry != null)
        {
            //Populate description after update so can access new related entities if need be
            wrapper.AuditEntry.Description = context.Configuration.DescriptionFactory(entity);
        }
        else 
        {
            return;
        }

        //Populate new values after insert so can access new related entities if need be
        foreach (var propertyWrapper in wrapper.PropertyWrappers ?? [])
        {
            if (propertyWrapper.Configuration?.Property != null
                && wrapper.AuditEntry.Properties.TryGetValue(
                    propertyWrapper.Configuration.Property.Name,
                out var auditEntryProperty))
            {
                await PopulateNewValueAsync(auditEntryProperty, propertyWrapper, entity, context).ConfigureAwait(false);
            }
        }

        PopulatePrimaryKeys(wrapper.AuditEntry, context);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    [MaxMethodLength(11)]
    private async Task DeletingAsync(object entity, AuditContext<TDbContext> context,
        CancellationToken cancellationToken)
    {
        if (context.Configuration.Ignore)
        {
            return;
        }

        var wrapper = PopulateEntryWrapper(entity, context, AuditState.Deleted);

        if (wrapper.AuditEntry == null) 
        {
            return;
        }

        foreach (var propertyWrapper in wrapper.PropertyWrappers ?? [])
        {
            //If we're in full more or there value has been modified
            if ((context.Configuration.Strategy == AuditStrategy.Full || propertyWrapper.OldValue != null) && propertyWrapper.Configuration != null)
            {
                var auditEntryProperty = InitProperty(propertyWrapper.Configuration);

                //Populate here as may access old relationship
                await PopulateOldValueAsync(auditEntryProperty, propertyWrapper, entity, context).ConfigureAwait(false);

                wrapper.AuditEntry.Properties[propertyWrapper.Configuration.Property.Name] = auditEntryProperty;
            }
        }
        
        PopulatePrimaryKeys(wrapper.AuditEntry, context);
    }
}
