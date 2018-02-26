using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public interface ICreatable<TUserId>
    {
        DateTimeOffset Created { get; set; }
        TUserId CreatedBy { get; set; }
    }
}