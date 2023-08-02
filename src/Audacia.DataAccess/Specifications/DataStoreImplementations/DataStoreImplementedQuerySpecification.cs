using System.Collections.Generic;
using System.Linq;
using Audacia.DataAccess.Specifications.Projection;

namespace Audacia.DataAccess.Specifications.DataStoreImplementations;

public class DataStoreImplementedQuerySpecification<T> : IDataStoreImplementedQuerySpecification<T>
{
    private readonly IDictionary<string, QueryParameter> _parameters;

    public string Name { get; }

    public IEnumerable<QueryParameter> Parameters
    {
        get
        {
            foreach (var propertyInfo in this.GetType().GetProperties().Where(propertyInfo =>
                propertyInfo.IsDefined(typeof(QueryParameterAttribute), true)))
            {
                _parameters[propertyInfo.Name] = new QueryParameter(propertyInfo.Name, propertyInfo.GetValue(this));
            }

            return _parameters.Values;
        }
    }

    public DataStoreImplementedQuerySpecification(string name)
    {
        Name = name;
    }

    public DataStoreImplementedQuerySpecification(string name, params QueryParameter[] parameters)
        : this(name)
    {
        _parameters = parameters.ToDictionary(parameter => parameter.ParameterName);
    }

    public DataStoreImplementedQuerySpecification<T, TResult> WithProjection<TResult>(
        IProjectionSpecification<T, TResult> projectionSpecification)
    {
        return new DataStoreImplementedQuerySpecification<T, TResult>(this, projectionSpecification);
    }
}

public class DataStoreImplementedQuerySpecification<T, TResult> : DataStoreImplementedQuerySpecification<T>, IDataStoreImplementedQuerySpecification<T, TResult>
{
    public IProjectionSpecification<T, TResult> Projection { get; }

    public DataStoreImplementedQuerySpecification(IDataStoreImplementedQuerySpecification<T> fromSpecification, IProjectionSpecification<T, TResult> projection)
        : base(fromSpecification.Name, fromSpecification.Parameters.ToArray())
    {
        Projection = projection;
    }
}