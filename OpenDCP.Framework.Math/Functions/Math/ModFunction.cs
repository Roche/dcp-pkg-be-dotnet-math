using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.Math
{
    public sealed class ModFunction : FunctionBase
    {
        public override string Name => "mod";

        public override string Category => "Math";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "dividend", type: typeof(double), optional: false),
            new ParameterDefinition(name: "divisor", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Calculates the remainder of dividing one number by another." },
                { LanguageCodes.German, "Berechnet den Rest der Division einer Zahl durch eine andere." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "dividend", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The numeric value to be divided." },
                        { LanguageCodes.German, "Der numerische Wert, der geteilt werden soll." }
                    }
                },
                {
                    "divisor", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The numeric value to divide the dividend by." },
                        { LanguageCodes.German, "Der numerische Wert, durch den geteilt wird." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            double dividend = parameters[0].AsNumber() ?? 0.0;
            double divisor = parameters[1].AsNumber() ?? 0.0;

            return FormulaValue.Number(dividend % divisor);
        }
    }
}
