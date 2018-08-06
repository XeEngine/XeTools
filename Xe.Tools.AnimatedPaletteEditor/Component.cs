using Xe.Tools.Projects;

namespace Xe.Tools.Components.AnimatedPaletteEditor
{
	public class Component : IComponent
	{
		public ComponentProperties Properties { get; private set; }
		public IProject Project { get; private set; }

		public bool IsSettingsAvailable => true;

		public Component(ComponentProperties settings)
		{
			Properties = settings;
		}

		public void Show()
		{
			var dialog = new Views.MainView(Properties.Project, Properties.File);
			dialog.Show();
		}
		public bool? ShowDialog()
		{
			var dialog = new Views.MainView(Properties.Project, Properties.File);
			return dialog.ShowDialog();
		}
		public void ShowSettings()
		{
		}

		public static ComponentInfo GetComponentInfo()
		{
			return new ComponentInfo()
			{
				Name = "Animated palette",
				ModuleName = "palanim",
				Editor = "Animated palette editor",
				Description = "Used to create scripts to animate a palette."
			};
		}
	}
}
