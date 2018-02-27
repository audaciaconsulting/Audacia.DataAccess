using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public class AuditConfigurationBuilder<TDbContext>
        where TDbContext : DbContext, new()
    {
        private bool _doNotAuditIfNoChangesInTrackedProperties;
        private AuditStrategy _strategy = AuditStrategy.ChangesOnly;
        private readonly IDictionary<Type, EntityAuditConfigurationBuilder> _entites =
            new Dictionary<Type, EntityAuditConfigurationBuilder>();
        
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

        public EntityAuditConfigurationBuilder<TEntity> For<TEntity>()
            where TEntity : class, new()
        {
            var entityType = typeof(TEntity);

            if (!_entites.TryGetValue(entityType, out var entityBuilder))
            {
                entityBuilder = new EntityAuditConfigurationBuilder<TEntity>();
                _entites.Add(entityType, entityBuilder);
            }

            return entityBuilder as EntityAuditConfigurationBuilder<TEntity>;
        }

        public AuditConfigurationBuilder<TDbContext> For<TEntity>(
            Action<EntityAuditConfigurationBuilder<TEntity>> entityBuilderAction)
            where TEntity : class, new()
        {
            var entityBuilder = For<TEntity>();

            entityBuilderAction(entityBuilder);

            return this;
        }

        public IAuditConfiguration<TDbContext> Build()
        {
            //Create context just to access model and fully populate configuration
            using (var context = new TDbContext())
            {
                //Loop through DB entities and find matching audit configurations
                var entities = from entityType in context.Model.GetEntityTypes()
                    let matchingConfigurations = (from pair in _entites
                        where pair.Key.IsAssignableFrom(entityType.ClrType)
                        orderby GetTypeSortOrder(pair.Value, entityType.ClrType)
                        select pair.Value).ToList()
                    select new EntityAuditConfiguration(entityType, matchingConfigurations, _strategy);

                return new AuditConfiguration<TDbContext>
                {
                    DoNotAuditIfNoChangesInTrackedProperties = _doNotAuditIfNoChangesInTrackedProperties,
                    Entities =
                        entities.ToDictionary(item => item.EntityType, item => item as IEntityAuditConfiguration),
                    Strategy = _strategy
                };
            }
        }

        private static int GetTypeSortOrder(EntityAuditConfigurationBuilder configuration, Type type)
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