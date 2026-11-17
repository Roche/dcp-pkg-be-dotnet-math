using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.String
{
    public sealed class LeftFunction : FunctionBase
    {
        public override string Name => "left";

        public override string Category => "String";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(string), optional: false),
            new ParameterDefinition(name: "length", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Extracts a substring of a given length from the beginning of a string." },
                { LanguageCodes.German, "Extrahiert eine Teilzeichenfolge einer bestimmten Länge vom Anfang einer Zeichenfolge." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The string value to extract from." },
                        { LanguageCodes.German, "Der Zeichenfolgenwert, aus dem extrahiert werden soll." }
                    }
                },
                {
                    "length", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The number of characters to extract from the start." },
                        { LanguageCodes.German, "Die Anzahl der Zeichen, die vom Anfang extrahiert werden sollen." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            string value = parameters[0].AsString() ?? string.Empty;
            int length = (int)(parameters[1].AsNumber() ?? 0.0);

            if (length <= 0)
            {
                return FormulaValue.String(string.Empty);
            }

            int safeLength = System.Math.Min(length, value.Length);
            return FormulaValue.String(value.Substring(0, safeLength));
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
