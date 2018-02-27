using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration
{
    public interface IEntityAuditConfiguration
    {
        IEntityType EntityType { get; }
        bool Ignore { get; }
        AuditStrategy Strategy { get; }
        Func<object, string> DescriptionFactory { get; }
        IDictionary<IProperty, IPropertyAuditConfiguration> Properties { get; }
    }
}