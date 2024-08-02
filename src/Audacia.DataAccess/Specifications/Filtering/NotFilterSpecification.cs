using System;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Specifications.Filtering;

/// <summary>
/// Negates an <see cref="IFilterSpecification{T}"/> into a new <see cref="IFilterSpecification{T}"/> object.
/// </summary>
/// <typeparam name="T">The type of entity to be filtered.</typeparam>
public class NotFilterSpecification<T> : IFilterSpecification<T>
{
    /// <summary>
    /// Gets the <see cref="Expression{T}"/> that contains the filtering rule(s).
    /// </summary>
    public Expression<Func<T, bool>> Expression { get; }

    /// <summary>
    /// Constructor which takes an instance of <see cref="IFilterSpecification{T}"/>.
    /// </summary>
    /// <param name="specificationToNegate">Instance of <see cref="IFilterSpecification{T}"/>.</param>
    public NotFilterSpecification(IFilterSpecification<T> specificationToNegate)
    {
        ArgumentNullException.ThrowIfNull(specificationToNegate, nameof(specificationToNegate));

        Expression = specificationToNegate.Expression.Not();
    }
}