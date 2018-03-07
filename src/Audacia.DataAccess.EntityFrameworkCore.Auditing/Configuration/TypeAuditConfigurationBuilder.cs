using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public abstract class TypeAuditConfigurationBuilder
    {
        internal bool? InternalIgnore;
        internal Func<object, string> InternalDescriptionFactory;
        internal AuditStrategy? InternalAuditStrategy;
        internal string InternalFriendlyName;

        internal readonly IDictionary<string, PropertyAuditConfigurationBuilder> Properties =
            new Dictionary<string, PropertyAuditConfigurationBuilder>();

        protected TypeAuditConfigurationBuilder(Type typeOfEntity)
        {
            TypeOfEntity = typeOfEntity;
        }

        public Type TypeOfEntity { get; }
    }

    public class TypeAuditConfigurationBuilder<T> : TypeAuditConfigurationBuilder
        where T : class
    {
        public TypeAuditConfigurationBuilder()
            : base(typeof(T))
        {
        }

        public TypeAuditConfigurationBuilder<T> Ignore(bool ignore = true)
        {
            InternalIgnore = ignore;

            return this;
        }

        public TypeAuditConfigurationBuilder<T> Strategy(AuditStrategy strategy)
        {
            InternalAuditStrategy = strategy;

            return this;
        }

        public TypeAuditConfigurationBuilder<T> Description(Func<T, string> descriptionFactory)
        {
            InternalDescriptionFactory = entity => descriptionFactory(entity as T);

            return this;
        }

        public PropertyAuditConfigurationBuilder<T, TProperty> Property<TProperty>(
            Expression<Func<T, TProperty>>
                propertySelector)
        {
            var propertyInfo = ExpressionExtensions.GetPropertyInfo(propertySelector);

            if (!Properties.TryGetValue(propertyInfo.Name, out var propertyBuilder))
            {
                propertyBuilder = new PropertyAuditConfigurationBuilder<T, TProperty>(propertyInfo);
                Properties.Add(propertyInfo.Name, propertyBuilder);
            }

            return (PropertyAuditConfigurationBuilder<T, TProperty>)propertyBuilder;
        }

        public TypeAuditConfigurationBuilder<T> Property<TProperty>(Expression<Func<T, TProperty>>
            propertySelector, Action<PropertyAuditConfigurationBuilder<T, TProperty>> propertyBuilderAction)
        {
            var builder = Property(propertySelector);

            propertyBuilderAction(builder);

            return this;
        }
    }
}