using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public class Module
    {
        private Type _type;
        private MethodInfo _methodValdiate;

        public string Name { get; private set; }

        private Module(Assembly assembly)
        {
            var type = assembly.GetTypes().Where(x => x.Namespace == "Xe.Tools.Modules").First();
            _type = type;
            Name = type.Name;

            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                if (method.IsStatic)
                {
                    switch (method.Name)
                    {
                        case "Validate":
                            _methodValdiate = method;
                            break;
                    }
                }
            }
        }

        public IModule CreateInstance(ModuleSettings settings)
        {
            return Activator.CreateInstance(_type, new object[] { settings }) as IModule;
        }
        public bool Validate(string filename)
        {
            /*if (!File.Exists(filename))
                throw new FileNotFoundException("File not found", filename);*/
            if (_methodValdiate == null)
                throw PrepareMethodNotFoundException(MethodBase.GetCurrentMethod());
            return (bool)_methodValdiate.Invoke(null, new object[] { filename });
        }

        private MethodAccessException PrepareMethodNotFoundException(MethodBase method)
        {
            var name = method.Name;
            var parameters = method.GetParameters();
            var ret = (method as MethodInfo)?.ReturnType ?? null;

            var strReturn = ret != null ? $"{ret.Name} " : string.Empty;
            var strParams = string.Join(",", parameters.Select(x => $"{x.ParameterType} {x.Name}"));
            var contract = $"{strReturn}{name}({strParams})";
            return new MethodAccessException($"Method {contract} not found.");
        }

        public static IEnumerable<Module> GetModules(string folder)
        {
            var files = Directory.EnumerateFiles(folder);
            var modules = new List<Module>();
            foreach (var file in files)
            {
                var fullPath = Path.GetFullPath(file);
                try
                {
                    var assembly = Assembly.LoadFile(fullPath);
                    var types = assembly.GetTypes();
                    bool isValid = false;
                    foreach (var type in types)
                    {
                        if (type.Namespace == "Xe.Tools.Modules")
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (isValid)
                        modules.Add(new Module(assembly));
                }
                catch (BadImageFormatException)
                {

                }
            }
            return modules;
        }
    }
}
