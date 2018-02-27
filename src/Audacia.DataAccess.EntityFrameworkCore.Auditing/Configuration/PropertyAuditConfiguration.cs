using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Audacia.Core.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public class PropertyAuditConfiguration
    {
        public PropertyAuditConfiguration(IProperty property, ICollection<PropertyAuditConfigurationBuilder> configurations)
        {
            Property = property;

            Ignore = configurations.FirstOrDefault(configuration => configuration.InternalIgnore.HasValue)
                         ?.InternalIgnore ?? false;

            FriendlyName = configurations.FirstOrDefault(configuration => configuration.InternalFriendlyName != null)
                         ?.InternalFriendlyName ?? property.Name.SplitCamelCase();

            //Later we do a check when we have value and this is null, will do ToEnumDescriptionString() for enum or just ToString() for others
            FriendlyValueFactory = configurations
                .FirstOrDefault(configuration => configuration.InternalFriendlyValueFactory != null)
                ?.InternalFriendlyValueFactory;
        }

        public IProperty Property { get; internal set; }
        public bool Ignore { get; internal set; }
        public string FriendlyName { get; internal set; }
        public Expression<Func<object, string>> FriendlyValueFactory { get; internal set; }
    }
}