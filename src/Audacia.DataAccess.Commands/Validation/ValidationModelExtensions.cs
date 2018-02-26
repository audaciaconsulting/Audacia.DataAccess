using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Commands.Validation
{
    public static class ValidationModelExtensions
    {
        public static IValidationModel<TModel> AlreadyExists<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, bool exists = true, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).AlreadyExists(exists);
            return model;
        }
        
        public static IValidationModel<TModel> IsRequired<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).IsRequired();
            return model;
        }
        
        public static IValidationModel<TModel> HasMaxLength<TModel>(this IValidationModel<TModel> model,
            Expression<Func<TModel, string>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMaxLength(length);
            return model;
        }
        
        public static IValidationModel<TModel> HasMinLength<TModel>(this IValidationModel<TModel> model,
            Expression<Func<TModel, string>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMinLength(length);
            return model;
        }
        
        public static IValidationModel<TModel> MustBeAValidId<TModel>(this IValidationModel<TModel> model,
            Expression<Func<TModel, int?>> property, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).MustBeAValidId();
            return model;
        }

        public static IValidationModel<TModel> MustBeGreaterThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThan(number);
            return model;
        }

        public static IValidationModel<TModel> MustBeGreaterThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThan(number);
            return model;
        }
        
        public static IValidationModel<TModel> MustBeGreaterThanOrEqualTo<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThanOrEqualTo(number);
            return model;
        }

        public static IValidationModel<TModel> MustBeGreaterThanOrEqualTo<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThanOrEqualTo(number);
            return model;
        }
        
        public static IValidationModel<TModel> MustBeLessThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThan(number);
            return model;
        }

        public static IValidationModel<TModel> MustBeLessThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThan(number);
            return model;
        }

        public static IValidationModel<TModel> MustBeLessThanOrEqualTo<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThanOrEqualTo(number);
            return model;
        }

        public static IValidationModel<TModel> MustBeLessThanOrEqualTo<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThanOrEqualTo(number);
            return model;
        }
    }
}
