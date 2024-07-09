using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.Core.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// EntityAuditConfiguration class.
/// </summary>
internal class EntityAuditConfiguration : IEntityAuditConfiguration
{
    /// <summary>
    /// Sets values of EntityType, Ignore, Strategy, FriendlyName, DescriptionFactory and Properties.
    /// </summary>
    /// <param name="entityType">Instance of entityType.</param>
    /// <param name="configurations">Instance of configurations.</param>
    /// <param name="globalStrategy">Instance of globalStrategy.</param>
    internal EntityAuditConfiguration(
        IEntityType entityType,
        ICollection<TypeAuditConfigurationBuilder> configurations,
        AuditStrategy globalStrategy)
    {
        EntityType = entityType;

        Ignore = configurations.FirstOrDefault(configuration => configuration.InternalIgnore == true)
                     ?.InternalIgnore ??
                 false;

        Strategy = configurations
                       .FirstOrDefault(configuration => configuration.InternalAuditStrategy != null)
                       ?.InternalAuditStrategy ?? globalStrategy;

        FriendlyName = configurations
                               .FirstOrDefault(configuration => configuration.InternalFriendlyName != null)
                               ?.InternalFriendlyName ?? entityType.ClrType.Name;

        DescriptionFactory = configurations
                                 .FirstOrDefault(configuration => configuration.InternalDescriptionFactory != null)
                                 ?.InternalDescriptionFactory ?? (_ => FriendlyName);

        var propertyConfigurationLookup = configurations.SelectMany(configuration => configuration.Properties)
            .GroupBy(
                propertyConfiguration => propertyConfiguration.Key,
                propertyConfiguration => propertyConfiguration.Value)
            .ToDictionary();

        //Loop through DB entities and find matching audit configurations
        var properties = from property in entityType.GetProperties()
            where !property.IsPrimaryKey()
            let matchingConfigurations = propertyConfigurationLookup.ContainsKey(property.Name)
                ? propertyConfigurationLookup[property.Name]
                : new List<PropertyAuditConfigurationBuilder>()
            select new PropertyAuditConfiguration(property, matchingConfigurations);

        Properties = properties.ToDictionary(
            propertyConfiguration => propertyConfiguration.Property.Name,
            propertyConfiguration => (IPropertyAuditConfiguration)propertyConfiguration);
    }

    /// <summary>
    /// Gets EntityType.
    /// </summary>
    public IEntityType EntityType { get; }

    /// <summary>
    /// Gets a value indicating whether should ignore.
    /// </summary>
    public bool Ignore { get; }

    /// <summary>
    /// Gets Strategy.
    /// </summary>
    public AuditStrategy Strategy { get; }

    /// <summary>
    /// Gets FriendlyName.
    /// </summary>
    public string FriendlyName { get; }

    /// <summary>
    /// Gets DescriptionFactory.
    /// </summary>
    public Func<object, string> DescriptionFactory { get; }

    /// <summary>
    /// Gets Properties.
    /// </summary>
    public IDictionary<string, IPropertyAuditConfiguration> Properties { get; }
}