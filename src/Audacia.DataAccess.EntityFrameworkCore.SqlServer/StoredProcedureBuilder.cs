using System.Collections.Generic;
using System.Linq;
using Audacia.DataAccess.Specifications.DataStoreImplementations;

namespace Audacia.Core.DataAcess.EntityFrameworkCore.SqlServer
{
    public class StoredProcedureBuilder
    {
        public string GetQueryText<T>(IDataStoreImplementedQuerySpecification<T> specification)
        {
            return $"EXECUTE [{specification.Name}] {GetParametersText(specification.Parameters)}";
        }

        private static string GetParametersText(IEnumerable<QueryParameter> parameters)
        {
            return string.Join(" ", parameters.Select(p => $"@{p.ParameterName} = {p.ParameterValue}"));
        }
    }
}