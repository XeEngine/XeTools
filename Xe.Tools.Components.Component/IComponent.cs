namespace Xe.Tools.Components
{
    public interface IComponent
    {
        ComponentProperties Properties { get; }
        bool IsSettingsAvailable { get; }

        bool? ShowDialog();
        void ShowSettings();
    }
}
