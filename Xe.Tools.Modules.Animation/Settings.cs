using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public partial class Animation
    {
        public class Settings : Settings<Settings>
        {
            public List<string> AnimationNames { get; set; } = new List<string>();

            public static async Task<Settings> OpenAsync(Project project)
            {
                return await OpenAsync(project, "animation");
            }
        }
    }
}
