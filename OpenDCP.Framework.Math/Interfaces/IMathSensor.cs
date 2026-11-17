using OpenDCP.Framework.Math.Constants;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Interfaces
{
    public interface IMathSensor<T>
    {
        string UniqueId { get; }
        InterpolationMode InterpolationMode { get; }
        List<IMathExpressionPoint<T>> Points { get; }
    }
}
