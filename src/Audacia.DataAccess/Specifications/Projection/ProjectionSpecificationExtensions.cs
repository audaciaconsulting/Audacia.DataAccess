using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Specifications.Projection;

public static class ProjectionSpecificationExtensions
{
    /// <summary>
    /// Execute the conversion rules against the given <paramref name="model"/>.
    /// </summary>
    /// <param name="projectionSpecification"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static TResult Convert<T, TResult>(this IProjectionSpecification<T, TResult> projectionSpecification, T model)
    {
        return projectionSpecification.Expression.Execute(model);
    }
}