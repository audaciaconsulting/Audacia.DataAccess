using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// AuditConfigurationBuilder class.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
/// <typeparam name="TDbContext">Database context type.</typeparam>
public class AuditConfigurationBuilder<TUserIdentifier, TDbContext>
    where TUserIdentifier : struct
    where TDbContext : DbContext
{
    private readonly ICollection<IAuditSinkFactory<TUserIdentifier, TDbContext>> _sinkFactories =
        new List<IAuditSinkFactory<TUserIdentifier, TDbContext>>();

    private readonly IDictionary<Type, TypeAuditConfigurationBuilder> _types =
        new Dictionary<Type, TypeAuditConfigurationBuilder>();

    private bool _doNotAuditIfNoChangesInTrackedProperties;
    private AuditStrategy _strategy = AuditStrategy.Partial;
    private Func<TUserIdentifier?> _userIdentifierFactory = () => null;

    /// <summary>
    /// Assigns value of userIdentifierFactory.
    /// </summary>
    /// <param name="factory">Factory delegate <see cref="Func{TUserIdentifier}" />.</param>
    /// <returns><see cref="AuditConfigurationBuilder{TUserIdentifier, TDbContext}"/>.</returns>
    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> UserIdentifierFactory(
        Func<TUserIdentifier?> factory)
    {
        _userIdentifierFactory = factory;

        return this;
    }

    /// <summary>
    /// Adds an factory item to the collection.
    /// </summary>
    /// <param name="factory">Factory delegate <see cref="Func{TUserIdentifier}" />.</param>
    /// <returns><see cref="AuditConfigurationBuilder{TUserIdentifier, TDbContext}"/>.</returns>
    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> AddSinkFactory(IAuditSinkFactory<TUserIdentifier, TDbContext> factory)
    {
        _sinkFactories.Add(factory);

        return this;
    }

    /// <summary>
    /// Adds a new factory item to the collection.
    /// </summary>
    /// <param name="factory">Factory delegate <see cref="Func{TUserIdentifier}" />.</param>
    /// <returns><see cref="AuditConfigurationBuilder{TUserIdentifier, TDbContext}"/>.</returns>
    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> AddSinkFactory(Func<TDbContext, IAuditSink<TUserIdentifier>> factory)
    {
        _sinkFactories.Add(new DynamicAuditSinkFactory<TUserIdentifier, TDbContext>(factory));

        return this;
    }

    /// <summary>
    /// Set the value of doNotAuditIfNoChangesInTrackedProperties.
    /// </summary>
    /// <param name="doNotAudit">Factory delegate <see cref="Func{TUserIdentifier}" />.</param>
    /// <returns><see cref="AuditConfigurationBuilder{TUserIdentifier, TDbContext}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1564:Parameter in public or internal member is of type bool or bool?", Justification = "Using booleans provides an easy to understand parameter.")]
    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> DoNotAuditIfNoChangesInTrackedProperties(bool doNotAudit = true)
    {
        _doNotAuditIfNoChangesInTrackedProperties = doNotAudit;

        return this;
    }

    /// <summary>
    /// Assigns value of strategy.
    /// </summary>
    /// <param name="strategy">Instance of <see cref="AuditStrategy"/>.</param>
    /// <returns><see cref="AuditConfigurationBuilder{TUserIdentifier, TDbContext}"/>.</returns>
    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> Strategy(AuditStrategy strategy)
    {
        _strategy = strategy;

        return this;
    }

    /// <summary>
    /// Creates and return an instance of <see cref="EntityAuditConfigurationBuilder{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Entity type of <see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</typeparam>
    /// <returns><see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1551:Call the more overloaded method from other overloads.", Justification = "Return types are different.")]
    public EntityAuditConfigurationBuilder<TEntity> Entity<TEntity>()
        where TEntity : class
    {
        var entityType = typeof(TEntity);

        if (!_types.TryGetValue(entityType, out var entityBuilder))
        {
            entityBuilder = new EntityAuditConfigurationBuilder<TEntity>();
            _types.Add(entityType, entityBuilder);
        }

        return (EntityAuditConfigurationBuilder<TEntity>)entityBuilder;
    }

    /// <summary>
    /// Creates a new instance of <see cref="EntityAuditConfigurationBuilder{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Entity type of <see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</typeparam>
    /// <param name="entityBuilderAction"> sasdsd.</param>
    /// <returns><see cref="AuditConfigurationBuilder{TUserIdentifier,TDbContext}"/>.</returns>
    public virtual AuditConfigurationBuilder<TUserIdentifier, TDbContext> Entity<TEntity>(
        Action<EntityAuditConfigurationBuilder<TEntity>> entityBuilderAction)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entityBuilderAction, nameof(entityBuilderAction));

        var entityBuilder = Entity<TEntity>();

        entityBuilderAction(entityBuilder);

        return this;
    }

    /// <summary>
    /// Creates and return a new instance of <see cref="TypeAuditConfigurationBuilder{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of TypeAuditConfigurationBuilder.</typeparam>
    /// <returns><see cref="TypeAuditConfigurationBuilder{T}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1551:Call the more overloaded method from other overloads.", Justification = "Return types are different.")]
    public TypeAuditConfigurationBuilder<T> Type<T>()
        where T : class
    {
        var type = typeof(T);

        if (!_types.TryGetValue(type, out var typeBuilder))
        {
            typeBuilder = new TypeAuditConfigurationBuilder<T>();
            _types.Add(type, typeBuilder);
        }

        return (TypeAuditConfigurationBuilder<T>)typeBuilder;
    }

    /// <summary>
    /// Creates a new instance of <see cref="AuditConfigurationBuilder{TUserIdentifier, TDbContext}"/>.
    /// </summary>
    /// <typeparam name="T">Type of TypeAuditConfigurationBuilder.</typeparam>
    /// <param name="typeBuilderAction">TypeBuilderAction delegate.</param>
    /// <returns><see cref="AuditConfigurationBuilder{TUserIdentifier, TDbContext}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1551:Call the more overloaded method from other overloads.", Justification = "Return types are different.")]
    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> Type<T>(
        Action<TypeAuditConfigurationBuilder<T>> typeBuilderAction)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(typeBuilderAction, nameof(typeBuilderAction));

        var typeBuilder = Type<T>();

        typeBuilderAction(typeBuilder);

        return this;
    }

    /// <summary>
    /// Build <see cref="IAuditConfiguration{TUserIdentifier, TDbContext}"/>. 
    /// </summary>
    /// <param name="model">Instance of <see cref="IModel"/>.</param>
    /// <returns><see cref="IAuditConfiguration{TUserIdentifier, TDbContext}"/>. </returns>
    public IAuditConfiguration<TUserIdentifier, TDbContext> Build(IModel model)
    {
        //Create context just to access model and fully populate configuration
        //Loop through DB entities and find matching audit configurations
        ArgumentNullException.ThrowIfNull(model, nameof(model));
        var entities = from entityType in model.GetEntityTypes()
            let matchingConfigurations = (from pair in _types
                where pair.Key.IsAssignableFrom(entityType.ClrType)
                orderby GetTypeSortOrder(pair.Value, entityType.ClrType)
                select pair.Value).ToList()
            select new EntityAuditConfiguration(entityType, matchingConfigurations, _strategy);

        return new AuditConfiguration<TUserIdentifier, TDbContext>
        {
            DoNotAuditIfNoChangesInTrackedProperties = _doNotAuditIfNoChangesInTrackedProperties,
            Entities =
                entities.ToDictionary(item => item.EntityType.ClrType, item => (IEntityAuditConfiguration)item),
            Strategy = _strategy,
            UserIdentifierFactory = _userIdentifierFactory,
            SinkFactories = _sinkFactories
        };
    }

    private static int GetTypeSortOrder(TypeAuditConfigurationBuilder configuration, Type type)
    {
        //Type itself always wins
        if (configuration.TypeOfEntity == type)
        {
            return 0;
        }

        //Base Classes beat interfaces
        return !configuration.TypeOfEntity.IsInterface ? 1 : 2;
    }
}