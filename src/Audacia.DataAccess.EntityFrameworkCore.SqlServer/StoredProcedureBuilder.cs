using System;
using System.Collections.Generic;
using System.Linq;
using Audacia.DataAccess.Specifications.DataStoreImplementations;

namespace Audacia.DataAccess.EntityFrameworkCore.SqlServer;

/// <summary>
/// Helper methods to build Stored Procedures.
/// </summary>
public class StoredProcedureBuilder
{
    /// <summary>
    /// Gets the query text from <see cref="IDataStoreImplementedQuerySpecification{T}"/>. 
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IDataStoreImplementedQuerySpecification{T}"/>.</typeparam>
    /// <param name="specification">Specification instance.</param>
    /// <returns>QueryTest string.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Changing this will add breaking changes.")]
    public string GetQueryText<T>(IDataStoreImplementedQuerySpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);

        return $"EXECUTE [{specification.Name}] {GetParametersText(specification.Parameters)}";
    }

    private static string GetParametersText(IEnumerable<QueryParameter> parameters)
    {
        return string.Join(" ", parameters.Select(p => $"@{p.ParameterName} = {p.ParameterValue}"));
    }
}