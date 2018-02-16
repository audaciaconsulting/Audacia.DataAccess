using System;

namespace Audacia.DataAccess.Specifications.DataStoreImplementations
{
    public class QueryParameter : IEquatable<QueryParameter>
    {
        public string ParameterName { get; }

        public object ParameterValue { get; }

        internal QueryParameter(string parameterName, object parameterValue)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public bool Equals(QueryParameter other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(ParameterName, other.ParameterName) && Equals(ParameterValue, other.ParameterValue);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((QueryParameter) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ParameterName != null ? ParameterName.GetHashCode() : 0) * 397) ^
                       (ParameterValue != null ? ParameterValue.GetHashCode() : 0);
            }
        }
    }
}