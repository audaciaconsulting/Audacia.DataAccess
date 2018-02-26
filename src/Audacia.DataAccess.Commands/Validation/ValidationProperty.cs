using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Commands.Validation
{
    /// <summary>
    /// Validates a single property
    /// </summary>
    /// <typeparam name="TProperty">Type of the member being validated</typeparam>
    public class ValidationProperty<TProperty>
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
    }
}
