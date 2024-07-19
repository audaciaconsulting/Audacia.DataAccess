using System.Collections;
using System.Collections.Generic;

namespace Audacia.DataAccess.Specifications.Including;

/// <summary>
/// Returns a collection of <see cref="IncludeStepPath"/>.
/// </summary>
public class IncludeStepPath : IEnumerable<IncludeStep>
{
    private readonly ICollection<IncludeStep> _internalCollection = new List<IncludeStep>();

    /// <summary>
    /// Creates a new collection of  <see cref="IncludeStepPath"/>.
    /// </summary>
    /// <returns>Collection of <see cref="IncludeStepPath"/>.</returns>
    public IEnumerator<IncludeStep> GetEnumerator() => _internalCollection.GetEnumerator();

    /// <summary>
    /// Gets an Enumerator for <see cref="IncludeStepPath"/>.
    /// </summary>
    /// <returns><see cref="IEnumerator"/>.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Adds an instance of <see cref="IncludeStep"/> to the internal collection.
    /// </summary>
    /// <param name="item">Instance of <see cref="IncludeStep"/>.</param>
    public void Add(IncludeStep item) => _internalCollection.Add(item);
}