using Xe.Tools.Components.ParticleEditor.Service;
using Xe.Tools.Components.ParticleEditor.ViewModels;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.ParticleEditor.Controls
{
    internal class ParticlePanel : DrawingControl
	{
		public ParticleSystem ParticleSystem { get; private set; }

		public ParticleEditorViewModel ViewModel => DataContext as ParticleEditorViewModel;
		
		protected override void OnDrawingCreated()
		{
			base.OnDrawingCreated();

			ParticleSystem = new ParticleSystem(ViewModel.ProjectService, ViewModel.AnimationService, Drawing);
			//ParticleSystem.Initialize(5, 64.0, 64.0);
			ViewModel.ParticleSystem = ParticleSystem;
		}

		protected override void OnDrawRequired()
		{
			Drawing.Clear(System.Drawing.Color.White);
			if (ParticleSystem != null)
			{
				ParticleSystem.Update(DeltaTime);
				ParticleSystem.Draw(ActualWidth / 2.0, ActualHeight / 2.0);
				
				ViewModel.SetTimer(ViewModel.Timer);
			}
		}

		protected override void OnDrawCompleted()
		{
			base.OnDrawCompleted();
		}
	}
}
