using System;

namespace Audacia.DataAccess.Specifications.DataStoreImplementations;

/// <summary>
/// Implements <see cref="IEquatable{QueryParameter}"/> to check equality of QueryParameters.
/// </summary>
public class QueryParameter : IEquatable<QueryParameter>
{
    /// <summary>
    /// Gets <see cref="ParameterName"/>.
    /// </summary>
    public string ParameterName { get; }

    /// <summary>
    /// Gets <see cref="ParameterValue"/>.
    /// </summary>
    public object ParameterValue { get; }

    /// <summary>
    /// Constructor which sets <see cref="ParameterName"/> and <see cref="ParameterValue"/>..
    /// </summary>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="parameterValue">Value of the parameter.</param>
    internal QueryParameter(string parameterName, object parameterValue)
    {
        ParameterName = parameterName;
        ParameterValue = parameterValue;
    }

    /// <summary>
    /// Check the <see cref="QueryParameter"/> equal to the exisiting instance.
    /// </summary>
    /// <param name="other">Instance of <see cref="QueryParameter"/>. </param>
    /// <returns>Result of comparison.</returns>
    public bool Equals(QueryParameter? other)
    {
        if (other is null) 
        { 
            return false; 
        }

        if (ReferenceEquals(this, other)) 
        { 
            return true; 
        }

        return string.Equals(ParameterName, other.ParameterName, StringComparison.Ordinal) && Equals(ParameterValue, other.ParameterValue);
    }

    /// <summary>
    /// Check the passed in Object is equal to the exisitng instance of <see cref="QueryParameter"/>.
    /// </summary>
    /// <param name="obj">Instance of an object.</param>
    /// <returns>Result of comparison.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null) 
        {
            return false; 
        }

        if (ReferenceEquals(this, obj)) 
        { 
            return true; 
        }

        return obj.GetType() == GetType() && Equals((QueryParameter)obj);
    }

    /// <summary>
    /// Gets the hashcode value of <see cref="QueryParameter"/>.
    /// </summary>
    /// <returns>Return hashcode value.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return ((ParameterName?.GetHashCode(StringComparison.Ordinal) ?? 0) * 397) ^
                   (ParameterValue?.GetHashCode() ?? 0);
        }
    }
}