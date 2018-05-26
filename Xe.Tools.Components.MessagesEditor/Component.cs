using System;

namespace Xe.Tools.Components.MessagesEditor
{
    public class Component : IComponent
    {
        public ComponentProperties Properties { get; private set; }

        public bool IsSettingsAvailable => false;

        public Component(ComponentProperties settings)
        {
            Properties = settings;
        }

        public MainWindow CreateMainWindow()
        {
			return new MainWindow();
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
                Name = "Message",
                ModuleName = "message",
                Editor = "Message editor",
                Description = "A tool for multi-language support."
            };
        }

        public void ShowSettings()
        {
            throw new NotImplementedException();
        }
    }
}
