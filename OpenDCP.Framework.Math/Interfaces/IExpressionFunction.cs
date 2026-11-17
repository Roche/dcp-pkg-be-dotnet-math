using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Models;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Interfaces
{
    public interface IExpressionFunction
    {
        public string Name { get; }
        public string Category { get; }
        public IReadOnlyList<ParameterDefinition> Parameters { get; }
        public string Description(string? languageCode);
        public FormulaValue Evaluate(EvaluationContext context, IReadOnlyList<FormulaValue> parameters);
        public string ExtractUoM(EvaluationContext context, Dictionary<string, string> sensorLookup, IReadOnlyList<IExpressionNode> arguments);
        public string GetDisplayName(EvaluationContext context, Dictionary<string, string> sensorLookup, IReadOnlyList<IExpressionNode> arguments);
        public IReadOnlyList<ParameterDefinition> GetFullParameterDefinitions(string? languageCode);
        public string GetParameterDescription(string parameterName, string? languageCode);
    }
}
