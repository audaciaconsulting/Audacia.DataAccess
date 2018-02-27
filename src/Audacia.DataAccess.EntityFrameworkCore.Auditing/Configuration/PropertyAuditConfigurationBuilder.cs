using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public abstract class PropertyAuditConfigurationBuilder
    {
        internal PropertyInfo PropertyInfo;
        internal bool? InternalIgnore;
        internal string InternalFriendlyName;

        //Note: If object is TEntity then it's local to type, if not it's a lookup
        internal Expression<Func<object, string>> InternalFriendlyValueFactory;

        // ReSharper disable once SuggestBaseTypeForParameter
        protected PropertyAuditConfigurationBuilder(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }

    public class PropertyAuditConfigurationBuilder<TEntity, TProperty> : PropertyAuditConfigurationBuilder
        where TEntity : class, new()
    {
        public PropertyAuditConfigurationBuilder(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
        }

        public PropertyAuditConfigurationBuilder<TEntity, TProperty> FriendlyName(string friendlyName)
        {
            InternalFriendlyName = friendlyName;

            return this;
        }

        public PropertyAuditConfigurationBuilder<TEntity, TProperty> FriendlyValue(
            Func<TEntity, string> valueFactory)
        {
            InternalFriendlyValueFactory = o => valueFactory(o as TEntity);

            return this;
        }

        public PropertyAuditConfigurationBuilder<TEntity, TProperty> FriendlyValue<TValueLookup>(Func<TValueLookup, string> valueFactory)
            where TValueLookup : class
        {
            InternalFriendlyValueFactory = o => valueFactory(o as TValueLookup);

            return this;
        }

        public PropertyAuditConfigurationBuilder<TEntity, TProperty> Ignore(bool ignore = true)
        {
            InternalIgnore = ignore;

            return this;
        }
    }
}