using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// IPropertyAuditConfiguration interface.
/// </summary>
public interface IPropertyAuditConfiguration
{
    /// <summary>
    /// Gets value of FriendlyValueLookupType.
    /// </summary>
    Type? FriendlyValueLookupType { get; }

    /// <summary>
    /// Gets value of FriendlyValueFactory.
    /// </summary>
    Func<object, string>? FriendlyValueFactory { get; }

    /// <summary>
    /// Gets a value indicating whether audit should be ignored.
    /// </summary>
    bool Ignore { get; }

    /// <summary>
    /// Gets value of FriendlyName.
    /// </summary>
    string FriendlyName { get; }

    /// <summary>
    /// Gets value of Property.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "This is may code already using this..")]
    IProperty Property { get; }
}