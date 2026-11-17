using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.Math
{
    public sealed class PowFunction : FunctionBase
    {
        public override string Name => "pow";

        public override string Category => "Math";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "base", type: typeof(double), optional: false),
            new ParameterDefinition(name: "exponent", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Calculates a number raised to the power of another number." },
                { LanguageCodes.German, "Berechnet eine Zahl, die mit einer anderen Zahl potenziert wird." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "base", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The base numeric value." },
                        { LanguageCodes.German, "Der numerische Basiswert." }
                    }
                },
                {
                    "exponent", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The power to raise the base value to." },
                        { LanguageCodes.German, "Der Exponent, mit dem der Basiswert potenziert wird." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            double baseValue = parameters[0].AsNumber() ?? 0.0;
            double exponentValue = parameters[1].AsNumber() ?? 0.0;

            return FormulaValue.Number(System.Math.Pow(baseValue, exponentValue));
        }
    }
}
