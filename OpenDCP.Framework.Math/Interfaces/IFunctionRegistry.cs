using OpenDCP.Framework.Math.Models;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Interfaces
{
    public interface IFunctionRegistry
    {
        public void Register(IExpressionFunction function);
        public void Register(string directoryPath);
        public IExpressionFunction GetFunction(string name);
        public IEnumerable<IExpressionFunction> GetFunctions();
        public bool Exists(string name);
        public IEnumerable<FunctionDefinitionDTO> GetFunctionDefinitions(string? languageKey);
    }
}
