using System;
using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Ordering
{
    /// <summary>
    /// Encapsulates the data needed to perform an order by clause.
    /// </summary>
    public class OrderStep
    {
        /// <summary>
        /// Gets a value indicating whether the ordering should be ascending (true) or descending (false).
        /// </summary>
        public bool Ascending { get; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> of the property on which to order.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the <see cref="System.Linq.Expressions.Expression"/> the contains the ordering rule.
        /// </summary>
        public Expression Expression { get; }

        public OrderStep(bool asc, Type type, Expression expression)
        {
            Ascending = asc;
            Type = type;
            Expression = expression;
        }
    }
}