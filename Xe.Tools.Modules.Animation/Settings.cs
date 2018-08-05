using System.Collections.Generic;
using System.Threading.Tasks;
using Xe.Tools.Projects;

namespace Xe.Tools.Modules
{
    public partial class Animation
    {
        public class Settings : Settings<Settings>
        {
            public List<string> AnimationNames { get; set; } = new List<string>();

            public static async Task<Settings> OpenAsync(IProject project)
            {
                return await OpenAsync(project, "animation");
            }
        }
    }
}
