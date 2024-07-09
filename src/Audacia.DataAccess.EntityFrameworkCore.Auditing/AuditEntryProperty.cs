namespace Audacia.DataAccess.EntityFrameworkCore.Auditing;

/// <summary>
/// AuditEntryProperty class.
/// </summary>
public class AuditEntryProperty
{
    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets FriendlyName.
    /// </summary>
    public string FriendlyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets OldValue.
    /// </summary>
    public object OldValue { get; set; } = new object();

    /// <summary>
    /// Gets or sets NewValue.
    /// </summary>
    public object NewValue { get; set; } = new object();

    /// <summary>
    /// Gets or sets FriendlyOldValue.
    /// </summary>
    public string FriendlyOldValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets FriendlyNewValue.
    /// </summary>
    public string FriendlyNewValue { get; set; } = string.Empty;
}