namespace Xe.Tools.Components
{
    public interface IComponent
    {
        ComponentSettings Settings { get; }

        bool? ShowDialog();
    }
}
