namespace Xe.Tools.Components
{
    public interface IComponent
    {
        ComponentProperties Properties { get; }
        bool IsSettingsAvailable { get; }

        void Show();
        bool? ShowDialog();
        void ShowSettings();
    }
}
