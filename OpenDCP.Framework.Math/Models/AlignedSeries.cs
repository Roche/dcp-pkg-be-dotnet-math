using System;
using System.Collections.Generic;


namespace OpenDCP.Framework.Math.Models
{
    internal sealed class AlignedSeries
    {
        public DateTime[] Timestamps { get; set; }
        public Dictionary<string, object[]> Values { get; set; }
    }
}
