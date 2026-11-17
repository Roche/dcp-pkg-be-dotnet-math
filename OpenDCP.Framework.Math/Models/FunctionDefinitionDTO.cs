using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Models
{
    public class FunctionDefinitionDTO
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IEnumerable<ParameterDefinitionDTO> Parameters { get; set; } = new List<ParameterDefinitionDTO>();
    }
}
