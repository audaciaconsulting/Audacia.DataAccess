using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Commands.Validation
{
    public static class ValidationModelExtensions
    {
        public static ValidationModel<TModel> AlreadyExists<TModel, TProperty>(this ValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, bool exists = true, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).AlreadyExists(exists);
            return model;
        }
        
        public static ValidationModel<TModel> IsRequired<TModel, TProperty>(this ValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).IsRequired();
            return model;
        }
        
        public static ValidationModel<TModel> HasMaxLength<TModel>(this ValidationModel<TModel> model,
            Expression<Func<TModel, string>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMaxLength(length);
            return model;
        }
        
        public static ValidationModel<TModel> HasMinLength<TModel>(this ValidationModel<TModel> model,
            Expression<Func<TModel, string>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMinLength(length);
            return model;
        }
        
        public static ValidationModel<TModel> MustBeAValidId<TModel>(this ValidationModel<TModel> model,
            Expression<Func<TModel, int?>> property, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).MustBeAValidId();
            return model;
        }

        public static ValidationModel<TModel> MustBeGreaterThan<TModel, TProperty>(this ValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThan(number);
            return model;
        }
        
        public static ValidationModel<TModel> MustBeGreaterThanOrEqualTo<TModel, TProperty>(this ValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThanOrEqualTo(number);
            return model;
        }
        
        public static ValidationModel<TModel> MustBeLessThan<TModel, TProperty>(this ValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThan(number);
            return model;
        }

        public static ValidationModel<TModel> MustBeLessThanOrEqualTo<TModel, TProperty>(this ValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThanOrEqualTo(number);
            return model;
        }
    }
}
