namespace Audacia.DataAccess.Specifications.Projection
{
    /// <summary>
    /// Allows a collection of objects of type <see cref="T"/> to be filtered, converted to objects
    /// of type <see cref="TResult"/> and have navigation properties included in results 
    /// by exposing an <see cref="IProjectionSpecification{T,TResult}"/> 
    /// in addition to the functionality provided by a <see cref="IQuerySpecification{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IProjectableQuerySpecification<T, TResult> : IQuerySpecification<T>
    {
        /// <summary>
        /// Gets the <see cref="IProjectionSpecification{T,TResult}"/> containing the rules
        /// to convert from an object of type <see cref="T"/> to an object of type <see cref="TResult"/>.
        /// </summary>
        IProjectionSpecification<T, TResult> Projection { get; }
    }
}