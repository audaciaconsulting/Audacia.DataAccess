using System;
using System.Collections.Generic;
using System.Linq;

namespace Audacia.DataAccess.Commands.Validation
{
    public static class ValidationPropertyExtensions
    {
        public static ValidationProperty<TProperty> AlreadyExists<TProperty>(this ValidationProperty<TProperty> model, bool exists = true)
        {
            if (exists)
            {
                model.AppendError($"The {model.DisplayName} of \"{model.Value}\" already exists on another {model.ModelName}");
            }

            return model;
        }
        
        public static ValidationProperty<TProperty> IsRequired<TProperty>(this ValidationProperty<TProperty> model)
        {
            var propertyValue = model.Value?.ToString();
            if (string.IsNullOrWhiteSpace(propertyValue))
            {
                model.AppendError($"The field {model.DisplayName} must be specified");
            }
            return model;
        }

        public static ValidationProperty<IEnumerable<TProperty>> IsRequired<TProperty>(this ValidationProperty<IEnumerable<TProperty>> model)
        {
            if (model.Value == null || !model.Value.Any())
            {
                model.AppendError($"At least one {model.DisplayName} is required");
            }
            return model;
        }

        public static ValidationProperty<string> HasMaxLength(this ValidationProperty<string> model, int length)
        {
            if (!string.IsNullOrEmpty(model.Value) && model.Value.Length > length)
            {
                model.AppendError($"The field {model.DisplayName} cannot be longer than {length} characters");
            }
            return model;
        }

        public static ValidationProperty<IEnumerable<TProperty>> HasMaxLength<TProperty>(this ValidationProperty<IEnumerable<TProperty>> model, int length)
        {
            if (model.Value != null && model.Value.Count() > length)
            {
                model.AppendError($"The maximum amount of {model.DisplayName}(s) is {length}");
            }
            return model;
        }
        
        public static ValidationProperty<string> HasMinLength(this ValidationProperty<string> model, int length)
        {
            if (!string.IsNullOrEmpty(model.Value) && model.Value.Length < length)
            {
                model.AppendError($"The field {model.DisplayName} must at least {length} characters long");
            }
            return model;
        }

        public static ValidationProperty<IEnumerable<TProperty>> HasMinLength<TProperty>(this ValidationProperty<IEnumerable<TProperty>> model, int length)
        {
            if (model.Value != null && model.Value.Count() < length)
            {
                model.AppendError($"The minimum amount of {model.DisplayName}(s) is {length}");
            }
            return model;
        }
        
        public static ValidationProperty<int?> MustBeAValidId(this ValidationProperty<int?> model)
        {
            if (model.Value.HasValue && model.Value.Value <= 0)
            {
                model.AppendError($"Invalid ID provided for {model.DisplayName}");
            }
            return model;
        }

        public static ValidationProperty<TProperty?> MustBeGreaterThan<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) < 1)
            {
                model.AppendError($"The value of {model.DisplayName} must greater than {number}");
                
            }
            return model;
        }
        
        public static ValidationProperty<TProperty?> MustBeGreaterThanOrEqualTo<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) == -1)
            {
                model.AppendError($"The value of {model.DisplayName} must greater than or equal to {number}");
            }
            return model;
        }
        
        public static ValidationProperty<TProperty?> MustBeLessThan<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) > -1)
            {
                model.AppendError($"The value of {model.DisplayName} must less than {number}");
            }
            return model;
        } 
        
        public static ValidationProperty<TProperty?> MustBeLessThanOrEqualTo<TProperty>(this ValidationProperty<TProperty?> model, TProperty number)
            where TProperty : struct, IComparable
        {
            if (model.Value.HasValue && model.Value.Value.CompareTo(number) == 1)
            {
                model.AppendError($"The value of {model.DisplayName} must less than or equal to {number}");
            }
            return model;
        }
    }
}
