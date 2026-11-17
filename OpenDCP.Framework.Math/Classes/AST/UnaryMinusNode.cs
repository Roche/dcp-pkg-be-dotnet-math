using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Classes.AST
{
    public sealed class UnaryMinusNode : IExpressionNode
    {
        public IExpressionNode Operand { get; }

        public UnaryMinusNode(IExpressionNode operand)
        {
            Operand = operand;
        }

        public FormulaValue Evaluate(EvaluationContext ctx)
        {
            var value = Operand.Evaluate(ctx);
            double? number = value.AsNumber();
            if (!number.HasValue)
            {
                return FormulaValue.Number(double.NaN);
            }
            return FormulaValue.Number(-number.Value);
        }

        public string ExtractUoM(EvaluationContext context, Dictionary<string, string> sensorLookup)
        {
            return Operand.ExtractUoM(context, sensorLookup);
        }

        public string GetDisplayName(EvaluationContext context, Dictionary<string, string> sensorLookup)
        {
            return $"-{Operand.GetDisplayName(context, sensorLookup)}";
        }
    }
}
