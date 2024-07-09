using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.Core.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// PropertyAuditConfiguration class. Setup audit configuration properties.
/// </summary>
internal class PropertyAuditConfiguration : IPropertyAuditConfiguration
{
    /// <summary>
    /// Sets values of Property, Ignore and FriendlyName.
    /// </summary>
    /// <param name="property">Instance of <see cref="IProperty"/>.</param>
    /// <param name="configurations">Instance of <see cref="ICollection{PropertyAuditConfigurationBuilder}"/>.</param>
    public PropertyAuditConfiguration(
        IProperty property,
        ICollection<PropertyAuditConfigurationBuilder> configurations)
    {
        Property = property;

        Ignore = configurations.FirstOrDefault(configuration => configuration.InternalIgnore == true)
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

    /// <summary>
    /// Gets value of Property.
    /// </summary>
    public IProperty Property { get; }

    /// <summary>
    /// Gets a value indicating whether audit should be ignored.
    /// </summary>
    public bool Ignore { get; }

    /// <summary>
    /// Gets value of Property.
    /// </summary>
    public string FriendlyName { get; }

    /// <summary>
    /// Gets value of FriendlyValueLookupType.
    /// </summary>
    public Type? FriendlyValueLookupType { get; }

    /// <summary>
    /// Gets value of FriendlyValueFactory.
    /// </summary>
    public Func<object, string>? FriendlyValueFactory { get; }
}