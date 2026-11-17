namespace OpenDCP.Framework.Math.Models
{
    public class ParameterDefinitionDTO
    {
        public string Name { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Optional { get; set; }
    }
}
