using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including;

/// <summary>
/// This implemnts <see cref="IBuildableIncludeSpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IBuildableIncludeSpecification{T}"/>.</typeparam>
public class IncludeSpecification<T> : IBuildableIncludeSpecification<T>
{
    private readonly List<IncludeStepPath> _includeStepPaths = new List<IncludeStepPath>();

    /// <summary>
    /// Empty constructor.
    /// </summary>
    protected IncludeSpecification()
    {
    }

    /// <summary>
    /// Creates and return a new instance of <see cref="IncludeSpecification{T}"/>.
    /// </summary>
    /// <returns>New instance of <see cref="IncludeSpecification{T}"/>.</returns>
    internal static IncludeSpecification<T> CreateInternal()
    {
        return new IncludeSpecification<T>();
    }

    /// <summary>
    /// Creates a new <see cref="IncludeSpecification{T}"/> and include all passed in <see cref="IIncludeSpecification{T}"/>.
    /// </summary>
    /// <param name="includeSpecifications"><see cref="IIncludeSpecification{T}"/> to be included in the <see cref="IncludeSpecification{T}"/>.</param>
    /// <returns>New instance of <see cref="IncludeSpecification{T}"/>.</returns>
    internal static IncludeSpecification<T> From(params IIncludeSpecification<T>[] includeSpecifications)
    {
        var specification = CreateInternal();
        foreach (var fromSpecification in includeSpecifications)
        {
            specification._includeStepPaths.AddRange(fromSpecification.IncludeStepPaths);
        }

        return specification;
    }

    /// <summary>
    /// Gets <see cref="IncludeSpecification{T}"/>.
    /// </summary>
    public IEnumerable<IncludeStepPath> IncludeStepPaths => _includeStepPaths;

    /// <summary>
    /// Adds <see cref="Expression{TKey}"/> to the list of  <see cref="IncludeStepPath"/>.
    /// </summary>
    /// <typeparam name="TKey">Return type of  <see cref="Expression{TKey}"/>.</typeparam>
    /// <param name="keySelector"><see cref="Expression{T}"/> needs to be included in <see cref="IncludeSpecification{T}"/>.</param>
    /// <returns>New instance of <see cref="ThenInclude{T}"/>.</returns>
    public IThenInclude<TKey> With<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        var path = new IncludeStepPath
        {
            new IncludeStep(keySelector)
        };

        _includeStepPaths.Add(path);
        
        return new ThenInclude<TKey>(path);
    }

    /// <summary>
    /// Adds (collection of) <see cref="Expression{TKey}"/> to the list of  <see cref="IncludeStepPath"/>.
    /// </summary>
    /// <typeparam name="TKey">Return type of  <see cref="Expression{TKey}"/>.</typeparam>
    /// <param name="keySelector"><see cref="Expression{T}"/> needs to be included in <see cref="IncludeSpecification{T}"/>.</param>
    /// <returns>New instance of <see cref="ThenInclude{T}"/>.</returns>
    public IThenInclude<TKey> With<TKey>(Expression<Func<T, ICollection<TKey>>> keySelector)
    {
        var path = new IncludeStepPath
        {
            new IncludeStep(keySelector)
        };

        _includeStepPaths.Add(path);
        
        return new ThenInclude<TKey>(path);
    }

    private class ThenInclude<TThen> : IThenInclude<TThen>
    {
        private readonly IncludeStepPath _includeStepPath;

        internal ThenInclude(IncludeStepPath includeStepPath)
        {
            _includeStepPath = includeStepPath;
        }

        public IThenInclude<TKey> Then<TKey>(Expression<Func<TThen, TKey>> keySelector)
        {
            _includeStepPath.Add(new IncludeStep(keySelector));

            return new ThenInclude<TKey>(_includeStepPath);
        }

        public IThenInclude<TKey> Then<TKey>(Expression<Func<TThen, ICollection<TKey>>> keySelector)
        {
            _includeStepPath.Add(new IncludeStep(keySelector));

            return new ThenInclude<TKey>(_includeStepPath);
        }
    }
}