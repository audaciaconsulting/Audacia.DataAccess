using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// IEntityAuditConfiguration interface.
/// </summary>
public interface IEntityAuditConfiguration
{
    /// <summary>
    /// Gets EntityType.
    /// </summary>
    IEntityType EntityType { get; }

    /// <summary>
    /// Gets a value indicating whether audit should be ignored.
    /// </summary>
    bool Ignore { get; }

    /// <summary>
    /// Gets value of Strategy.
    /// </summary>
    AuditStrategy Strategy { get; }

    /// <summary>
    /// Gets value of FriendlyName.
    /// </summary>
    string FriendlyName { get; }

    /// <summary>
    /// Gets value of DescriptionFactory.
    /// </summary>
    Func<object, string> DescriptionFactory { get; }

    /// <summary>
    /// Gets value of Properties.
    /// </summary>
    IDictionary<string, IPropertyAuditConfiguration> Properties { get; }
}