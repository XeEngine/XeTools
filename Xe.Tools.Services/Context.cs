using Xe.Tools.Projects;

namespace Xe.Tools.Services
{
	public class Context
	{
		public Context(IProject project)
		{
			ProjectService = new ProjectService(project);
			MessagesService = new MessageService(ProjectService);
		}

		public ProjectService ProjectService { get; }

		public MessageService MessagesService { get; }
	}
}
