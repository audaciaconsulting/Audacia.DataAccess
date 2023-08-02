using System;
using System.Linq;
using System.Linq.Expressions;
using Audacia.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audacia.DataAccess.EntityFrameworkCore.Extensions;

public static class ModelBuilderExtensions
{
    public static void HasGlobalQueryFilter<TEntityInterfaceOrBase>(this ModelBuilder modelBuilder,
        Expression<Func<TEntityInterfaceOrBase, bool>> filter)
        where TEntityInterfaceOrBase : class
    {
        var clrTypes = from entityType in modelBuilder.Model.GetEntityTypes()
                       where typeof(TEntityInterfaceOrBase).IsAssignableFrom(entityType.ClrType)
                       select entityType.ClrType;

        foreach (var clrType in clrTypes)
        {
            var typeSpecificFilter = filter.ConvertGenericTypeArgument(clrType);

            modelBuilder.Entity(clrType).HasQueryFilter(typeSpecificFilter);
        }
    }

    public static void HasGlobalPropertyConfiguration<TEntityInterfaceOrBase>(this ModelBuilder modelBuilder,
        Expression<Func<TEntityInterfaceOrBase, object>> propertyExpression,
        Action<PropertyBuilder> propertyBuilderAction)
        where TEntityInterfaceOrBase : class
    {
        var propertyInfo = ExpressionExtensions.GetPropertyInfo(propertyExpression);

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