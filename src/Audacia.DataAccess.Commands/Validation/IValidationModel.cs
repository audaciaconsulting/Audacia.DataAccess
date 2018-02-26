using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Audacia.Commands;

namespace Audacia.DataAccess.Commands.Validation
{
    public interface IValidationModel
    {
        /// <summary>
        /// Key value pairs of Property Name and Validation Errors,
        /// Any errors against the Model uses a blank string as the Property Name
        /// </summary>
        IDictionary<string, IEnumerable<string>> ModelErrors { get; }
        
        /// <summary>
        /// All validation errors regardless of property
        /// </summary>
        IEnumerable<string> AllErrors  { get; }

        /// <summary>
        /// Returns true if there are no errors
        /// </summary>
        bool IsValid { get; }
        
        /// <summary>
        /// User friendly model name
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// Adds a validation error for a property
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="errorMessage">Description of the validation error</param>
        void AddModelError(string propertyName, string errorMessage);
        
        /// <summary>
        /// Checks if the model is null,
        /// Adds a validation error when null
        /// </summary>
        /// <returns>true if the model is null</returns>
        bool IsModelNull();
        
        /// <summary>
        /// Appends any validation errors to the command result
        /// </summary>
        /// <returns>A command result</returns>
        Task<CommandResult> ToCommandResultAsync();
    }

    public interface IValidationModel<TModel> : IValidationModel where TModel : class
    {
        /// <summary>
        /// Creates a validation property
        /// </summary>
        /// <typeparam name="TProperty">Type of the member being validated</typeparam>
        /// <param name="property">Expresssion referencing the member being validated</param>
        /// <param name="displayName">User friendly member name</param>
        /// <returns>A validation property</returns>
        ValidationProperty<TProperty> Property<TProperty>(Expression<Func<TModel, TProperty>> property, string displayName = null);
    }
}
