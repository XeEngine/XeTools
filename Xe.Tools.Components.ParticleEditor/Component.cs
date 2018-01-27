using System;
using Xe.Tools.Components.ParticleEditor.Windows;

namespace Xe.Tools.Components.ParticleEditor
{
	public class Component : IComponent
	{
		public ComponentProperties Properties { get; private set; }

		public bool IsSettingsAvailable => false;

		public Component(ComponentProperties settings)
		{
			Properties = settings;
		}

		public Windows.ParticleEditor CreateMainWindow()
		{
			var window = new Windows.ParticleEditor();
			window.Open(Properties.Project, Properties.File);
			return window;
		}

		public void Show()
		{
			CreateMainWindow().Show();
		}
		public bool? ShowDialog()
		{
			return CreateMainWindow().ShowDialog();
		}


		public static ComponentInfo GetComponentInfo()
		{
			return new ComponentInfo()
			{
				Name = "ParticleEditor",
				ModuleName = "particleanim",
				Editor = "Particle animator editor",
				Description = "Particle animator editor."
			};
		}

		public void ShowSettings()
		{
			throw new NotImplementedException();
		}
	}
}
