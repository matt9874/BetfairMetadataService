using System.Collections.Generic;
using System.Linq.Expressions;

namespace BetfairMetadataService.Domain.Common.Helpers
{
    internal class SubstExpressionVisitor : ExpressionVisitor
    {
        internal Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (subst.TryGetValue(node, out Expression newValue))
            {
                return newValue;
            }
            return node;
        }
    }
}
