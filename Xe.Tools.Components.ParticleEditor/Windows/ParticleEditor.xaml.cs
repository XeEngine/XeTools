using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.ParticleEditor.Windows
{
    /// <summary>
    /// Interaction logic for ParticleEditor.xaml
    /// </summary>
    public partial class ParticleEditor : WindowEx, IController
    {
		private Timer _timer = new Timer(8.0);
		private IProjectFile _projectFile;
		private Game.Particles.ParticlesData _particlesData = new Game.Particles.ParticlesData();

		public ParticleEditorViewModel ViewModel => DataContext as ParticleEditorViewModel;

		public Game.Particles.ParticlesData ParticleData => _particlesData;

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
			DataContext = new ParticleEditorViewModel(this, projectService);
			particlePanel.DataContext = DataContext;

			var path = projectFile.FullPath;
			if (File.Exists(path))
			{
				using (var stream = new StreamReader(path))
				{
					_projectFile = projectFile;
					_particlesData =
						JsonConvert.DeserializeObject<Xe.Game.Particles.ParticlesData>(
							stream.ReadToEnd()
						);
				}
			}
		}

		private void ButtonCreateEffect_Click(object sender, RoutedEventArgs e)
		{
			var particleEffect = new Game.Particles.Effect()
			{
				Multiplier = 1.0,
				Speed = 1.0,
				Duration = 10.0,
			};
			AddParticle(particleEffect);
		}

		private void CtrlEffect_RequireRemoval(Controls.Effect obj)
		{
			ViewModel.SelectedParticleGroup?.Effects.Remove(obj.ViewModel.Effect);
			panelParticleGroupEffects.Children.Remove(obj);
		}

		private void AddParticle(Game.Particles.Effect particleEffect)
		{
			ViewModel.SelectedParticleGroup?.Effects.Add(particleEffect);
			AddParticleCtrl(particleEffect);
		}

		private void AddParticleCtrl(Game.Particles.Effect particleEffect)
		{
			var ctrlEffect = new Controls.Effect()
			{
				DataContext = new ParticleEffectsViewModel(
					ViewModel.ParticleSystem,
					ViewModel.SelectedParticleGroup,
					particleEffect)
			};
			ctrlEffect.RequireRemoval += CtrlEffect_RequireRemoval;
			panelParticleGroupEffects.Children.Add(ctrlEffect);
		}

		protected override bool DoSaveChanges()
		{
			if (_projectFile != null && ViewModel.ParticlesData != null)
			{
				var path = _projectFile.FullPath;
				using (var stream = new StreamWriter(path))
				{
					ViewModel.SaveChanges();
					var str = JsonConvert.SerializeObject(ViewModel.ParticlesData, Formatting.Indented);
					stream.Write(str);
					return true;
				}
			}
			return false;
		}

		public void RefreshEffectsList()
		{
			if (ViewModel.SelectedParticleGroup != null)
			{
				panelParticleGroupEffects.Children.Clear();
				foreach (var item in ViewModel.SelectedParticleGroup.Effects)
				{
					AddParticleCtrl(item);
				}
			}
		}
	}
}
