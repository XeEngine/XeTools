using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Xe.Tools
{
    /// <summary>
    /// Plugin management system to load external assemblies
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Plugin<T>
    {
        private static string _pathAssembly;
        private static string _dirAssembly;

        protected Type _type;

        public string Name { get; protected set; }

        protected Plugin(Type type)
        {
            Name = _type.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        protected static IEnumerable<T> GetPlugins(string folder, string[] extensions,
            Func<Type, T> funcComparison)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                if (_pathAssembly == null)
                    _pathAssembly = Assembly.GetExecutingAssembly().Location;
                if (_dirAssembly == null)
                    _dirAssembly = Path.GetDirectoryName(_pathAssembly);
                folder = _dirAssembly;
            }
            else if (!Directory.Exists(folder))
            {
                Log.Warning($"Directory ${folder} for plugins loading was not found.");
                return new T[0];
            }

            var fileNames = Directory.EnumerateFiles(folder)
                .Where(fileName => extensions.Contains(Path.GetExtension(fileName).ToLower()))
                .Select(fileName => Path.GetFullPath(fileName));

            var plugins = new List<T>();
            foreach (var file in fileNames)
            {
                try
                {
                    var assembly = Assembly.LoadFile(file);
                    foreach (var type in assembly.GetTypes())
                    {
                        var plugin = funcComparison.Invoke(type);
                        if (plugin != null)
                            plugins.Add(plugin);
                    }
                }
                catch (BadImageFormatException)
                {
                    Log.Warning($"File {file} is a bad format.");
                }
                catch (Exception e)
                {
                    Log.Error($"Exception on {file}: {e.Message}");
                }
            }
            return plugins;
        }
        
        protected MethodAccessException PrepareMethodNotFoundException(MethodBase method)
        {
            var name = method.Name;
            var parameters = method.GetParameters();
            var ret = (method as MethodInfo)?.ReturnType ?? null;

            var strReturn = ret != null ? $"{ret.Name} " : string.Empty;
            var strParams = string.Join(",", parameters.Select(x => $"{x.ParameterType} {x.Name}"));
            var contract = $"{strReturn}{name}({strParams})";
            return new MethodAccessException($"Method {contract} not found.");
        }
    }
}
