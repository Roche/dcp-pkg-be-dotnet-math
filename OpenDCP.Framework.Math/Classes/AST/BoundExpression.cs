using OpenDCP.Framework.Math.Interfaces;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Classes.AST
{
    internal class BoundExpression
    {
        public IExpressionNode Ast { get; }
        public IReadOnlyList<string> Inputs { get; }
        public Dictionary<string, int> InputIndexMap { get; }

        public BoundExpression(IExpressionNode ast, IReadOnlyList<string> inputs, Dictionary<string, int> inputIndexMap)
        {
            Ast = ast;
            Inputs = inputs;
            InputIndexMap = inputIndexMap;
        }
    }
}
