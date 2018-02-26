using System;
using System.Collections.Generic;
using System.Linq;

namespace Audacia.DataAccess.Commands.Validation
{
    public static class ValidationPropertyExtensions
    {
        /// <summary>
        /// Adds a model error explaining the value already exists
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="exists">whether the value already exists</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty> AlreadyExists<TProperty>(this ValidationProperty<TProperty> model, bool exists = true)
        {
            if (exists)
            {
                model.AddError($"The {model.DisplayName} of \"{model.Value}\" already exists on another {model.ModelName}");
            }

            return model;
        }
        
        /// <summary>
        /// Validates that the property has been filled in
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty> IsRequired<TProperty>(this ValidationProperty<TProperty> model)
        {
            var propertyValue = model.Value?.ToString();
            if (string.IsNullOrWhiteSpace(propertyValue))
            {
                model.AddError($"The field {model.DisplayName} must be specified");
            }
            return model;
        }

        /// <summary>
        /// Validates that there is at least one element
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<IEnumerable<TProperty>> IsRequired<TProperty>(this ValidationProperty<IEnumerable<TProperty>> model)
        {
            if (model.Value == null || !model.Value.Any())
            {
                model.AddError($"At least one {model.DisplayName} is required");
            }
            return model;
        }

        /// <summary>
        /// Validates that the property is not longer than the max length
        /// </summary>
        /// <param name="model">validation property</param>
        /// <param name="length">max length of the string</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<string> HasMaxLength(this ValidationProperty<string> model, int length)
        {
            if (!string.IsNullOrEmpty(model.Value) && model.Value.Length > length)
            {
                model.AddError($"The field {model.DisplayName} cannot be longer than {length} characters");
            }
            return model;
        }

        /// <summary>
        /// Validates that the property is not longer than the max length
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="length">max length of the array</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<IEnumerable<TProperty>> HasMaxLength<TProperty>(this ValidationProperty<IEnumerable<TProperty>> model, int length)
        {
            if (model.Value != null && model.Value.Count() > length)
            {
                model.AddError($"The maximum amount of {model.DisplayName}(s) is {length}");
            }
            return model;
        }
        
        
        /// <summary>
        /// Validates that the property is not shorter than the min length
        /// </summary>
        /// <param name="model">validation property</param>
        /// <param name="length">min length of the string</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<string> HasMinLength(this ValidationProperty<string> model, int length)
        {
            if (!string.IsNullOrEmpty(model.Value) && model.Value.Length < length)
            {
                model.AddError($"The field {model.DisplayName} must at least {length} characters long");
            }
            return model;
        }
        
        /// <summary>
        /// Validates that the property is not shorter than the min length
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="length">min length of the array</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<IEnumerable<TProperty>> HasMinLength<TProperty>(this ValidationProperty<IEnumerable<TProperty>> model, int length)
        {
            if (model.Value != null && model.Value.Count() < length)
            {
                model.AddError($"The minimum amount of {model.DisplayName}(s) is {length}");
            }
            return model;
        }
        
        /// <summary>
        /// Validates that the Id is not null or less than zero
        /// </summary>
        /// <param name="model">validation property</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<int?> MustBeAValidId(this ValidationProperty<int?> model)
        {
            if (model.Value.HasValue && model.Value.Value <= 0)
            {
                model.AddError($"Invalid ID provided for {model.DisplayName}");
            }
            return model;
        }

        /// <summary>
        /// Validates that the property is greater than the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty> MustBeGreaterThan<TProperty>(this ValidationProperty<TProperty> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.CompareTo(number) < 1)
            {
                model.AddError($"The value of {model.DisplayName} must greater than {number}");
                
            }
            return model;
        }
        
        /// <summary>
        /// Validates that the property is greater than the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty?> MustBeGreaterThan<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) < 1)
            {
                model.AddError($"The value of {model.DisplayName} must greater than {number}");
                
            }
            return model;
        }
        
        /// <summary>
        /// Validates that the property is greater than or equal to the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty> MustBeGreaterThanOrEqualTo<TProperty>(this ValidationProperty<TProperty> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.CompareTo(number) == -1)
            {
                model.AddError($"The value of {model.DisplayName} must greater than or equal to {number}");
            }
            return model;
        }
        
        /// <summary>
        /// Validates that the property is greater than or equal to the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty?> MustBeGreaterThanOrEqualTo<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) == -1)
            {
                model.AddError($"The value of {model.DisplayName} must greater than or equal to {number}");
            }
            return model;
        }
        
        /// <summary>
        /// Validates that the property is less than the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty> MustBeLessThan<TProperty>(this ValidationProperty<TProperty> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.CompareTo(number) > -1)
            {
                model.AddError($"The value of {model.DisplayName} must less than {number}");
            }
            return model;
        } 
        
        /// <summary>
        /// Validates that the property is less than the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty?> MustBeLessThan<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) > -1)
            {
                model.AddError($"The value of {model.DisplayName} must less than {number}");
            }
            return model;
        } 
        
        /// <summary>
        /// Validates that the property is less than or equal to the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty> MustBeLessThanOrEqualTo<TProperty>(this ValidationProperty<TProperty> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.CompareTo(number) == 1)
            {
                model.AddError($"The value of {model.DisplayName} must less than or equal to {number}");
            }
            return model;
        }
        
        /// <summary>
        /// Validates that the property is less than or equal to the given value
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="model">validation property</param>
        /// <param name="number">value to validate against</param>
        /// <returns>the validation property</returns>
        public static ValidationProperty<TProperty?> MustBeLessThanOrEqualTo<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) == 1)
            {
                model.AddError($"The value of {model.DisplayName} must less than or equal to {number}");
            }
            return model;
        }
    }
}
