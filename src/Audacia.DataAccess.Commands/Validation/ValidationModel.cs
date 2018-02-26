using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Audacia.Commands;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Commands.Validation
{
    public class ValidationModel<TModel> : IValidationModel where TModel : class
    {
        private readonly string _modelName;
        private readonly TModel _model;
        private readonly IDictionary<string, HashSet<string>> _errors;

        public ValidationModel(TModel model, string modelName)
        {
            _modelName = string.IsNullOrWhiteSpace(modelName) 
                ? typeof(TModel).Name.SplitCamelCase()
                : modelName;
            _model = model;
            _errors = new Dictionary<string, HashSet<string>>();
        }

        public string ModelName => _modelName;

        public bool IsValid => !_errors.Any();

        public IDictionary<string, IEnumerable<string>> ErrorDictionary => _errors as IDictionary<string, IEnumerable<string>>;

        public IEnumerable<string> Errors => _errors.SelectMany(kvp => kvp.Value).ToArray();
        
        public void AddModelError(string propertyName, string errorMessage)
        {
            if (propertyName == null)
            {
                throw new ArgumentException("Property Name cannot be null", nameof(propertyName));
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                throw new ArgumentException("Model Error Message cannot be empty", nameof(errorMessage));
            }

            if (_errors.ContainsKey(propertyName))
            {
                _errors[propertyName].Add(errorMessage);
            }
            else
            {
                _errors.Add(propertyName, new HashSet<string> { errorMessage });
            }
        }
        
        public bool IsModelNull()
        {
            if (_model == default(TModel))
            {
                AddModelError(string.Empty, "No data received");

                return true;
            }

            return false;
        }
        
        public ValidationProperty<TProperty> Property<TProperty>(Expression<Func<TModel, TProperty>> property, string displayName = null)
        { 
            var propertyName = GetNameForProperty(property);
            var value = property.Compile()(_model); 

            return new ValidationProperty<TProperty>(this, propertyName, displayName, value);
        }

        public Task<CommandResult> ToCommandResultAsync()
        {
            return Task.FromResult(new CommandResult(IsValid, Errors));
        }
        
        private static string GetNameForProperty(Expression expression)
        {
            if (expression is LambdaExpression)
            {
                return ExpressionExtensions.GetPropertyInfo(expression).Name;
            }

            if (expression is UnaryExpression unaryExpression)
            {
                return string.Join(".", GetProperties(unaryExpression.Operand).Select(p => p.Name));
            }

            return string.Empty;
        }
        
        private static IEnumerable<PropertyInfo> GetProperties(Expression expression)
        {
            if (!(expression is MemberExpression memberExpression))
            {
                yield break;
            }

            var property = memberExpression.Member as PropertyInfo;
            foreach (var propertyInfo in GetProperties(memberExpression.Expression))
            {
                yield return propertyInfo;
            }
            yield return property;
        }
    }
}
