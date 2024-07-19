using System;

namespace Audacia.DataAccess.Model;

/// <summary>
/// IModifiable interface.
/// </summary>
/// <typeparam name="TUserIdentifier">Struct type of <see cref="IModifiable{TUserIdentifier}"/>.</typeparam>
public interface IModifiable<TUserIdentifier>
    where TUserIdentifier : struct
{
    /// <summary>
    /// Gets or sets Modified.
    /// </summary>
    DateTimeOffset? Modified { get; set; }

    /// <summary>
    /// Gets or sets ModifiedBy.
    /// </summary>
    TUserIdentifier? ModifiedBy { get; set; }
}