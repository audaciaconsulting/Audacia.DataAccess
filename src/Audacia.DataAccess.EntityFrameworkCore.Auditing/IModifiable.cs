using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public interface IModifiable<TUserId>
    {
        DateTimeOffset Modified { get; set; }
        TUserId ModifiedBy { get; set; }
    }
}