using System;
using System.Linq.Expressions;
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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projectionSpecification"></param>
    /// <returns></returns>
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(IProjectionSpecification<T, TResult> projectionSpecification) where T : class
    {
        return new ProjectableQuerySpecification<T, TResult>(projectionSpecification);
    }

    /// <summary>
    /// Creates a <see cref="ProjectableQuerySpecification{T,TResult}"/> instance with an <see cref="IProjectionSpecification{T,TResult}"/>
    /// based on the given <paramref name="projectionExpression"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projectionExpression"></param>
    /// <returns></returns>
    public static ProjectableQuerySpecification<T, TResult> WithProjection<T, TResult>(Expression<Func<T, TResult>> projectionExpression) where T : class
    {
        return new ProjectableQuerySpecification<T, TResult>(new DynamicProjectionSpecification<T, TResult>(projectionExpression));
    }
}

/// <summary>
/// Default implementation of <see cref="IProjectableQuerySpecification{T,TResult}"/> that
/// allows a collection of objects of type <see cref="T"/> to be filtered, converted to objects
/// of type <see cref="TResult"/> and have navigation properties included in results .
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class ProjectableQuerySpecification<T, TResult> : IProjectableQuerySpecification<T, TResult> where T : class
{
    public IFilterSpecification<T> Filter { get; set; }
    public IIncludeSpecification<T> Include { get; set; }
    public IProjectionSpecification<T, TResult> Projection { get; set; }

    public ProjectableQuerySpecification(IProjectionSpecification<T, TResult> projectionSpecification)
    {
        Projection = projectionSpecification;
    }

    public ProjectableQuerySpecification(IQuerySpecification<T> startFrom,
        IProjectionSpecification<T, TResult> projectionSpecification)
    {
        Filter = startFrom.Filter;
        Include = startFrom.Include;
        Projection = projectionSpecification;
    }
}