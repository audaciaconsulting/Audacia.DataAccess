using System;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Specifications.Filtering;

/// <summary>
/// Provides a set of static methods to combine and create new <see cref="IFilterSpecification{T}"/> objects.
/// </summary>
public static class FilterSpecificationExtensions
{
    /// <summary>
    /// Execute the filter rule(s) against the given <paramref name="model"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filterSpecification"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static bool IsSatisfiedBy<T>(this IFilterSpecification<T> filterSpecification, T model)
    {
        return filterSpecification.Expression.Execute(model);
    }

    /// <summary>
    /// Combines the given <see cref="IFilterSpecification{T}"/> objects into a new <see cref="IFilterSpecification{T}"/> 
    /// using the logical AND operator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static IFilterSpecification<T> And<T>(this IFilterSpecification<T> left, IFilterSpecification<T> right) where T : class
    {
        return new AndFilterSpecification<T>(left, right);
    }

    /// <summary>
    /// Combines the given <see cref="IFilterSpecification{T}"/> and <see cref="Expression{TDelegate}"/> 
    /// into a new <see cref="IFilterSpecification{T}"/> using the logical AND operator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static IFilterSpecification<T> And<T>(this IFilterSpecification<T> left, Expression<Func<T, bool>> right) where T : class
    {
        return new AndFilterSpecification<T>(left, right);
    }

    /// <summary>
    /// Combines the given <see cref="IFilterSpecification{T}"/> objects into a new <see cref="IFilterSpecification{T}"/> 
    /// using the logical OR operator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static IFilterSpecification<T> Or<T>(this IFilterSpecification<T> left, IFilterSpecification<T> right) where T : class
    {
        return new OrFilterSpecification<T>(left, right);
    }

    /// <summary>
    /// Combines the given <see cref="IFilterSpecification{T}"/> and <see cref="Expression{TDelegate}"/> 
    /// into a new <see cref="IFilterSpecification{T}"/> using the logical OR operator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static IFilterSpecification<T> Or<T>(this IFilterSpecification<T> left, Expression<Func<T, bool>> right) where T : class
    {
        return new OrFilterSpecification<T>(left, right);
    }

    /// <summary>
    /// Negates the given <see cref="IFilterSpecification{T}"/> and returns the negation as a new <see cref="IFilterSpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="specification"></param>
    /// <returns></returns>
    public static IFilterSpecification<T> Not<T>(this IFilterSpecification<T> specification) where T : class
    {
        return new NotFilterSpecification<T>(specification);
    }
}