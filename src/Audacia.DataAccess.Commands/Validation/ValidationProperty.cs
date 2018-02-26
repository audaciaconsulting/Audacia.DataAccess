using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Commands.Validation
{
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
        
        public void AppendError(string message) =>_validationModel.AddModelError(PropertyName, message);

        public string ModelName => _validationModel.ModelName;

        public string PropertyName { get; }

        public string DisplayName { get; }

        public TProperty Value { get; }
    }
}
