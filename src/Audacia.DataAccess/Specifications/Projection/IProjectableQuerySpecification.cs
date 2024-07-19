namespace Audacia.DataAccess.Specifications.Projection;

/// <summary>
/// Allows a collection of objects of type <see cref="IQuerySpecification{T}"/> to be filtered, converted to objects
/// of type <see cref="IProjectableQuerySpecification{T,TResult}"/> and have navigation properties included in results 
/// by exposing an <see cref="IProjectionSpecification{T,TResult}"/> 
/// in addition to the functionality provided by a <see cref="IQuerySpecification{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IProjectableQuerySpecification{T,TResult}"/>. </typeparam>
/// <typeparam name="TResult">Return type of <see cref="IProjectableQuerySpecification{T,TResult}"/>.</typeparam>
public interface IProjectableQuerySpecification<T, TResult> : IQuerySpecification<T>
{
    /// <summary>
    /// Gets the <see cref="IProjectionSpecification{T,TResult}"/> containing the rules
    /// to convert from an object of type <see cref="IProjectionSpecification{T,TResult}"/> to an object of type <see cref="IProjectionSpecification{T,TResult}"/>.
    /// </summary>
    IProjectionSpecification<T, TResult>? Projection { get; }
}