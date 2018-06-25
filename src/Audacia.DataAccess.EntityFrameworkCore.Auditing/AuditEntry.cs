using System.Collections.Generic;
using Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class AuditEntry<TUserIdentifier>
        where TUserIdentifier : struct
    {
        public object[] PrimaryKeyValues { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public AuditStrategy Strategy { get; set; }
        public AuditState State { get; set; }
        public TUserIdentifier? UserIdentifier { get; set; }

        public IDictionary<string, AuditEntryProperty> Properties { get; } =
            new Dictionary<string, AuditEntryProperty>();
    }
}