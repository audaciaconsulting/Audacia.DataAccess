using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.Commands.Validation
{
    public class ValidationBase
    {
        protected string GetNameForProperty(Expression expression)
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
        
        protected IEnumerable<PropertyInfo> GetProperties(Expression expression)
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
