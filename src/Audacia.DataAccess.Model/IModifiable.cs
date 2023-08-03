using System;

namespace Audacia.DataAccess.Model;

public interface IModifiable<TUserIdentifier>
    where TUserIdentifier : struct
{
    DateTimeOffset? Modified { get; set; }

    TUserIdentifier? ModifiedBy { get; set; }
}