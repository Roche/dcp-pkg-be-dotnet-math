using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;

namespace OpenDCP.Framework.Math.Classes.AST
{
    public sealed class PlaceholderNode : IExpressionNode
    {
        public string SensorName { get; }

        public PlaceholderNode(string sensor)
        {
            SensorName = sensor;
        }

        public FormulaValue Evaluate(EvaluationContext ctx)
        {
            if (ctx.Values != null && ctx.Values.TryGetValue(SensorName, out object[] seriesValues))
            {
                object rawValue = seriesValues[ctx.Index];

                switch (rawValue)
                {
                    case double doubleVal:
                        return FormulaValue.Number(doubleVal);

                    case bool boolVal:
                        return FormulaValue.Boolean(boolVal);

                    case int intVal:
                        return FormulaValue.Number(intVal);

                    case float floatVal:
                        return FormulaValue.Number(floatVal);

                    case string stringVal:
                        return FormulaValue.String(stringVal);

                    case null:
                        return FormulaValue.Number(double.NaN);

                    default:
                        try
                        {
                            double converted = Convert.ToDouble(rawValue);
                            return FormulaValue.Number(converted);
                        }
                        catch
                        {
                            return FormulaValue.Number(double.NaN);
                        }
                }
            }

            return FormulaValue.Number(double.NaN);
        }

        public string ExtractUoM(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            return sensorLookup.TryGetValue(SensorName, out var uom) ? uom : "dim";
        }

        public string GetDisplayName(EvaluationContext ctx, Dictionary<string, string> sensorLookup)
        {
            return sensorLookup.TryGetValue(SensorName, out var name) ? name : "Unknown";
        }
    }
}
