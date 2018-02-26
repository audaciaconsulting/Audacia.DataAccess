using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Audacia.Commands;

namespace Audacia.DataAccess.Commands.Validation
{
    public interface IValidationModel
    {
        IDictionary<string, IEnumerable<string>> ErrorDictionary { get; }
        
        IEnumerable<string> Errors  { get; }

        bool IsValid { get; }
        
        string ModelName { get; }

        void AddModelError(string propertyName, string errorMessage);
        
        bool IsModelNull();
        
        Task<CommandResult> ToCommandResultAsync();
    }

    public interface IValidationModel<TModel> : IValidationModel where TModel : class
    {
        ValidationProperty<TProperty> Property<TProperty>(Expression<Func<TModel, TProperty>> property, string displayName = null);
    }
}
