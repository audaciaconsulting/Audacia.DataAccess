using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public interface ISoftDeletable<TUserId>
    {
        DateTimeOffset Deleted { get; set; }
        TUserId DeletedBy { get; set; }
    }
}