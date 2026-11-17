using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Classes.AST
{
    public sealed class BinaryNode : IExpressionNode
    {
        public string Operation { get; }
        public IExpressionNode Left { get; }
        public IExpressionNode Right { get; }
        public BinaryNode(string operation, IExpressionNode left, IExpressionNode right)
        {
            Operation = operation;
            Left = left;
            Right = right;
        }
        public FormulaValue Evaluate(EvaluationContext ctx)
        {
            var left = Left.Evaluate(ctx);
            var right = Right.Evaluate(ctx);

            switch (Operation)
            {
                case "+":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Number(double.NaN);
                    return FormulaValue.Number(left.AsNumber().Value + right.AsNumber().Value);
                case "-":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Number(double.NaN);
                    return FormulaValue.Number(left.AsNumber().Value - right.AsNumber().Value);
                case "*":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Number(double.NaN);
                    return FormulaValue.Number(left.AsNumber().Value * right.AsNumber().Value);
                case "/":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Number(double.NaN);
                    double denominator = right.AsNumber().Value;
                    if (denominator == 0) return FormulaValue.Number(double.NaN);
                    return FormulaValue.Number(left.AsNumber().Value / denominator);
                case "=":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Boolean(false);
                    return FormulaValue.Boolean(left.AsNumber().Value == right.AsNumber().Value);
                case "!=":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Boolean(true);
                    return FormulaValue.Boolean(left.AsNumber().Value != right.AsNumber().Value);
                case ">":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Boolean(false);
                    return FormulaValue.Boolean(left.AsNumber().Value > right.AsNumber().Value);
                case "<":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Boolean(false);
                    return FormulaValue.Boolean(left.AsNumber().Value < right.AsNumber().Value);
                case ">=":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Boolean(false);
                    return FormulaValue.Boolean(left.AsNumber().Value >= right.AsNumber().Value);
                case "<=":
                    if (left.AsNumber() == null || right.AsNumber() == null) return FormulaValue.Boolean(false);
                    return FormulaValue.Boolean(left.AsNumber().Value <= right.AsNumber().Value);
                case "AND":
                case "&":
                    return FormulaValue.Boolean(left.GetBoolValue() && right.GetBoolValue());
                case "OR":
                case "|":
                    return FormulaValue.Boolean(left.GetBoolValue() || right.GetBoolValue());
                default:
                    throw new NotSupportedException($"The operator '{Operation}' is not implemented inside the AST execution engine.");
            }
        }

        public string ExtractUoM(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            string leftUoM = Left.ExtractUoM(ctx, sensorLookup);
            string rightUoM = Right.ExtractUoM(ctx, sensorLookup);

            bool isLeftConstant = Left is ConstantNode;
            bool isRightConstant = Right is ConstantNode;

            switch (Operation)
            {
                case "+":
                case "-":
                    if (isLeftConstant) return rightUoM;
                    if (isRightConstant) return leftUoM;

                    if (!string.IsNullOrEmpty(leftUoM) && leftUoM.Equals(rightUoM, StringComparison.OrdinalIgnoreCase))
                    {
                        return leftUoM;
                    }

                    if (string.IsNullOrEmpty(leftUoM)) return rightUoM;
                    if (string.IsNullOrEmpty(rightUoM)) return leftUoM;

                    return $"{leftUoM} {Operation} {rightUoM}";

                case "*":
                    if (string.IsNullOrEmpty(leftUoM)) return rightUoM;
                    if (string.IsNullOrEmpty(rightUoM)) return leftUoM;

                    return $"{leftUoM} {Operation} {rightUoM}";

                case "/":
                    if (!string.IsNullOrEmpty(leftUoM) && leftUoM.Equals(rightUoM, StringComparison.OrdinalIgnoreCase))
                    {
                        return "dim";
                    }

                    if (isLeftConstant)
                    {
                        return string.IsNullOrEmpty(rightUoM) ? string.Empty : $"1 / {rightUoM}";
                    }

                    if (isRightConstant)
                    {
                        return leftUoM;
                    }

                    if (string.IsNullOrEmpty(leftUoM)) return $"1 / {rightUoM}";
                    if (string.IsNullOrEmpty(rightUoM)) return leftUoM;

                    return $"{leftUoM} {Operation} {rightUoM}";

                default:
                    return string.Empty;
            }
        }

        public string GetDisplayName(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            string leftDisplay = Left.GetDisplayName(ctx, sensorLookup);
            string rightDisplay = Right.GetDisplayName(ctx, sensorLookup);

            bool wrapLeft = NeedsParentheses(Left, Operation);
            bool wrapRight = NeedsParentheses(Right, Operation);

            string leftStr = wrapLeft ? $"({leftDisplay})" : leftDisplay;
            string rightStr = wrapRight ? $"({rightDisplay})" : rightDisplay;

            return $"{leftStr} {Operation} {rightStr}";
        }

        private bool NeedsParentheses(IExpressionNode childNode, string parentOperation)
        {
            if (childNode is BinaryNode childBinary)
            {
                if ((parentOperation == "*" || parentOperation == "/") &&
                    (childBinary.Operation == "+" || childBinary.Operation == "-"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
