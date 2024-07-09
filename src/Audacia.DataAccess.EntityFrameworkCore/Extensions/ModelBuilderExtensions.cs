using System;
using System.Linq;
using System.Linq.Expressions;
using Audacia.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audacia.DataAccess.EntityFrameworkCore.Extensions;

/// <summary>
/// Extenstion class for ModelBuilder.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Chekcs if the <see cref="ModelBuilder"/> instance has any query filters.
    /// </summary>
    /// <typeparam name="TEntityInterfaceOrBase">Entity type.</typeparam>
    /// <param name="modelBuilder">Instance of <see cref="ModelBuilder"/>.</param>
    /// <param name="filter">Filter value.</param>
    public static void HasGlobalQueryFilter<TEntityInterfaceOrBase>(
        this ModelBuilder modelBuilder,
        Expression<Func<TEntityInterfaceOrBase, bool>> filter)
        where TEntityInterfaceOrBase : class
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        var clrTypes = from entityType in modelBuilder.Model.GetEntityTypes()
                       where typeof(TEntityInterfaceOrBase).IsAssignableFrom(entityType.ClrType)
                       select entityType.ClrType;

        foreach (var clrType in clrTypes)
        {
            var typeSpecificFilter = filter.ConvertGenericTypeArgument(clrType);

            modelBuilder.Entity(clrType).HasQueryFilter(typeSpecificFilter);
        }
    }

    /// <summary>
    /// Chekcs if the <see cref="ModelBuilder"/> instance has any property expressions.
    /// </summary>
    /// <typeparam name="TEntityInterfaceOrBase">Entity type.</typeparam>
    /// <param name="modelBuilder">Instance of <see cref="ModelBuilder"/>.</param>
    /// <param name="propertyExpression">Expression instance.</param>
    /// <param name="propertyBuilderAction"><see cref="Action{PropertyBuilder}"/> instance. </param>
    public static void HasGlobalPropertyConfiguration<TEntityInterfaceOrBase>(
        this ModelBuilder modelBuilder,
        Expression<Func<TEntityInterfaceOrBase, object>> propertyExpression,
        Action<PropertyBuilder> propertyBuilderAction)
        where TEntityInterfaceOrBase : class
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        ArgumentNullException.ThrowIfNull(propertyBuilderAction);

        var propertyInfo = ExpressionExtensions.GetPropertyInfo(propertyExpression);

        ArgumentNullException.ThrowIfNull(propertyInfo);

        var clrTypes = from entityType in modelBuilder.Model.GetEntityTypes()
                       where typeof(TEntityInterfaceOrBase).IsAssignableFrom(entityType.ClrType)
                       select entityType.ClrType;

        foreach (var clrType in clrTypes)
        {
            var propertyBuilder = modelBuilder.Entity(clrType).Property(propertyInfo.Name);

            propertyBuilderAction(propertyBuilder);
        }
    }
}