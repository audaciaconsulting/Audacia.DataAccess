using System;
using System.Collections.Generic;

namespace Audacia.DataAccess.Specifications.Including;

public class DynamicIncludeSpecification<T> : IIncludeSpecification<T>
{
    private readonly IBuildableIncludeSpecification<T> _wrappedSpecification;

    public DynamicIncludeSpecification(Action<IBuildableIncludeSpecification<T>> includeAction)
    {
        _wrappedSpecification = IncludeSpecification<T>.CreateInternal();

        includeAction(_wrappedSpecification);
    }

    public IEnumerable<IncludeStepPath> IncludeStepPaths => _wrappedSpecification.IncludeStepPaths;
}