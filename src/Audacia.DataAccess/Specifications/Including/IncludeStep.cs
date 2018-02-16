using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including
{
    /// <summary>
    /// Encapsulates the data needed to perform an include.
    /// </summary>
    public class IncludeStep
    {
        /// <summary>
        /// Gets the <see cref="System.Type"/> of the property on which to order.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the <see cref="System.Linq.Expressions.Expression"/> the contains the ordering rule.
        /// </summary>
        public Expression Expression { get; }

        public IncludeStep(Type type, Expression expression)
        {
            Type = type;
            Expression = expression;
        }
    }
}
