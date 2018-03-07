using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface IModifiable<TUserId>
        where TUserId : struct
    {
        DateTimeOffset? Modified { get; set; }
        TUserId? ModifiedBy { get; set; }
    }
}