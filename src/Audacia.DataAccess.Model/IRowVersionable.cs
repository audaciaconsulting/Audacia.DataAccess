using System.Collections.Generic;

namespace Audacia.DataAccess.Model;

/// <summary>
/// IRowVersionable interface.
/// </summary>
public interface IRowVersionable
{
    /// <summary>
    /// Gets or sets RowVersion.
    /// </summary>
    IEnumerable<byte> RowVersion { get; set; }
}