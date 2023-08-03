using System.Collections;
using System.Collections.Generic;

namespace Audacia.DataAccess.Specifications.Including;

public class IncludeStepPath : IEnumerable<IncludeStep>
{
    private readonly ICollection<IncludeStep> _internalCollection = new List<IncludeStep>();
    
    public IEnumerator<IncludeStep> GetEnumerator() => _internalCollection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(IncludeStep item) => _internalCollection.Add(item);
}