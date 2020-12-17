using System;
using System.Linq;
using System.Linq.Expressions;

namespace Reflection.Differentiation
{
    public class Algebra
    {
        public static Expression Differentiate(Expression f)
        {
            var type = f.GetType();

            switch (true)
            {
                case true when typeof(BinaryExpression).IsAssignableFrom(type):
                {
                    var binaryExpression = (BinaryExpression) f;
                    var left = binaryExpression.Left;
                    var right = binaryExpression.Right;
                    var nodeType = binaryExpression.NodeType;

                    if (nodeType == ExpressionType.Multiply)
                    {
                        return Expression.Add(
                            Expression.Multiply(
                                Differentiate(left),
                                right
                            ),
                            Expression.Multiply(
                                left,
                                Differentiate(right)
                            )
                        );
                    }

                    if (nodeType == ExpressionType.Add)
                    {
                        return Expression.Add(
                            Differentiate(left),
                            Differentiate(right)
                        );
                    }

                    throw new ArgumentException();
                }
                case true when typeof(MethodCallExpression).IsAssignableFrom(type):
                {
                    var methodCallExpression = (MethodCallExpression) f;
                    var argument = methodCallExpression.Arguments.First();

                    if (methodCallExpression.Method == typeof(Math).GetMethod(nameof(Math.Sin)))
                    {
                        return Expression.Multiply(
                            Expression.Call(
                                typeof(Math).GetMethod(nameof(Math.Cos)),
                                argument
                            ),
                            Differentiate(argument)
                        );
                    }

                    if (methodCallExpression.Method == typeof(Math).GetMethod(nameof(Math.Cos)))
                    {
                        return Expression.Multiply(
                            Expression.Multiply(
                                Expression.Constant(-1.0, typeof(double)),
                                Expression.Call(
                                    typeof(Math).GetMethod(nameof(Math.Sin)),
                                    argument
                                )
                            ),
                            Differentiate(argument)
                        );
                    }

                    throw new ArgumentException(methodCallExpression.Method.Name);
                }
                case true when typeof(ParameterExpression).IsAssignableFrom(type):
                    return Expression.Constant(1.0, typeof(double));
                case true when typeof(ConstantExpression).IsAssignableFrom(type):
                    return Expression.Constant(0.0, typeof(double));
                default:
                    throw new ArgumentException(f.ToString());
            }
        }

        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> f)
        {
            var differentiatedBody = Differentiate(f.Body);

            return Expression.Lambda<Func<double, double>>(
                differentiatedBody, f.Parameters);
        }
    }
}