using Xe.Tools.Projects;
using Xe.Tools.Services;

namespace Xe.Tools.Components
{
	public class ComponentProperties
    {
        public IProject Project { get; set; }

		public IProjectFile File { get; set; }

		public Context Context { get; set; }

		//public Project ProjectLegacy;
		//public Container Container;
		//public Item Item;
	}
}
