using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.Core.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

internal class PropertyAuditConfiguration : IPropertyAuditConfiguration
{
    public PropertyAuditConfiguration(IProperty property,
        ICollection<PropertyAuditConfigurationBuilder> configurations)
    {
        Property = property;

        Ignore = configurations.FirstOrDefault(configuration => configuration.InternalIgnore.HasValue)
                     ?.InternalIgnore ?? false;

        FriendlyName = configurations.FirstOrDefault(configuration => configuration.InternalFriendlyName != null)
                           ?.InternalFriendlyName ?? property.Name.SplitCamelCase();

        var friendlyValueConfiguration = configurations
            .FirstOrDefault(configuration => configuration.InternalFriendlyValueFactory != null);

        if (friendlyValueConfiguration != null)
        {
            FriendlyValueFactory = friendlyValueConfiguration.InternalFriendlyValueFactory;

            if (friendlyValueConfiguration.InternalFriendlyValueLookupType != null)
            {
                FriendlyValueLookupType = friendlyValueConfiguration.InternalFriendlyValueLookupType;
            }
        }
    }

    public IProperty Property { get; }

    public bool Ignore { get; }

    public string FriendlyName { get; }

    public Type FriendlyValueLookupType { get; }

    public Func<object, string> FriendlyValueFactory { get; }
}