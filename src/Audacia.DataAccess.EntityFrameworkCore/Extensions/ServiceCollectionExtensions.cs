using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Audacia.DataAccess.EntityFrameworkCore.Extensions;

/// <summary>
/// Extensions to <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds <see cref="IQueryable{T}"/>s (which will resolve to the equivalent <see cref="DbSet{T}"/> )to the DI container so that they can be injected directly into classes.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns><see cref="IServiceCollection"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("member-design", "AV1130:Return interfaces to unchangeable collections", Justification = "This is an extension method.")]
    public static IServiceCollection AddQueryables(this IServiceCollection services)
    {
        return services.AddTransient(typeof(IQueryable<>), typeof(QueryableProxy<,>));
    }
}
