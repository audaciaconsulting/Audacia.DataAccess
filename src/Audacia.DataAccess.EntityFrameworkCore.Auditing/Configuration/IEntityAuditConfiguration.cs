using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

public interface IEntityAuditConfiguration
{
    IEntityType EntityType { get; }
    bool Ignore { get; }
    AuditStrategy Strategy { get; }
    string FriendlyName { get; }
    Func<object, string> DescriptionFactory { get; }
    IDictionary<string, IPropertyAuditConfiguration> Properties { get; }
}