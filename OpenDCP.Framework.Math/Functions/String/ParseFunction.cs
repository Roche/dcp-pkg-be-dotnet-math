using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OpenDCP.Framework.Math.Functions.String
{
    public sealed class ParseFunction : FunctionBase
    {
        public override string Name => "parse";

        public override string Category => "String";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(string), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Parses a string value into a number. Returns NaN if the string cannot be parsed." },
                { LanguageCodes.German, "Wandelt einen Zeichenfolgenwert in eine Zahl um. Gibt NaN zurück, wenn die Zeichenfolge nicht umgewandelt werden kann." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The string value to parse as a number." },
                        { LanguageCodes.German, "Der Zeichenfolgenwert, der als Zahl umgewandelt werden soll." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            string value = parameters[0].AsString() ?? string.Empty;

            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            {
                return FormulaValue.Number(result);
            }

            return FormulaValue.Number(double.NaN);
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
