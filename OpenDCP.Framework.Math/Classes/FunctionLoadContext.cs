using System.Reflection;
using System.Runtime.Loader;

namespace OpenDCP.Framework.Math.Classes
{
    public class FunctionLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public FunctionLoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var pluginResolved = _resolver.ResolveAssemblyToPath(assemblyName);

            if (pluginResolved != null)
            {
                return LoadFromAssemblyPath(pluginResolved);
            }

            return null;
        }
    }
}
