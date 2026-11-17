using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenDCP.Framework.Math.Classes.AST
{
    public sealed class FunctionNode : IExpressionNode
    {
        public string Name { get; }
        public List<IExpressionNode> Arguments { get; }
        public FunctionNode(string name, List<IExpressionNode> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public FormulaValue Evaluate(EvaluationContext ctx)
        {
            var func = ctx.Functions.GetFunction(Name);

            var evaluated = Arguments.Select(x => x.Evaluate(ctx)).ToList();

            return func.Evaluate(ctx, evaluated);
        }

        public string ExtractUoM(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            var func = ctx.Functions.GetFunction(Name);
            return func.ExtractUoM(ctx, sensorLookup, Arguments);
        }

        public string GetDisplayName(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            var func = ctx.Functions.GetFunction(Name);

            return func.GetDisplayName(ctx, sensorLookup, Arguments);
        }
    }
}
