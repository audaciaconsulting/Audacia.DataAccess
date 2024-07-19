using System.Linq.Expressions;

namespace Audacia.DataAccess.Specifications.Including;

/// <summary>
/// Encapsulates the data needed to perform an include.
/// </summary>
public class IncludeStep
{
    /// <summary>
    /// Gets the <see cref="System.Linq.Expressions.Expression"/> the contains the ordering rule.
    /// </summary>
    public Expression Expression { get; }

    /// <summary>
    /// Constructor which sets the value of <see cref="Expression"/>.
    /// </summary>
    /// <param name="expression">Instance of <see cref="Expression"/>.</param>
    public IncludeStep(Expression expression)
    {
        Expression = expression;
    }
}
