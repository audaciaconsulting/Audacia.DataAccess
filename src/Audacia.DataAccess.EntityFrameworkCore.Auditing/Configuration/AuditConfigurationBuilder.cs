using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

public class AuditConfigurationBuilder<TUserIdentifier, TDbContext>
    where TDbContext : DbContext
    where TUserIdentifier : struct
{
    private bool _doNotAuditIfNoChangesInTrackedProperties;
    private AuditStrategy _strategy = AuditStrategy.Partial;
    private Func<TUserIdentifier?> _userIdentifierFactory = () => null;
    private readonly ICollection<IAuditSinkFactory<TUserIdentifier, TDbContext>> _sinkFactories =
        new List<IAuditSinkFactory<TUserIdentifier, TDbContext>>();
    private readonly IDictionary<Type, TypeAuditConfigurationBuilder> _types =
        new Dictionary<Type, TypeAuditConfigurationBuilder>();

    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> UserIdentifierFactory(
        Func<TUserIdentifier?> factory)
    {
        _userIdentifierFactory = factory;

        return this;
    }

    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> AddSinkFactory(IAuditSinkFactory<TUserIdentifier, TDbContext> factory)
    {
        _sinkFactories.Add(factory);

        return this;
    }

    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> AddSinkFactory(Func<TDbContext, IAuditSink<TUserIdentifier>> factory)
    {
        _sinkFactories.Add(new DynamicAuditSinkFactory<TUserIdentifier, TDbContext>(factory));

        return this;
    }

    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> DoNotAuditIfNoChangesInTrackedProperties(bool doNotAudit = true)
    {
        _doNotAuditIfNoChangesInTrackedProperties = doNotAudit;

        return this;
    }

    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> Strategy(AuditStrategy strategy)
    {
        _strategy = strategy;

        return this;
    }

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

    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> Entity<TEntity>(
        Action<EntityAuditConfigurationBuilder<TEntity>> entityBuilderAction)
        where TEntity : class
    {
        var entityBuilder = Entity<TEntity>();

        entityBuilderAction(entityBuilder);

        return this;
    }

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

    public AuditConfigurationBuilder<TUserIdentifier, TDbContext> Type<T>(
        Action<TypeAuditConfigurationBuilder<T>> typeBuilderAction)
        where T : class
    {
        var typeBuilder = Type<T>();

        typeBuilderAction(typeBuilder);

        return this;
    }

    public IAuditConfiguration<TUserIdentifier, TDbContext> Build(IModel model)
    {
        //Create context just to access model and fully populate configuration
        //Loop through DB entities and find matching audit configurations
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