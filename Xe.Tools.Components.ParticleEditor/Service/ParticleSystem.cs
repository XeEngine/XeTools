using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Drawing;
using Xe.Game.Animations;
using Xe.Game.Drawing;
using Xe.Tools.Services;

namespace Xe.Tools.Components.ParticleEditor.Service
{
	public class ParticleBaseSystem
	{
		public AnimationDrawer AnimationDrawer { get; protected set; }
	}

	public class ParticleSystem : ParticleBaseSystem
	{
		public ProjectService ProjectService { get; }

		public AnimationService AnimationService { get; }

		public IDrawing Drawing { get; }

		public AnimationData AnimationData { get; private set; }

		public double Timer { get; set; }

		public IEnumerable<ParticleGroup> ParticleGroups { get; set; } = new List<ParticleGroup>();

		public ParticleSystem(ProjectService projectService, AnimationService animationService, IDrawing drawing)
		{
			ProjectService = projectService;
			AnimationService = animationService;
			Drawing = drawing;
		}

		public bool LoadAnimation(string name)
		{
			var file = AnimationService.ProjectFiles
				.FirstOrDefault(x => x.Name == name);
			if (file != null)
			{
				var animationData = AnimationService.GetAnimationData(file);
				if (animationData != null)
				{
					var basePath = Path.GetDirectoryName(file.FullPath);
					return LoadAnimation(animationData, basePath);
				}
			}
			return false;
		}

		public bool LoadAnimation(AnimationData animationData, string basePath)
		{
			AnimationData = animationData;
			AnimationDrawer?.Dispose();
			AnimationDrawer = new AnimationDrawer(animationData, Drawing, basePath);
			return true;
		}

		public void Update(double deltaTime)
		{
			Timer += deltaTime;
			foreach (var item in ParticleGroups)
			{
				item.Timer = Timer;
			}
		}

		public void Draw(double x, double y)
		{
			var entities = ParticleGroups.SelectMany(group => group.Entities)
				.OrderBy(entity => entity.Priority);
			foreach (var entity in entities)
			{
				entity.Draw(x, y);
			}
		}
	}
}
