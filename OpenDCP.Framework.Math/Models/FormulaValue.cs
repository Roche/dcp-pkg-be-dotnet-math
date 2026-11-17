using OpenDCP.Framework.Math.Constants;

namespace OpenDCP.Framework.Math.Models
{
    public sealed class FormulaValue
    {
        public OutputType OutputType { get; }
        public double? NumberValue { get; }
        public bool? BooleanValue { get; }
        public string? StringValue { get; }

        public static FormulaValue Number(double? value)
        {
            return new FormulaValue(OutputType.Numeric, numberValue: value, booleanValue: null, stringValue: null);
        }

        public static FormulaValue Boolean(bool? value)
        {
            return new FormulaValue(OutputType.Boolean, numberValue: null, booleanValue: value, stringValue: null);
        }

        public static FormulaValue String(string? value)
        {
            return new FormulaValue(OutputType.String, numberValue: null, booleanValue: null, stringValue: value);
        }

        private FormulaValue(OutputType outputType, double? numberValue, bool? booleanValue, string? stringValue)
        {
            OutputType = outputType;
            NumberValue = numberValue;
            BooleanValue = booleanValue;
            StringValue = stringValue;
        }

        public double? AsNumber() => NumberValue;
        public bool? AsBool() => BooleanValue;
        public string? AsString() => StringValue;

        public bool GetBoolValue()
        {

            if (BooleanValue.HasValue)
            {
                return BooleanValue.Value;
            }

            //values different then 0 are true
            if (NumberValue.HasValue)
            {
                return NumberValue.Value != 0.0;
            }

            return false;
        }
    }
}
