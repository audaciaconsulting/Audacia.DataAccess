using Audacia.DataAccess.Specifications.Filtering;
using Audacia.DataAccess.Specifications.Including;

namespace Audacia.DataAccess.Specifications;

/// <summary>
/// Exposes the basic specifications to allow a collection of objects of type <see cref="T"/>
/// to be filtered and have navigation properties included in result sets.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IQuerySpecification<T>
{
    /// <summary>
    /// Gets the <see cref="IFilterSpecification{T}"/> containing filtering rules.
    /// </summary>
    IFilterSpecification<T> Filter { get; set; }

    /// <summary>
    /// Gets the <see cref="IIncludeSpecification{T}"/> containing rules to
    /// include navigation properties in result sets.
    /// </summary>
    IIncludeSpecification<T> Include { get; set; }
}