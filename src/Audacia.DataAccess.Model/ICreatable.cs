using System;

namespace Audacia.DataAccess.Model;

public interface ICreatable<TUserIdentifier>
    where TUserIdentifier : struct
{
    DateTimeOffset Created { get; set; }
    TUserIdentifier? CreatedBy { get; set; }
}