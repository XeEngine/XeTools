using System;
using Xe.Tools.Components.SequenceEditor.Windows;

namespace Xe.Tools.Components.SequenceEditor
{
	public class Component : IComponent
	{
		public ComponentProperties Properties { get; private set; }

		public bool IsSettingsAvailable => false;

		public Component(ComponentProperties settings)
		{
			Properties = settings;
		}

		public Windows.SequenceEditor CreateMainWindow()
		{
			var window = new Windows.SequenceEditor();
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
				Name = "SequenceEditor",
				ModuleName = "sequence",
				Editor = "Sequence editor",
				Description = "Editor used to create, edit and test seqeunces. A sequence is a cutscene or a programming-driven event."
			};
		}

		public void ShowSettings()
		{
			throw new NotImplementedException();
		}
	}
}
