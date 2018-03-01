namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class AuditEntryProperty
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string FriendlyOldValue { get; set; }
        public string FriendlyNewValue { get; set; }
    }
}