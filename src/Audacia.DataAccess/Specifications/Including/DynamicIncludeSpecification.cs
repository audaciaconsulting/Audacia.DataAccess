using System;
using System.Collections.Generic;

namespace Audacia.DataAccess.Specifications.Including;

/// <summary>
/// Combines two or more <see cref="IBuildableIncludeSpecification{T}"/> objects into a single <see cref="IBuildableIncludeSpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="DynamicIncludeSpecification{T}"/>.</typeparam>
public class DynamicIncludeSpecification<T> : IIncludeSpecification<T>
{
    private readonly IBuildableIncludeSpecification<T> _wrappedSpecification;

    /// <summary>
    /// Constructor which takes in an instance of <see cref="IBuildableIncludeSpecification{T}"/>.
    /// </summary>
    /// <param name="includeAction">Instance of <see cref="IBuildableIncludeSpecification{T}"/>. </param>
    public DynamicIncludeSpecification(Action<IBuildableIncludeSpecification<T>> includeAction)
    {
        _wrappedSpecification = IncludeSpecification<T>.CreateInternal();

        if (includeAction != null)
        {
            includeAction(_wrappedSpecification);
        }
    }

    /// <summary>
    /// Gets the default value of <see cref="IEnumerable{IncludeStepPath}"/>.
    /// </summary>
    public IEnumerable<IncludeStepPath> IncludeStepPaths => _wrappedSpecification.IncludeStepPaths;
}