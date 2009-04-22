﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;

namespace LinqToCodedom.Visitors
{
    public class CodeExpressionVisitor
    {
        private VisitorContext _ctx;

        public CodeExpressionVisitor(VisitorContext ctx)
        {
            _ctx = ctx;
        }

        public CodeExpression Visit(Expression exp)
        {
            if (exp == null)
                return null;

            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                //return this.VisitUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return this.VisitBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                //return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                //return this.VisitConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return _ctx.VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New:
                //return this.VisitNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                //return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                //return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                //return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                //return this.VisitListInit((ListInitExpression)exp);
                default:
                    throw new NotImplementedException(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
        }

        public static CodeMethodReferenceExpression GetMethodRef(System.Reflection.MethodInfo methodInfo)
        {
            var c = new CodeMethodReferenceExpression()
            {
                MethodName = methodInfo.Name
            };
            if (methodInfo.IsStatic)
                c.MethodName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            return c;
        }

        private CodeExpression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            var c = new CodeMethodInvokeExpression(GetMethodRef(methodCallExpression.Method));
            foreach (var par in methodCallExpression.Arguments)
            {
                c.Parameters.Add(new CodeExpressionVisitor(_ctx).Visit(par));
            }
            c.Method.TargetObject = new CodeExpressionVisitor(_ctx).Visit(methodCallExpression.Object);
            return c;
        }

        private CodeExpression VisitBinary(BinaryExpression binaryExpression)
        {
			CodeBinaryOperatorType operType;
            
			switch (binaryExpression.NodeType)
			{
				case ExpressionType.Add:
					operType = CodeBinaryOperatorType.Add;
					break;
				case ExpressionType.AddChecked:
					operType = CodeBinaryOperatorType.Add;
					break;
				case ExpressionType.And:
					operType = CodeBinaryOperatorType.BooleanAnd;
					break;
				case ExpressionType.AndAlso:
					operType = CodeBinaryOperatorType.BooleanAnd;
					break;
				case ExpressionType.ArrayIndex:
					
				case ExpressionType.ArrayLength:
					
				case ExpressionType.Call:
					
				case ExpressionType.Coalesce:
					
				case ExpressionType.Conditional:
					
				case ExpressionType.Constant:
					
				case ExpressionType.Convert:
					
				case ExpressionType.ConvertChecked:
					
				case ExpressionType.Divide:
					operType = CodeBinaryOperatorType.Divide;
					break;
				case ExpressionType.Equal:
					operType = CodeBinaryOperatorType.IdentityEquality;
					break;
				case ExpressionType.ExclusiveOr:
					operType = CodeBinaryOperatorType.BooleanOr;
					break;
				case ExpressionType.GreaterThan:
					operType = CodeBinaryOperatorType.GreaterThan;
					break;
				case ExpressionType.GreaterThanOrEqual:
					operType = CodeBinaryOperatorType.GreaterThanOrEqual;
					break;
				case ExpressionType.Invoke:
					
				case ExpressionType.Lambda:
					
				case ExpressionType.LeftShift:
					
				case ExpressionType.LessThan:
					operType = CodeBinaryOperatorType.LessThan;
					break;
				case ExpressionType.LessThanOrEqual:
					operType = CodeBinaryOperatorType.LessThanOrEqual;
					break;
				case ExpressionType.ListInit:
				case ExpressionType.MemberAccess:
				case ExpressionType.MemberInit:
				case ExpressionType.Modulo:
					operType = CodeBinaryOperatorType.Modulus;
					break;
				case ExpressionType.Multiply:
					operType = CodeBinaryOperatorType.Multiply;
					break;
				case ExpressionType.MultiplyChecked:
					operType = CodeBinaryOperatorType.Multiply;
					break;
				case ExpressionType.Negate:
				
				case ExpressionType.NegateChecked:
				
				case ExpressionType.New:
				
				case ExpressionType.NewArrayBounds:
				
				case ExpressionType.NewArrayInit:
				case ExpressionType.Not:
					
				case ExpressionType.NotEqual:
					operType = CodeBinaryOperatorType.IdentityInequality;
					break;
				case ExpressionType.Or:
					operType = CodeBinaryOperatorType.BooleanOr;
					break;
				case ExpressionType.OrElse:
					operType = CodeBinaryOperatorType.BooleanOr;
					break;
				case ExpressionType.Parameter:
				case ExpressionType.Power:					
				case ExpressionType.Quote:
					
				case ExpressionType.RightShift:
					
				case ExpressionType.Subtract:
					operType = CodeBinaryOperatorType.Subtract;
					break;
				case ExpressionType.SubtractChecked:
					operType = CodeBinaryOperatorType.Subtract;
					break;
				case ExpressionType.TypeAs:
				case ExpressionType.TypeIs:
				case ExpressionType.UnaryPlus:
				default:
					throw new NotImplementedException();
					break;
			}
			return new CodeBinaryOperatorExpression(
                        Visit(binaryExpression.Left), operType, Visit(binaryExpression.Right));
				
            
        }

        private CodeExpression VisitLambda(LambdaExpression lambdaExpression)
        {
            _ctx.VisitParams(lambdaExpression.Parameters);

            return Visit(lambdaExpression.Body);
        }

        private CodeExpression VisitConstant(ConstantExpression constantExpression)
        {
            return new CodePrimitiveExpression(constantExpression.Value);
        }

        private CodeExpression VisitMemberAccess(MemberExpression memberExpression)
        {
            var c = Visit(memberExpression.Expression);
            if (c is CodeSnippetExpression)
                throw new NotImplementedException();
            else
                return c;
        }
    }
}
