using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface ISoftDeletable<TUserId>
        where TUserId : struct
    {
        DateTimeOffset? Deleted { get; set; }
        TUserId? DeletedBy { get; set; }
    }
}