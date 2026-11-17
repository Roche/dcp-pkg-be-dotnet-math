using System;

namespace OpenDCP.Framework.Math.Interfaces
{
    public interface IMathExpressionPoint<T>
    {
        DateTime Timestamp { get; }
        bool Good { get; }
        dynamic Value { get; }
    }
}
