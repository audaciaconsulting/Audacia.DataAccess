using System;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Specifications.Projection;

/// <summary>
/// Extennsion for <see cref="IProjectionSpecification{T,TResult}"/>.
/// </summary>
public static class ProjectionSpecificationExtensions
{
    /// <summary>
    /// Execute the conversion rules against the given <paramref name="model"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cerf="IProjectionSpecification{T,TRseult}"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cerf="IProjectionSpecification{T,TRseult}"/>.</typeparam>
    /// <param name="projectionSpecification">Instance of <see cerf="IProjectionSpecification{T,TRseult}"/>.</param>
    /// <param name="model">Instance of model of Type T.</param>
    /// <returns><see cref="IProjectionSpecification{T,TResult}"/>.</returns>
    public static TResult Convert<T, TResult>(this IProjectionSpecification<T, TResult> projectionSpecification, T model)
    {
        ArgumentNullException.ThrowIfNull(projectionSpecification);
        return projectionSpecification.Expression.Execute(model);
    }
}