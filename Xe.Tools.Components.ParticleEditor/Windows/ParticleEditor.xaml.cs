using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xe.Tools.Components.ParticleEditor.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Services;

namespace Xe.Tools.Components.ParticleEditor.Windows
{
    /// <summary>
    /// Interaction logic for ParticleEditor.xaml
    /// </summary>
    public partial class ParticleEditor : Window
    {
		private Timer _timer = new Timer(8.0);

		public ParticleEditorViewModel ViewModel => DataContext as ParticleEditorViewModel;

		public ParticleEditor()
        {
			InitializeComponent();

			_timer.Elapsed += (s, e) =>
			{
				Application.Current?.Dispatcher.Invoke(new Action(() =>
				{
					textRenderingTime.Text = $"Rendering time {particlePanel.LastDrawTime}";
					textPresentationTime.Text = $"Presentation time {particlePanel.LastDrawAndPresentTime - particlePanel.LastDrawTime}";
					textDeltaTime.Text = $"Delta time {particlePanel.DeltaTime}";
				}));
			};
			_timer.Start();

#if DEBUG
			//Configurator.Configurator.Initialize();
			//var projectFactory = new XeGsProj();
			//var project = projectFactory.Open(@"D:\Xe\Repo\vladya\soc\data\soc.game.proj.json");
			//if (project != null)
			//{
			//	var projectService = new ProjectService(project);
			//	DataContext = new ParticleEditorViewModel(projectService);
			//	particlePanel.DataContext = DataContext;
			//}
#endif
		}

		public void Open(IProject project, IProjectFile projectFile)
		{
			var projectService = new ProjectService(project);
			DataContext = new ParticleEditorViewModel(projectService);
			particlePanel.DataContext = DataContext;
		}

		private void ButtonCreateEffect_Click(object sender, RoutedEventArgs e)
		{
			var particleEffect = new Game.Particles.Effect()
			{
				Multiplier = 1.0,
				Speed = 1.0,
				Duration = 10.0,
			};
			var ctrlEffect = new Controls.Effect()
			{
				DataContext = new ParticleEffectsViewModel(
					ViewModel.ParticleSystem,
					ViewModel.SelectedParticleGroup,
					particleEffect)
			};
			ctrlEffect.RequireRemoval += CtrlEffect_RequireRemoval;

			ViewModel.SelectedParticleGroup?.Effects.Add(particleEffect);
			panelParticleGroupEffects.Children.Add(ctrlEffect);
		}

		private void CtrlEffect_RequireRemoval(Controls.Effect obj)
		{
			ViewModel.SelectedParticleGroup?.Effects.Remove(obj.ViewModel.Effect);
			panelParticleGroupEffects.Children.Remove(obj);
		}
	}
}
