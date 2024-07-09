using System.Collections.Generic;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.DataStoreImplementations;

/// <summary>
/// Query specification for data store provided queries, such as stored procedures.
/// </summary>
/// <typeparam name="T">Type of <see cref="IDataStoreImplementedQuerySpecification{T}"/> instance.</typeparam>
public interface IDataStoreImplementedQuerySpecification<T>
{
    /// <summary>
    /// Gets the name of the query to execute.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the parameters to be passed to the data store query.
    /// </summary>
    IEnumerable<QueryParameter> Parameters { get; }
}

/// <summary>
/// Query specification for data store provided queries, such as stored procedures.
/// </summary>
/// <typeparam name="T">Type of <see cref="IDataStoreImplementedQuerySpecification{T}"/> instance.</typeparam>
/// <typeparam name="TResult">Output type of <see cref="IDataStoreImplementedQuerySpecification{T}"/> instance.</typeparam>
public interface IDataStoreImplementedQuerySpecification<T, TResult> : IDataStoreImplementedQuerySpecification<T>
{
    /// <summary>
    /// Gets the <see cref="IProjectionSpecification{T,TResult}"/> containing the rules
    /// to convert from an object of type <see cref="IProjectionSpecification{T, TResult}"/> to an object of type <see cref="IProjectionSpecification{T, TResult}"/>.
    /// </summary>
    IProjectionSpecification<T, TResult> Projection { get; }
}