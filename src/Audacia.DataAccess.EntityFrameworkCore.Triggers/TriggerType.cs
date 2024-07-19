namespace Audacia.DataAccess.EntityFrameworkCore.Triggers;

/// <summary>
/// TriggerType enum.
/// </summary>
public enum TriggerType
{
    /// <summary>
    /// No specific type.
    /// </summary>
    None,

    /// <summary>
    /// When inserting records.
    /// </summary>
    Inserting,

    /// <summary>
    /// After inserting records.
    /// </summary>
    Inserted,

    /// <summary>
    /// When updating records.
    /// </summary>
    Updating,

    /// <summary>
    /// After updating records.
    /// </summary>
    Updated,

    /// <summary>
    /// When deleting records.
    /// </summary>
    Deleting,

    /// <summary>
    /// After deleing records.
    /// </summary>
    Deleted
}
