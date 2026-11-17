using System;

namespace OpenDCP.Framework.Math.Models
{
    public class EvaluationPoint
    {
        DateTime ObsId { get; set; }
        public bool Good { get; set; }
        public dynamic Value { get; set; }
    }
}
