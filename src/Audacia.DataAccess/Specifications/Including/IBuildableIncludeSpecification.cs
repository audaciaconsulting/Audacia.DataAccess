namespace Audacia.DataAccess.Specifications.Including;

/// <summary>
/// Extends <see cref="IIncludeSpecification{T}"/> and <see cref="IInclude{T}"/> Interfaces.
/// </summary>
/// <typeparam name="T">Type of <see cref="IBuildableIncludeSpecification{T}"/>. </typeparam>
public interface IBuildableIncludeSpecification<T> : IIncludeSpecification<T>, IInclude<T>
{
}