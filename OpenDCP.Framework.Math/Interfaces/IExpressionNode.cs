using OpenDCP.Framework.Math.Classes;
using OpenDCP.Framework.Math.Models;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Interfaces
{
    public interface IExpressionNode
    {
        FormulaValue Evaluate(EvaluationContext ctx);

        string ExtractUoM(EvaluationContext ctx, Dictionary<string, string> sensorLookup);

        string GetDisplayName(EvaluationContext ctx, Dictionary<string, string> sensorLookup);
    }
}
