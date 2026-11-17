using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.Math
{
    public sealed class ExpFunction : FunctionBase
    {
        public override string Name => "exp";

        public override string Category => "Math";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Calculates e raised to the power of a given number." },
                { LanguageCodes.German, "Berechnet e potenziert mit einer gegebenen Zahl." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The exponent to which e is raised." },
                        { LanguageCodes.German, "Der Exponent, mit dem e potenziert wird." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            double targetValue = parameters[0].AsNumber() ?? 0.0;

            return FormulaValue.Number(System.Math.Exp(targetValue));
        }

        public override string ExtractUoM(
            EvaluationContext context,
            Dictionary<string, string> sensorLookup,
            IReadOnlyList<IExpressionNode> arguments)
        {
            return "dim";
        }
    }
}
