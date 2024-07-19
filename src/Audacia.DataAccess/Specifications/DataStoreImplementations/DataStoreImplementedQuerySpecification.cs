using System.Collections.Generic;
using System.Linq;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.DataStoreImplementations;

/// <summary>
/// Implementation of IDataStoreImplementedQuerySpecification.
/// </summary>
/// <typeparam name="T">Type of <see cref="DataStoreImplementedQuerySpecification{T}"/>.</typeparam>
public class DataStoreImplementedQuerySpecification<T> : IDataStoreImplementedQuerySpecification<T>
{
    private readonly IDictionary<string, QueryParameter> _parameters;

    /// <summary>
    /// Gets Name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets Parameters.
    /// </summary>
    public IEnumerable<QueryParameter> Parameters
    {
        get
        {
            foreach (var propertyInfo in this.GetType().GetProperties().Where(propertyInfo =>
                propertyInfo.IsDefined(typeof(QueryParameterAttribute), true)))
            {
                _parameters[propertyInfo.Name] = new QueryParameter(propertyInfo.Name, propertyInfo.GetValue(this) ?? new { });
            }

            return _parameters.Values;
        }
    }

    /// <summary>
    /// Sets the value of Name.
    /// </summary>
    /// <param name="name">Value of Name.</param>
    public DataStoreImplementedQuerySpecification(string name)
    {
        Name = name;
        _parameters = new Dictionary<string, QueryParameter>();
    }

    /// <summary>
    /// Sets the Name and QueryParameter list.
    /// </summary>
    /// <param name="name">Value of Name.</param>
    /// <param name="parameters">QueryParameter list.</param>
    public DataStoreImplementedQuerySpecification(string name, params QueryParameter[] parameters)
        : this(name)
    {
        _parameters = parameters.ToDictionary(parameter => parameter.ParameterName);
    }

    /// <summary>
    /// Creates a new instance of <see cref="IDataStoreImplementedQuerySpecification{T, TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult">Return type of <see cref="IProjectionSpecification{T, TResult}"/>.</typeparam>
    /// <param name="projectionSpecification"><see cref="IProjectionSpecification{T, TResult}"/>.</param>    
    /// <returns>Instance of <see cref="IDataStoreImplementedQuerySpecification{T, TResult}"/>.</returns>
    public DataStoreImplementedQuerySpecification<T, TResult> WithProjection<TResult>(
        IProjectionSpecification<T, TResult> projectionSpecification)
    {
        return new DataStoreImplementedQuerySpecification<T, TResult>(this, projectionSpecification);
    }
}

/// <summary>
/// Implementation of <see cref="IDataStoreImplementedQuerySpecification{T, TResult}"/>.
/// </summary>
/// <typeparam name="T">Input type of  <see cref="IDataStoreImplementedQuerySpecification{T, TResult}"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="IDataStoreImplementedQuerySpecification{T, TResult}"/>.</typeparam>
public class DataStoreImplementedQuerySpecification<T, TResult> : DataStoreImplementedQuerySpecification<T>, IDataStoreImplementedQuerySpecification<T, TResult>
{
    /// <summary>
    /// Gets Projection.
    /// </summary>
    public IProjectionSpecification<T, TResult> Projection { get; }

    /// <summary>
    /// Creates an instance of <see cref="IDataStoreImplementedQuerySpecification{T}"/>.
    /// </summary>
    /// <param name="fromSpecification"><see cref="IDataStoreImplementedQuerySpecification{T}"/> will be used as the query specification.</param>
    /// <param name="projection"><see cref="IProjectionSpecification{T, TResult}"/> will be used as the projection specification.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Spacing Rules", "SA1010:Opening Square Brackets Must Be Spaced Correctly", Justification = "This is the only way to create an empty array.")]
    public DataStoreImplementedQuerySpecification(IDataStoreImplementedQuerySpecification<T> fromSpecification, IProjectionSpecification<T, TResult> projection)
        : base(fromSpecification == null ? string.Empty : fromSpecification.Name, fromSpecification == null ? [] : fromSpecification.Parameters.ToArray())
    {
        Projection = projection;
    }
}