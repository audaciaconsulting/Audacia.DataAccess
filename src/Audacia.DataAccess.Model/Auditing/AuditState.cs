namespace Audacia.DataAccess.Model.Auditing;

/// <summary>
/// Audit state enums.
/// </summary>
public enum AuditState
{
    /// <summary>
    /// Shows audit record has been added.
    /// </summary>
    Added,

    /// <summary>
    /// Shows audit record has been modified.
    /// </summary>
    Modified,

    /// <summary>
    /// Shows audit record has been deleted.
    /// </summary>
    Deleted
}