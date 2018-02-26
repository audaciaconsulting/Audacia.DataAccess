using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Commands.Validation
{
    public static class ValidationModelExtensions
    {
        /// <summary>
        /// Adds a model error explaining the value already exists
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="exists">whether the value already exists</param> 
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> AlreadyExists<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, bool exists = true, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).AlreadyExists(exists);
            return model;
        }
        
        /// <summary>
        /// Validates that the property has been filled in
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> IsRequired<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).IsRequired();
            return model;
        }
        
        
        /// <summary>
        /// Validates that the property is not longer than the max length
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="length">max length of the string</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> HasMaxLength<TModel>(this IValidationModel<TModel> model,
            Expression<Func<TModel, string>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMaxLength(length);
            return model;
        }

        /// <summary>
        /// Validates that the property is not longer than the max length
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="length">max length of the string</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> HasMaxLength<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, IEnumerable<TProperty>>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMaxLength(length);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is not shorter than the min length
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="length">min length of the string</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> HasMinLength<TModel>(this IValidationModel<TModel> model,
            Expression<Func<TModel, string>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMinLength(length);
            return model;
        }

        
        /// <summary>
        /// Validates that the property is not shorter than the min length
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="length">min length of the string</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> HasMinLength<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, IEnumerable<TProperty>>> property, int length, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).HasMinLength(length);
            return model;
        }
        
        /// <summary>
        /// Validates that the Id is not null or less than zero
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeAValidId<TModel>(this IValidationModel<TModel> model,
            Expression<Func<TModel, int?>> property, string displayName = null)
            where TModel : class
        {
            model.Property(property, displayName).MustBeAValidId();
            return model;
        }

        /// <summary>
        /// Validates that the property is greater than the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeGreaterThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThan(number);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is greater than the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeGreaterThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThan(number);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is greater than or equal to the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeGreaterThanOrEqualTo<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThanOrEqualTo(number);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is greater than or equal to the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeGreaterThanOrEqualTo<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeGreaterThanOrEqualTo(number);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is less than the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeLessThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThan(number);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is less than the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeLessThan<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty?>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThan(number);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is less than or equal to the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
        public static IValidationModel<TModel> MustBeLessThanOrEqualTo<TModel, TProperty>(this IValidationModel<TModel> model,
            Expression<Func<TModel, TProperty>> property, TProperty number, string displayName = null)
            where TModel : class
            where TProperty : struct, IComparable
        {
            model.Property(property, displayName).MustBeLessThanOrEqualTo(number);
            return model;
        }
        
        /// <summary>
        /// Validates that the property is less than or equal to the given value
        /// </summary>
        /// <typeparam name="TModel">Type of the model being validated</typeparam>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="number">value to validate against</param>
        /// <param name="displayName">User friendly property name</param>
        /// <returns>the validation model</returns>
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
