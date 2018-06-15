using System;
using System.Reflection;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public abstract class PropertyAuditConfigurationBuilder
    {
        internal PropertyInfo PropertyInfo;
        internal bool? InternalIgnore;
        internal string InternalFriendlyName;
        internal Type InternalFriendlyValueLookupType;
        internal Func<object, string> InternalFriendlyValueFactory;

        // ReSharper disable once SuggestBaseTypeForParameter
        protected PropertyAuditConfigurationBuilder(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }

    public class PropertyAuditConfigurationBuilder<T, TProperty> : PropertyAuditConfigurationBuilder
        where T : class
    {
        public PropertyAuditConfigurationBuilder(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
        }

        public PropertyAuditConfigurationBuilder<T, TProperty> FriendlyName(string friendlyName)
        {
            InternalFriendlyName = friendlyName;

            return this;
        }

        public PropertyAuditConfigurationBuilder<T, TProperty> FriendlyValue(
            Func<T, string> valueFactory)
        {
            InternalFriendlyValueFactory = o => valueFactory(o as T);

            return this;
        }

        public PropertyAuditConfigurationBuilder<T, TProperty> FriendlyValue<TValueLookup>(
            Func<TValueLookup, string> valueFactory)
            where TValueLookup : class
        {
            InternalFriendlyValueLookupType = typeof(TValueLookup);
            InternalFriendlyValueFactory = o => valueFactory(o as TValueLookup);

            return this;
        }

        public PropertyAuditConfigurationBuilder<T, TProperty> Ignore(bool ignore = true)
        {
            InternalIgnore = ignore;

            return this;
        }
    }
}