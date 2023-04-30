using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public class ExpressionRewriter
    {
        /// <summary>
        /// Rename the param name of the expression.
        /// </summary>
        /// <typeparam name="T">Expression func.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <param name="newParamName">The new param name.</param>
        /// <returns></returns>
        public static Expression<T> Rewrite<T>(Expression<T> exp, string newParamName)
        {
            var param = Expression.Parameter(exp.Parameters[0].Type, newParamName);
            var newExpression = new ExpressionRewriterVisitor(param).Visit(exp);

            return (Expression<T>)newExpression;
        }

        private class ExpressionRewriterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _parameterExpression;

            public ExpressionRewriterVisitor(ParameterExpression parameterExpression)
            {
                _parameterExpression = parameterExpression;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameterExpression;
            }
        }
    }
}
