using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.String
{
    public sealed class MidFunction : FunctionBase
    {
        public override string Name => "mid";
        public override string Category => "String";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(string), optional: false),
            new ParameterDefinition(name: "start", type: typeof(double), optional: false),
            new ParameterDefinition(name: "length", type: typeof(double), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Extracts a substring starting at a given position with a given length." },
                { LanguageCodes.German, "Extrahiert eine Teilzeichenfolge ab einer bestimmten Position mit einer bestimmten Länge." }
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
                    "start", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The zero-based starting position of the substring." },
                        { LanguageCodes.German, "Die nullbasierte Startposition der Teilzeichenfolge." }
                    }
                },
                {
                    "length", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The number of characters to extract." },
                        { LanguageCodes.German, "Die Anzahl der zu extrahierenden Zeichen." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            string value = parameters[0].AsString() ?? string.Empty;
            int start = (int)(parameters[1].AsNumber() ?? 0.0);
            int length = (int)(parameters[2].AsNumber() ?? 0.0);

            if (start < 0 || start >= value.Length || length <= 0)
            {
                return FormulaValue.String(string.Empty);
            }

            int safeLength = System.Math.Min(length, value.Length - start);
            return FormulaValue.String(value.Substring(start, safeLength));
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
