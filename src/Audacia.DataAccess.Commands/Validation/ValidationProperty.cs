using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Commands.Validation
{
    /// <summary>
    /// Validates a single property
    /// </summary>
    /// <typeparam name="TProperty">Type of the member being validated</typeparam>
    public class ValidationProperty<TProperty> : ValidationBase
    {
        private readonly IValidationModel _validationModel;

        public ValidationProperty(IValidationModel validationModel,
            string propertyName, string displayName, TProperty value)
        {
            _validationModel = validationModel;
            PropertyName = propertyName;
            DisplayName = displayName ?? propertyName.SplitCamelCase();
            Value = value;
        }
        
        /// <summary>
        /// Adds a validation error to the property
        /// </summary>
        /// <param name="message">Description of the validation error</param>
        public void AddError(string message) =>_validationModel.AddModelError(PropertyName, message);

        /// <summary>
        /// User friendly model name
        /// </summary>
        public string ModelName => _validationModel.ModelName;

        /// <summary>
        /// Name of the property
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// User friendly property name
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Property value
        /// </summary>
        public TProperty Value { get; }
        
        /// <summary>
        /// Creates a child validation property on a property object
        /// </summary>
        /// <typeparam name="TChild">Type of the member being validated</typeparam>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="displayName">User friendly member name</param>
        /// <returns>A validation property</returns>
        public ValidationProperty<TChild> Property<TChild>(Expression<Func<TProperty, TChild>> property, string displayName = null)
        { 
            var propertyName = GetNameForProperty(property);
            var value = property.Compile()(Value);
            var childPropertyName = $"{PropertyName}.{propertyName}";
            return new ValidationProperty<TChild>(_validationModel, childPropertyName, displayName, value);
        }
    }
}
