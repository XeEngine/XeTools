using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Xe.Tools.Modules
{
    public class Module : Plugin<Module>
    {
        protected MethodInfo _methodValdiate;

        private Module(Type type) :
            base(type)
        {
            foreach (var method in type.GetMethods())
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

        public IModule CreateInstance(ModuleInit settings)
        {
            settings.Type = Type;
            return Activator.CreateInstance(Type, new object[] { settings }) as IModule;
        }

        public bool Validate(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("File not found", filename);
            if (_methodValdiate == null)
                throw PrepareMethodNotFoundException(MethodBase.GetCurrentMethod());
            return (bool)_methodValdiate.Invoke(null, new object[] { filename });
        }


        public static IEnumerable<Module> GetModules(string folder = null)
        {
            return GetPlugins(folder,
                new string[] { ".exe", ".dll", ".module" }, type =>
                {
                    try
                    {
                        if (type.Namespace == null)
                            return null;
                        if (!type.GetInterfaces().Any(x => x.FullName == "Xe.Tools.Modules.IModule"))
                            return null;
                        return new Module(type);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                    }
                    return null;
                });
        }
    }
}
