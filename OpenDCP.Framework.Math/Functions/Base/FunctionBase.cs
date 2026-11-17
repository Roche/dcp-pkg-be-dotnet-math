using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenDCP.Framework.Math.Functions.Base
{
    public abstract class FunctionBase : IExpressionFunction
    {
        public abstract string Name { get; }

        public abstract string Category { get; }

        public abstract IReadOnlyList<ParameterDefinition> Parameters { get; }

        protected abstract Dictionary<string, string> DescriptionTranslations { get; }

        protected abstract Dictionary<string, Dictionary<string, string>> ParameterTranslations { get; }

        public string Description(string? languageCode = LanguageCodes.Default)
        {
            var lang = string.IsNullOrEmpty(languageCode) ? LanguageCodes.Default : languageCode;

            return DescriptionTranslations.GetValueOrDefault(lang)
                ?? DescriptionTranslations.GetValueOrDefault(LanguageCodes.Default)
                ?? string.Empty;
        }

        public string GetParameterDescription(string parameterName, string? languageCode = LanguageCodes.Default)
        {
            var byLanguage = ParameterTranslations.GetValueOrDefault(parameterName);

            if (byLanguage == null) return string.Empty;

            var lang = string.IsNullOrEmpty(languageCode) ? LanguageCodes.Default : languageCode;

            return byLanguage.GetValueOrDefault(lang)
                ?? byLanguage.GetValueOrDefault(LanguageCodes.Default)
                ?? string.Empty;
        }

        public IReadOnlyList<ParameterDefinition> GetFullParameterDefinitions(string? languageCode = LanguageCodes.Default)
        {
            return Parameters
                .Select(p => p.WithDescription(GetParameterDescription(p.Name, languageCode)))
                .ToList();
        }

        public abstract FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters);

        public virtual string ExtractUoM(
            EvaluationContext context,
            Dictionary<string, string> sensorLookup,
            IReadOnlyList<IExpressionNode> arguments)
        {
            if (arguments == null || arguments.Count == 0) return string.Empty;

            return arguments[0].ExtractUoM(context, sensorLookup);
        }

        public virtual string GetDisplayName(
            EvaluationContext context,
            Dictionary<string, string> sensorLookup,
            IReadOnlyList<IExpressionNode> arguments)
        {
            if (arguments == null || arguments.Count == 0) return $"{Name}()";

            var argDisplays = arguments.Select(arg => arg.GetDisplayName(context, sensorLookup));

            return $"{Name}({string.Join(", ", argDisplays)})";
        }
    }
}
