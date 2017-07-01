using System.Collections.Generic;
using System.Linq;
using Xe.Tools.Modules;

namespace Xe.Tools.Builder
{
    public static partial class Program
    {
        private static Dictionary<string, Module> Modules { get; } =
            Module.GetModules("modules").ToDictionary(x => x.Name.ToLower(), x => x);
    }
}
