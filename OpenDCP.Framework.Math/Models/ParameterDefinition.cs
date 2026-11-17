using System;

namespace OpenDCP.Framework.Math.Models
{
    public class ParameterDefinition
    {
        public string Name { get; }
        public Type Type { get; }
        public bool Optional { get; }
        public string? Description { get; }

        public ParameterDefinition(string name, Type type, bool optional, string? description = null)
        {
            Name = name;
            Type = type;
            Optional = optional;
            Description = description;
        }

        public ParameterDefinition WithDescription(string description) => new ParameterDefinition(Name, Type, Optional, description);
    }
}
