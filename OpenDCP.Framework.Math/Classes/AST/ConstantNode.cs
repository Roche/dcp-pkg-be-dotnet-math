using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Classes.AST
{
    public sealed class ConstantNode : IExpressionNode
    {
        private readonly FormulaValue _value;

        public ConstantNode(FormulaValue value)
        {
            _value = value;
        }

        public FormulaValue Evaluate(EvaluationContext ctx)
        {
            return _value;
        }

        public string ExtractUoM(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            double? num = _value.AsNumber();

            if (num.HasValue)
            {
                return num.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            if (_value.AsBool().HasValue)
            {
                return _value.AsBool().Value.ToString().ToLowerInvariant(); // "true" or "false"
            }

            return string.Empty;
        }

        public string GetDisplayName(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            double? num = _value.AsNumber();
            if (num.HasValue)
            {
                return num.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            if (_value.AsBool().HasValue)
            {
                return _value.AsBool().Value.ToString().ToLowerInvariant();
            }

            return string.Empty;
        }
    }
}
