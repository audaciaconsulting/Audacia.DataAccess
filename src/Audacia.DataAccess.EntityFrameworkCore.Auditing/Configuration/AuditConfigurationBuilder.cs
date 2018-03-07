﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public class AuditConfigurationBuilder<TDbContext>
        where TDbContext : DbContext
    {
        private bool _doNotAuditIfNoChangesInTrackedProperties;
        private AuditStrategy _strategy = AuditStrategy.Partial;

        private readonly IDictionary<Type, TypeAuditConfigurationBuilder> _types =
            new Dictionary<Type, TypeAuditConfigurationBuilder>();

        public AuditConfigurationBuilder<TDbContext> DoNotAuditIfNoChangesInTrackedProperties(bool doNotAudit = true)
        {
            _doNotAuditIfNoChangesInTrackedProperties = doNotAudit;

            return this;
        }

        public AuditConfigurationBuilder<TDbContext> Strategy(AuditStrategy strategy)
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

            return entityBuilder as EntityAuditConfigurationBuilder<TEntity>;
        }

        public AuditConfigurationBuilder<TDbContext> Entity<TEntity>(
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

            return typeBuilder as TypeAuditConfigurationBuilder<T>;
        }

        public AuditConfigurationBuilder<TDbContext> Type<T>(
            Action<TypeAuditConfigurationBuilder<T>> typeBuilderAction)
            where T : class
        {
            var typeBuilder = Type<T>();

            typeBuilderAction(typeBuilder);

            return this;
        }

        public IAuditConfiguration<TDbContext> Build(IModel model)
        {
            //Create context just to access model and fully populate configuration
            //Loop through DB entities and find matching audit configurations
            var entities = from entityType in model.GetEntityTypes()
                let matchingConfigurations = (from pair in _types
                    where pair.Key.IsAssignableFrom(entityType.ClrType)
                    orderby GetTypeSortOrder(pair.Value, entityType.ClrType)
                    select pair.Value).ToList()
                select new EntityAuditConfiguration(entityType, matchingConfigurations, _strategy);

            return new AuditConfiguration<TDbContext>
            {
                DoNotAuditIfNoChangesInTrackedProperties = _doNotAuditIfNoChangesInTrackedProperties,
                Entities =
                    entities.ToDictionary(item => item.EntityType.ClrType, item => item as IEntityAuditConfiguration),
                Strategy = _strategy
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
}