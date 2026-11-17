using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Functions.Base;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Functions.PointAttributes
{
    public sealed class IsGoodFunction : FunctionBase
    {
        public override string Name => "isgood";

        public override string Category => "Point Attributes";

        public override IReadOnlyList<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>
        {
            new ParameterDefinition(name: "value", type: typeof(object), optional: false)
        };

        protected override Dictionary<string, string> DescriptionTranslations { get; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { LanguageCodes.English, "Checks whether a point's value is valid - not null, empty, N/A, undefined." },
                { LanguageCodes.German, "Prüft, ob der Wert eines Punkts gültig ist - nicht null, leer, N/A oder undefiniert." }
            };

        protected override Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "value", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { LanguageCodes.English, "The point value to validate." },
                        { LanguageCodes.German, "Der zu validierende Punktwert." }
                    }
                }
            };

        public override FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters)
        {
            var value = parameters[0];

            return FormulaValue.Boolean(IsValid(value));
        }

        private static bool IsValid(FormulaValue value)
        {
            if (value == null)
            {
                return false;
            }

            switch (value.OutputType)
            {
                case OutputType.Numeric:
                    if (!value.NumberValue.HasValue)
                    {
                        return false;
                    }

                    if (double.IsNaN(value.NumberValue.Value) || double.IsInfinity(value.NumberValue.Value))
                    {
                        return false;
                    }

                    return true;

                case OutputType.String:
                    return !string.IsNullOrWhiteSpace(value.StringValue);

                case OutputType.Boolean:
                    return value.BooleanValue.HasValue;

                default:
                    return false;
            }
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
