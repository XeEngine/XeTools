using System.Windows;
using Xe.Tools.Configurator;

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
            Configurator.Configurator.Initialize();
        }
    }
}
