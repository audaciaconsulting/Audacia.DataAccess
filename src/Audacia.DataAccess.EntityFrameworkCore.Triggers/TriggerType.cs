namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public enum TriggerType
    {
        None,
        Inserting,
        Inserted,
        Updating,
        Updated,
        Deleting,
        Deleted
    }
}