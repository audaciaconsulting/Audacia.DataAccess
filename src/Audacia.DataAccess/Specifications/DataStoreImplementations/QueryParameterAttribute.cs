using System;

namespace Audacia.DataAccess.Specifications.DataStoreImplementations;

/// <summary>
/// This class extends <see cref="Attribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class QueryParameterAttribute : Attribute
{
}