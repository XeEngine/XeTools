using Xe.Tools.Components.MessagesEditor.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.MessagesEditor.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WindowMain : WindowEx
	{
		private Context context;
		private bool wasModified;

		public WindowMain()
		{
			InitializeComponent();
		}

		public void SetContext(IProjectFile file, Context context)
		{
			this.context = context;
			var vm = new MessagesViewModel(file, context);
			vm.OnNotifyModify += () =>
			{
				if (!wasModified)
				{
					AskExitConfirmation = true;
					wasModified = true;
				}
			};

			DataContext = vm;
		}

		protected override bool DoSaveChanges()
		{
			context.MessagesService.SaveChanges();
			return true;
		}
	}
}
