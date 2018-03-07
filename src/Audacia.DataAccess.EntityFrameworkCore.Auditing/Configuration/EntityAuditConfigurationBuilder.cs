using System;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public class EntityAuditConfigurationBuilder<TEntity> : TypeAuditConfigurationBuilder
        where TEntity : class
    {
        public EntityAuditConfigurationBuilder()
            : base(typeof(TEntity))
        {
        }

        public EntityAuditConfigurationBuilder<TEntity> Ignore(bool ignore = true)
        {
            InternalIgnore = ignore;

            return this;
        }

        public EntityAuditConfigurationBuilder<TEntity> Strategy(AuditStrategy strategy)
        {
            InternalAuditStrategy = strategy;

            return this;
        }

        public EntityAuditConfigurationBuilder<TEntity> FriendlyName(string friendlyTypeName)
        {
            InternalFriendlyName = friendlyTypeName;

            return this;
        }

        public EntityAuditConfigurationBuilder<TEntity> Description(Func<TEntity, string> descriptionFactory)
        {
            InternalDescriptionFactory = entity => descriptionFactory(entity as TEntity);

            return this;
        }

        public PropertyAuditConfigurationBuilder<TEntity, TProperty> Property<TProperty>(
            Expression<Func<TEntity, TProperty>>
                propertySelector)
        {
            var propertyInfo = ExpressionExtensions.GetPropertyInfo(propertySelector);

            if (!Properties.TryGetValue(propertyInfo.Name, out var propertyBuilder))
            {
                propertyBuilder = new PropertyAuditConfigurationBuilder<TEntity, TProperty>(propertyInfo);
                Properties.Add(propertyInfo.Name, propertyBuilder);
            }

            return (PropertyAuditConfigurationBuilder<TEntity, TProperty>)propertyBuilder;
        }

        public EntityAuditConfigurationBuilder<TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty>>
            propertySelector, Action<PropertyAuditConfigurationBuilder<TEntity, TProperty>> propertyBuilderAction)
        {
            var builder = Property(propertySelector);

            propertyBuilderAction(builder);

            return this;
        }
    }
}