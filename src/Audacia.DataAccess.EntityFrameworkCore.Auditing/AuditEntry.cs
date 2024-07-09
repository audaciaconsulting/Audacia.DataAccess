using System.Collections.Generic;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;
using Audacia.DataAccess.Model.Auditing;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

/// <summary>
/// AuditEntry class.
/// </summary>
/// <typeparam name="TUserIdentifier">UserIdentifier struct stype.</typeparam>
public class AuditEntry<TUserIdentifier>
    where TUserIdentifier : struct
{
    /// <summary>
    /// Gets or sets PrimaryKeyValues.
    /// </summary>
    public IEnumerable<object> PrimaryKeyValues { get; set; } = new List<object>();

    /// <summary>
    /// Gets or sets FullName.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets ShortName.
    /// </summary>
    public string ShortName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets FriendlyName.
    /// </summary>
    public string FriendlyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets Strategy.
    /// </summary>
    public AuditStrategy? Strategy { get; set; }

    /// <summary>
    /// Gets or sets State.
    /// </summary>
    public AuditState? State { get; set; }

    /// <summary>
    /// Gets or sets UserIdentifier.
    /// </summary>
    public TUserIdentifier? UserIdentifier { get; set; }

    /// <summary>
    /// Gets Properties.
    /// </summary>
    public IDictionary<string, AuditEntryProperty> Properties { get; } =
        new Dictionary<string, AuditEntryProperty>();
}