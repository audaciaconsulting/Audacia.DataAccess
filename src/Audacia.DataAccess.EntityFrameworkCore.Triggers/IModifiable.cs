using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface IModifiable<TUserIdentifier>
        where TUserIdentifier : struct
    {
        DateTimeOffset? Modified { get; set; }
        TUserIdentifier? ModifiedBy { get; set; }
    }
}