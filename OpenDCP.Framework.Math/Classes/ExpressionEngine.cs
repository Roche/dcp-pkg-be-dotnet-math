using OpenDCP.Framework.Math.Classes.AST;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenDCP.Framework.Math.Classes
{
    public sealed class ExpressionEngine
    {
        private readonly IFunctionRegistry _functions;
        private readonly IExpressionParser _parser;

        public ExpressionEngine(IExpressionParser parser, IFunctionRegistry functions)
        {
            _parser = parser;
            _functions = functions;
        }

        public List<(DateTime Timestamp, dynamic Value, bool Good)> Evaluate<T>(
            string expression,
            List<IMathSensor<T>> rawData)
        {
            //Parse
            var ast = _parser.Parse(expression);

            //Bind
            var bound = Bind(ast, rawData.Select(x => x.UniqueId).ToList(), _functions);

            //Align
            var aligned = Align(rawData);

            //Execute
            var ctx = new EvaluationContext
            {
                Functions = _functions,
                Values = aligned.Values
            };

            var result = new List<(DateTime Timestamp, dynamic Value, bool Good)>();

            for (int i = 0; i < aligned.Timestamps.Length; i++)
            {
                ctx.Index = i;
                FormulaValue calculatedValue = bound.Ast.Evaluate(ctx);

                dynamic finalResultValue;
                bool isGood = true;
                if (calculatedValue.AsBool().HasValue)
                {
                    finalResultValue = calculatedValue.AsBool().Value;
                }
                else if (calculatedValue.AsString() != null)
                {
                    finalResultValue = calculatedValue.AsString();
                }
                else
                {
                    finalResultValue = calculatedValue.AsNumber();

                    if (finalResultValue is double num && (double.IsNaN(num) || double.IsInfinity(num)))
                    {
                        isGood = false;
                    }
                }

                if (finalResultValue is null)
                {
                    isGood = false;
                    finalResultValue = double.NaN;
                }

                result.Add((
                    Timestamp: aligned.Timestamps[i],
                    Value: finalResultValue,
                    Good: isGood
                ));
            }

            return result;
        }

        public void ValidateExpressions(List<string> expressions)
        {
            if (expressions == null) return;

            foreach (var expression in expressions.Select(e => e.Trim()))
            {
                if (string.IsNullOrEmpty(expression)) continue;

                try
                {
                    IExpressionNode rootNode = _parser.Parse(expression);
                    if (rootNode == null)
                    {
                        throw new Exception("Parser returned an empty expression tree.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Invalid Expression syntax detected: {expression}. Details: {ex.Message}");
                }
            }
        }

        public string GetExpressionDisplayName(Dictionary<string, string> sensorLookup, string expression)
        {
            if (string.IsNullOrEmpty(expression)) return string.Empty;

            IExpressionNode rootNode = _parser.Parse(expression);

            var ctx = new EvaluationContext
            {
                Functions = _functions
            };

            string displayName = rootNode.GetDisplayName(ctx, sensorLookup);

            return displayName;
        }

        public string GetExpressionUoM(Dictionary<string, string> sensorLookup, string expression)
        {
            if (string.IsNullOrEmpty(expression)) return string.Empty;

            IExpressionNode rootNode = _parser.Parse(expression);

            var ctx = new EvaluationContext
            {
                Functions = _functions
            };

            string uom = rootNode.ExtractUoM(ctx, sensorLookup);

            return Regex.Replace(uom, @"\s+", " ").Trim();
        }

        public List<string> ExtractWebIds(string expression)
        {
            if (string.IsNullOrEmpty(expression)) return new List<string>();

            IExpressionNode rootNode = _parser.Parse(expression);

            var sensorNames = new HashSet<string>();
            FindPlaceholderNodes(rootNode, sensorNames);

            return sensorNames.ToList();
        }

        private void FindPlaceholderNodes(IExpressionNode node, HashSet<string> collectedNames)
        {
            if (node == null) return;

            switch (node)
            {
                case BinaryNode b:
                    FindPlaceholderNodes(b.Left, collectedNames);
                    FindPlaceholderNodes(b.Right, collectedNames);
                    break;
                case PlaceholderNode p:
                    if (!string.IsNullOrEmpty(p.SensorName))
                    {
                        collectedNames.Add(p.SensorName);
                    }
                    break;
                case FunctionNode f:
                    if (f.Arguments != null)
                    {
                        foreach (var arg in f.Arguments)
                        {
                            FindPlaceholderNodes(arg, collectedNames);
                        }
                    }
                    break;
                case ConstantNode _:
                default:
                    break;
            }
        }

        private BoundExpression Bind(
            IExpressionNode ast,
            IReadOnlyList<string> availableSeries,
            IFunctionRegistry functions)
        {
            var indexMap = availableSeries.Select((name, index) => new
            {
                name,
                index
            }).ToDictionary(x => x.name, x => x.index);

            //Validation
            List<string> validationErrors = ValidateAst(ast, indexMap);

            if (validationErrors.Any())
            {
                throw new ArgumentException($"Validation failed with errors: {string.Join(" | ", validationErrors)}");
            }

            var boundAst = BindNode(ast, indexMap);

            return new BoundExpression(boundAst, availableSeries, indexMap);
        }

        private IExpressionNode BindNode(IExpressionNode node, Dictionary<string, int> map)
        {
            switch (node)
            {
                case ConstantNode c:
                    return c;
                case PlaceholderNode p:
                    return new PlaceholderNode(p.SensorName);
                case BinaryNode b:
                    return new BinaryNode(
                        b.Operation,
                        BindNode(b.Left, map),
                        BindNode(b.Right, map)
                       );
                case FunctionNode f:
                    return new FunctionNode(f.Name, f.Arguments.Select(a => BindNode(a, map)).ToList());
                case UnaryMinusNode u:
                    return new UnaryMinusNode(BindNode(u.Operand, map));
                default:
                    throw new NotSupportedException(node.GetType().ToString());
            }
        }

        private AlignedSeries Align<T>(List<IMathSensor<T>> data)
        {
            var result = new AlignedSeries();

            if (data == null || data.Count == 0)
            {
                result.Timestamps = Array.Empty<DateTime>();
                result.Values = new Dictionary<string, object[]>();
                return result;
            }

            var masterTimestamps = data
                .SelectMany(x => x.Points ?? new List<IMathExpressionPoint<T>>())
                .Select(dp => dp.Timestamp)
                .Distinct()
                .OrderBy(t => t)
                .ToArray();

            result.Timestamps = masterTimestamps;
            result.Values = new Dictionary<string, object[]>(StringComparer.OrdinalIgnoreCase);

            foreach (var sensor in data)
            {
                var alignedValues = new object[masterTimestamps.Length];
                var points = sensor.Points ?? new List<IMathExpressionPoint<T>>();

                var sortedPoints = points.OrderBy(dp => dp.Timestamp).ToList();
                int pointer = 0;

                for (int i = 0; i < masterTimestamps.Length; i++)
                {
                    alignedValues[i] = InterpolateValue(sortedPoints, masterTimestamps[i], ref pointer, sensor.InterpolationMode);
                }

                result.Values[sensor.UniqueId] = alignedValues;
            }

            return result;
        }

        private object InterpolateValue<T>(List<IMathExpressionPoint<T>> dataPoints, DateTime targetTime, ref int pointer, InterpolationMode mode)
        {
            if (dataPoints == null || dataPoints.Count == 0)
                return double.NaN;

            while (pointer + 1 < dataPoints.Count && dataPoints[pointer + 1].Timestamp <= targetTime)
            {
                pointer++;
            }

            IMathExpressionPoint<T> lower = dataPoints[pointer];
            IMathExpressionPoint<T> upper = pointer + 1 < dataPoints.Count ? dataPoints[pointer + 1] : null;

            if (lower?.Value is bool boolValue)
            {
                return lower.Good ? boolValue : false;
            }
            if (upper?.Value is bool upperBoolValue && lower is null)
            {
                return upper.Good ? upperBoolValue : false;
            }

            if (lower?.Value is string stringValue)
            {
                return lower.Good ? stringValue : null;
            }
            if (upper?.Value is string upperStringValue && lower is null)
            {
                return upper.Good ? upperStringValue : null;
            }

            if (lower is null || upper is null)
            {
                return TryConvertToDouble(lower?.Value)
                    ?? TryConvertToDouble(upper?.Value)
                    ?? double.NaN;
            }

            if (lower.Timestamp == targetTime)
            {
                return TryConvertToDouble(lower.Value) ?? double.NaN;
            }

            if (mode == InterpolationMode.StepWise)
            {
                return lower.Good ? TryConvertToDouble(lower.Value) : double.NaN;
            }

            if (!upper.Good)
                return lower.Value is null ? double.NaN : TryConvertToDouble(lower.Value);

            if (!lower.Good)
                return double.NaN;

            double? v1 = TryConvertToDouble(lower.Value);
            double? v2 = TryConvertToDouble(upper.Value);

            if (!v1.HasValue && !v2.HasValue) return double.NaN;
            if (!v1.HasValue) return v2.Value;
            if (!v2.HasValue) return v1.Value;

            double t1 = lower.Timestamp.Ticks;
            double t2 = upper.Timestamp.Ticks;
            double t = targetTime.Ticks;

            return v1.Value + (t - t1) / (t2 - t1) * (v2.Value - v1.Value);
        }

        private static double? TryConvertToDouble(object value)
        {
            if (value == null)
                return null;

            if (value is string s && s.Equals("NA", StringComparison.OrdinalIgnoreCase))
                return null;

            try
            {
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        private List<string> ValidateAst(IExpressionNode node, Dictionary<string, int> map)
        {
            var errors = new List<string>();
            ValidateAstRecursive(node, map, errors);
            return errors;
        }

        private void ValidateAstRecursive(IExpressionNode node, Dictionary<string, int> map, List<string> errors)
        {
            if (node == null) return;

            switch (node)
            {
                case PlaceholderNode p:
                    if (!map.ContainsKey(p.SensorName))
                    {
                        errors.Add($"Formula uses sensor parameter '{p.SensorName}', which was not provided in raw dataset.");
                    }
                    break;

                case BinaryNode b:
                    ValidateAstRecursive(b.Left, map, errors);
                    ValidateAstRecursive(b.Right, map, errors);
                    break;

                case FunctionNode f:
                    if (!_functions.Exists(f.Name))
                    {
                        errors.Add($"The function '{f.Name}' is not registered in the system library.");
                    }

                    foreach (var arg in f.Arguments)
                    {
                        ValidateAstRecursive(arg, map, errors);
                    }
                    break;
            }
        }
    }
}
