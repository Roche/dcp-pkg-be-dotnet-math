using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.Math
{
    public sealed class AbsFunction : FunctionBase
    {
        public override string Name => "abs";

        public override string Category => "Math";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Calculates the absolute value of a given number." },
                { LanguageCodes.German, "Berechnet den Absolutwert einer gegebenen Zahl." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The numeric value to calculate the absolute value for." },
                        { LanguageCodes.German, "Der numerische Wert, für den der Absolutwert berechnet werden soll." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            double targetValue = parameters[0].AsNumber() ?? 0.0;

            return FormulaValue.Number(System.Math.Abs(targetValue));
        }
    }
}
