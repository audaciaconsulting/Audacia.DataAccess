using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface IModifiable<TUserId>
    {
        DateTimeOffset Modified { get; set; }
        TUserId ModifiedBy { get; set; }
    }
}