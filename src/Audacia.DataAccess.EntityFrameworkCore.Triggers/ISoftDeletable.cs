using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface ISoftDeletable<TUserIdentifier>
        where TUserIdentifier : struct
    {
        DateTimeOffset? Deleted { get; set; }
        TUserIdentifier? DeletedBy { get; set; }
    }
}