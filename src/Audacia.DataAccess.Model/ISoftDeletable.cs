using System;

namespace Audacia.DataAccess.Model;

/// <summary>
/// ISoftDeletable interface.
/// </summary>
/// <typeparam name="TUserIdentifier">Struct type <see cref="ISoftDeletable{TUserIdentifier}"/>.</typeparam>
public interface ISoftDeletable<TUserIdentifier>
    where TUserIdentifier : struct
{
    /// <summary>
    /// Gets or sets Deleted.
    /// </summary>
    DateTimeOffset? Deleted { get; set; }

    /// <summary>
    /// Gets or sets DeletedBy.
    /// </summary>
    TUserIdentifier? DeletedBy { get; set; }
}