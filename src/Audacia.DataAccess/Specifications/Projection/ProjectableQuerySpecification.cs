using System;
using System.Linq.Expressions;
using Audacia.Core;
using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;

namespace Audacia.DataAccess.Specifications.Projection;

/// <summary>
/// Convenience class to allow <see cref="ProjectableQuerySpecification{T,TResult}"/> instances to be created using type inference
/// so generic parameters don't have to be specified explicitly.
/// </summary>
public static class ProjectableQuerySpecification
{
    /// <summary>
    /// Creates a <see cref="ProjectableQuerySpecification{T,TResult}"/> instance with the given <paramref name="projectionSpecification"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IProjectionSpecification{T,TResult}"/>. </typeparam>
    /// <typeparam name="TResult">Return type of <see cref="IProjectionSpecification{T,TResult}"/>.</typeparam>
    /// <param name="projectionSpecification">Instance of <see cref="IProjectionSpecification{T,TResult}"/>.</param>
    /// <returns><see cref="IProjectionSpecification{T,TResult}"/>.</returns>
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(IProjectionSpecification<T, TResult> projectionSpecification) where T : class
    {
        return new ProjectableQuerySpecification<T, TResult>(projectionSpecification);
    }

    /// <summary>
    /// Creates a <see cref="ProjectableQuerySpecification{T,TResult}"/> instance with an <see cref="IProjectionSpecification{T,TResult}"/>
    /// based on the given <paramref name="projectionExpression"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Expression{T}"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="Expression{T}"/>.</typeparam>
    /// <param name="projectionExpression">An instance of <see cref="Expression{T}"/>.</param>
    /// <returns><see cref="ProjectableQuerySpecification{T, TResult}"/>.</returns>
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(Expression<Func<T, TResult>> projectionExpression) where T : class
    {
        return new ProjectableQuerySpecification<T, TResult>(new DynamicProjectionSpecification<T, TResult>(projectionExpression));
    }
}

/// <summary>
/// Default implementation of <see cref="IProjectableQuerySpecification{T,TResult}"/> that
/// allows a collection of objects of type <see cref="IProjectableQuerySpecification{T,TResult}"/> to be filtered, converted to objects
/// of type <see cref="IProjectableQuerySpecification{T,TResult}"/> and have navigation properties included in results .
/// </summary>
/// <typeparam name="T">Type of  <see cref="IProjectableQuerySpecification{T,TResult}"/>.</typeparam>
/// <typeparam name="TResult">Return type of  <see cref="IProjectableQuerySpecification{T,TResult}"/>.</typeparam>
public class ProjectableQuerySpecification<T, TResult> : IProjectableQuerySpecification<T, TResult> where T : class
{
    /// <summary>
    /// Gets or sets Filter.
    /// </summary>
    public IFilterSpecification<T>? Filter { get; set; }

    /// <summary>
    /// Gets or sets Include.
    /// </summary>
    public IIncludeSpecification<T>? Include { get; set; }

    /// <summary>
    /// Gets or sets Projection.
    /// </summary>
    public IProjectionSpecification<T, TResult> Projection { get; set; }

    /// <summary>
    /// Constructor takes in projectionSpecification.
    /// </summary>
    /// <param name="projectionSpecification">Instance of projectionSpecification.</param>
    public ProjectableQuerySpecification(IProjectionSpecification<T, TResult> projectionSpecification)
    {
        ArgumentNullException.ThrowIfNull(projectionSpecification, nameof(projectionSpecification));

        Projection = projectionSpecification;
    }

    /// <summary>
    /// Constructor takes in <see cref="IQuerySpecification{T}"/> projectionSpecification and <see cref="IProjectionSpecification{T,Result}"/> projectionSpecification.
    /// </summary>
    /// <param name="startFrom">Instance of <see cref="IQuerySpecification{T}"/>.</param>
    /// <param name="projectionSpecification">Instance of <see cref="IProjectionSpecification{T,Result}"/>.</param>
    public ProjectableQuerySpecification(
        IQuerySpecification<T> startFrom,
        IProjectionSpecification<T, TResult> projectionSpecification)
    {
        ArgumentNullException.ThrowIfNull(projectionSpecification, nameof(projectionSpecification));
        ArgumentNullException.ThrowIfNull(startFrom, nameof(startFrom));

        Filter = startFrom.Filter;
        Include = startFrom.Include;

        Projection = projectionSpecification;
    }
}