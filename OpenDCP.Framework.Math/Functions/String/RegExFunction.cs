using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenDCP.Framework.Math.Functions.String
{
    public sealed class RegExFunction : FunctionBase
    {
        public override string Name => "regex";

        public override string Category => "String";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(string), optional: false),
            new ParameterDefinition(name: "pattern", type: typeof(string), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Matches a string against a regular expression pattern and returns the first match, or an empty string if no match is found." },
                { LanguageCodes.German, "Vergleicht eine Zeichenfolge mit einem regulären Ausdruck und gibt die erste Übereinstimmung zurück, oder eine leere Zeichenfolge, wenn keine Übereinstimmung gefunden wird." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The string value to search." },
                        { LanguageCodes.German, "Der zu durchsuchende Zeichenfolgenwert." }
                    }
                },
                {
                    "pattern", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The regular expression pattern to match against." },
                        { LanguageCodes.German, "Das reguläre Ausdrucksmuster, gegen das abgeglichen werden soll." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            string value = parameters[0].AsString() ?? string.Empty;
            string pattern = parameters[1].AsString() ?? string.Empty;

            var match = Regex.Match(value, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
            return FormulaValue.String(match.Success ? match.Value : string.Empty);
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
