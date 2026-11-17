using OpenDCP.Framework.Math.Interfaces;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Classes
{
    public sealed class EvaluationContext
    {
        public int Index { get; set; }
        public Dictionary<string, object[]> Values { get; set; } = new Dictionary<string, object[]>(System.StringComparer.OrdinalIgnoreCase);
        public IFunctionRegistry Functions { get; set; }
    }
}
