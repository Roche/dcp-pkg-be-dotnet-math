using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.Math
{
    public sealed class FloorFunction : FunctionBase
    {
        public override string Name => "floor";

        public override string Category => "Math";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Rounds a number down to the nearest integer." },
                { LanguageCodes.German, "Rundet eine Zahl auf die nächste ganze Zahl ab." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The numeric value to round down to the nearest integer." },
                        { LanguageCodes.German, "Der numerische Wert, der abgerundet werden soll." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            double targetValue = parameters[0].AsNumber() ?? 0.0;

            return FormulaValue.Number(System.Math.Floor(targetValue));
        }
    }
}
