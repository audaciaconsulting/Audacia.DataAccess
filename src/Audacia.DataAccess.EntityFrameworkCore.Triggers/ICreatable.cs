using System;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public interface ICreatable<TUserIdentifier>
        where TUserIdentifier : struct
    {
        DateTimeOffset Created { get; set; }
        TUserIdentifier? CreatedBy { get; set; }
    }
}