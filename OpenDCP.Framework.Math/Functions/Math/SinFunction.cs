using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.Math
{
    public sealed class SinFunction : FunctionBase
    {
        public override string Name => "sin";

        public override string Category => "Math";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Calculates sine of angle in radians." },
                { LanguageCodes.German, "Berechnet den Sinus eines Winkels im Bogenmaß." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The angle in radians for which to calculate the sine value." },
                        { LanguageCodes.German, "Der Winkel im Bogenmaß, für den der Sinus berechnet werden soll." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            double targetValue = parameters[0].AsNumber() ?? 0.0;

            return FormulaValue.Number(System.Math.Sin(targetValue));
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
