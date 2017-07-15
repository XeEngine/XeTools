namespace Xe.Tools.Components
{
    public interface IComponent
    {
        ComponentProperties Settings { get; }
        bool IsSettingsAvailable { get; }

        bool? ShowDialog();
        void ShowSettings();
    }
}
