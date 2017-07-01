using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Xe.Tools.Components
{
    public class Component : Plugin<Component>
    {
        private MethodInfo _methodGetComponentInfo;

        public ComponentInfo ComponentInfo { get; private set; }

        private Component(Type type) :
            base(type)
        {
            foreach (var method in type.GetMethods())
            {
                if (method.IsStatic)
                {
                    switch (method.Name)
                    {
                        case "GetComponentInfo":
                            _methodGetComponentInfo = method;
                            break;
                    }
                }
            }

            ComponentInfo = GetComponentInfo();

            if (ComponentInfo.ModuleName.Length > 2)
                Name = char.ToUpper(ComponentInfo.ModuleName[0]) + ComponentInfo.ModuleName.Substring(1);
            else if (ComponentInfo.ModuleName.Length > 1)
                Name = ComponentInfo.ModuleName.ToUpper();
            else
                Name = string.Empty;
        }

        public IComponent CreateInstance(ComponentSettings settings)
        {
            return Activator.CreateInstance(Type, new object[] { settings }) as IComponent;
        }

        private ComponentInfo GetComponentInfo()
        {
            if (_methodGetComponentInfo == null)
                throw PrepareMethodNotFoundException(MethodBase.GetCurrentMethod());
            return _methodGetComponentInfo?.Invoke(null, null) as ComponentInfo;
        }


        public static IEnumerable<Component> GetComponents(string folder = null)
        {
            return GetPlugins(folder,
                new string[] { ".exe", ".dll", ".component" }, type =>
                {
                    if (type.Namespace.IndexOf("Xe.Tools.Components") != 0)
                        return null;
                    if (!type.GetInterfaces().Any(x => x.FullName == "Xe.Tools.Components.IComponent"))
                        return null;
                    return new Component(type);
                });
        }
    }
}
