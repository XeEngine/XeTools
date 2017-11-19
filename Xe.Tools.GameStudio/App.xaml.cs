using System.Windows;

namespace Xe.Tools.GameStudio
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Factory.Register<Xe.Drawing.IDrawing, Xe.Drawing.DrawingDirectX>(Factory.Scope.Instance);
        }
    }
}
