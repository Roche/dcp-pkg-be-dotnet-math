namespace OpenDCP.Framework.Math.Interfaces
{
    public interface IExpressionParser
    {
        IExpressionNode Parse(string expression);
    }
}
