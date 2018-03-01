using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public abstract class EntityAuditConfigurationBuilder
    {
        internal bool? InternalIgnore;
        internal Func<object, string> InternalDescriptionFactory;
        internal AuditStrategy? InternalAuditStrategy;
        internal string InternalFriendlyName;

        internal readonly IDictionary<string, PropertyAuditConfigurationBuilder> Properties =
            new Dictionary<string, PropertyAuditConfigurationBuilder>();

        protected EntityAuditConfigurationBuilder(Type typeOfEntity)
        {
            TypeOfEntity = typeOfEntity;
        }

        public Type TypeOfEntity { get; }
    }

    public class EntityAuditConfigurationBuilder<TEntity> : EntityAuditConfigurationBuilder
        where TEntity : class, new()
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

            return propertyBuilder as PropertyAuditConfigurationBuilder<TEntity, TProperty>;
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