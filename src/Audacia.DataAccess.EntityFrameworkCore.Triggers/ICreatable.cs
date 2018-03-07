using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface ICreatable<TUserId>
    {
        DateTimeOffset Created { get; set; }
        TUserId CreatedBy { get; set; }
    }
}