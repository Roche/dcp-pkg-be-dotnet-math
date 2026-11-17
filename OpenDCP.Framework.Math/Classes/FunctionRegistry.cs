using OpenDCP.Framework.Math.Functions.Math;
using OpenDCP.Framework.Math.Functions.PointAttributes;
using OpenDCP.Framework.Math.Functions.String;
using OpenDCP.Framework.Math.Interfaces;
using OpenDCP.Framework.Math.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenDCP.Framework.Math.Classes
{
    public class FunctionRegistry : IFunctionRegistry
    {
        private readonly Dictionary<string, IExpressionFunction> _functions = new Dictionary<string, IExpressionFunction>(StringComparer.OrdinalIgnoreCase);

        public bool Exists(string name)
        {
            return _functions.ContainsKey(name);
        }

        public IExpressionFunction GetFunction(string name)
        {
            return _functions[name];
        }

        public IEnumerable<IExpressionFunction> GetFunctions()
        {
            return _functions.Values;
        }

        public IEnumerable<FunctionDefinitionDTO> GetFunctionDefinitions(string? languageKey)
        {
            return GetFunctions()
                .Select(x => new FunctionDefinitionDTO
                {
                    Name = x.Name,
                    Category = x.Category,
                    Description = x.Description(languageKey),
                    Parameters = x.GetFullParameterDefinitions(languageKey)
                        .Select(p => new ParameterDefinitionDTO
                        {
                            Name = p.Name,
                            Description = p.Description ?? string.Empty,
                            TypeName = p.Type.Name,
                            Optional = p.Optional
                        })
                        .ToList()
                })
                .ToList();
        }

        public void RegisterMathFunctions()
        {
            Register(new AbsFunction());
            Register(new CeilingFunction());
            Register(new CosFunction());
            Register(new ExpFunction());
            Register(new FloorFunction());
            Register(new Log10Function());
            Register(new LogFunction());
            Register(new ModFunction());
            Register(new PiFunction());
            Register(new PowFunction());
            Register(new RemainderFunction());
            Register(new RoundFunction());
            Register(new SinFunction());
            Register(new TanFunction());
        }

        public void RegisterStringFunctions()
        {
            Register(new LeftFunction());
            Register(new MidFunction());
            Register(new ParseFunction());
            Register(new RegExFunction());
            Register(new RightFunction());
        }

        public void RegisterPointAttributesFunctions()
        {
            Register(new IsGoodFunction());
        }

        public void RegisterAllDefaultFunctions()
        {
            RegisterMathFunctions();
            RegisterStringFunctions();
            RegisterPointAttributesFunctions();
        }

        public void Register(IExpressionFunction function)
        {
            _functions.Add(function.Name, function);
        }

        public void Register(string directoryPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(directoryPath);

            if (!Directory.Exists(directoryPath)) return;

            var dllPaths = Directory.GetFiles(directoryPath, "*.dll");

            foreach (var dllPath in dllPaths)
            {
                try
                {
                    var loadContext = new FunctionLoadContext(dllPath);
                    var assembly = loadContext.LoadFromAssemblyPath(dllPath);

                    var functionTypes = assembly.GetTypes()
                        .Where(t => typeof(IExpressionFunction).IsAssignableFrom(t)
                                    && !t.IsAbstract
                                    && !t.IsInterface
                                    && t.GetConstructor(Type.EmptyTypes) != null);

                    foreach (var type in functionTypes)
                    {
                        var instance = (IExpressionFunction)Activator.CreateInstance(type)!;

                        if (!Exists(instance.Name))
                        {
                            Register(instance);
                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    continue;
                }
                catch (FileLoadException)
                {
                    continue;
                }
            }
        }
    }
}
