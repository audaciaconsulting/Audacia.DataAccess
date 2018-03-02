using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public class EntityAuditConfiguration : IEntityAuditConfiguration
    {
        internal EntityAuditConfiguration(IEntityType entityType,
            ICollection<TypeAuditConfigurationBuilder> configurations,
            AuditStrategy globalStrategy)
        {
            EntityType = entityType;

            Ignore = configurations.FirstOrDefault(configuration => configuration.InternalIgnore.HasValue)
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
                ?.InternalDescriptionFactory;

            var propertyConfigurationLookup = configurations.SelectMany(configuration => configuration.Properties)
                .GroupBy(propertyConfiguration => propertyConfiguration.Key,
                    propertyConfiguration => propertyConfiguration.Value)
                .ToDictionary();

            //Loop through DB entities and find matching audit configurations
            var properties = from property in entityType.GetProperties()
                where !property.IsPrimaryKey()
                let matchingConfigurations = propertyConfigurationLookup[property.Name]
                select new PropertyAuditConfiguration(property, matchingConfigurations);

            Properties = properties.ToDictionary(propertyConfiguration => propertyConfiguration.Property.Name,
                propertyConfiguration => propertyConfiguration as IPropertyAuditConfiguration);
        }

        public IEntityType EntityType { get; }
        public bool Ignore { get; }
        public AuditStrategy Strategy { get; }
        public string FriendlyName { get; }
        public Func<object, string> DescriptionFactory { get; }

        public IDictionary<string, IPropertyAuditConfiguration> Properties { get; }
    }
}