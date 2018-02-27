using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface ISoftDeletable<TUserId>
    {
        DateTimeOffset Deleted { get; set; }
        TUserId DeletedBy { get; set; }
    }
}