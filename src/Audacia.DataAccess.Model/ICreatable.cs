using System;

namespace Audacia.DataAccess.Model;

/// <summary>
/// ICreatable interfcae.
/// </summary>
/// <typeparam name="TUserIdentifier">Struct type of <see cref="ICreatable{TUserIdentifier}"/>.</typeparam>
public interface ICreatable<TUserIdentifier>
    where TUserIdentifier : struct
{
    /// <summary>
    /// Gets or sets Created.
    /// </summary>
    DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets CreatedBy.
    /// </summary>
    TUserIdentifier? CreatedBy { get; set; }
}