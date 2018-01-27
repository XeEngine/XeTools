using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Drawing;
using Xe.Game.Animations;
using Xe.Tools.Services;

namespace Xe.Game.Drawing
{
	public sealed class AnimationDrawer : IDisposable
	{
		#region public properties
		
		public AnimationData AnimationData { get; private set; }

		public IDrawing Drawing { get; private set; }

		public ResourceService<string, ISurface> Resources { get; }

		public string BasePath { get; set; }

		/// <summary>
		/// Notify the change of a frame, entity child create from this drawer.
		/// </summary>
		public event Action<AnimationEntityDrawer> FrameChanged;

		#endregion

		#region public methods

		public AnimationDrawer(AnimationData animationData, IDrawing drawing, string basePath = null, ResourceService<string, ISurface> resources = null)
		{
			AnimationData = animationData;
			Drawing = drawing;
			Resources = resources ?? new ResourceService<string, ISurface>(OnResourceTilesetLoad, OnResourceTilesetUnload);
			BasePath = basePath;

			DictionaryAnimationDefinitions = AnimationData.AnimationDefinitions.ToDictionary(x => x.Name, x => x);
			DictionaryAnimations = AnimationData.Animations.ToDictionary(x => x.Name, x => x);
			DictionaryFrames = AnimationData.Frames.ToDictionary(x => x.Name, x => x);
		}

		public AnimationEntityDrawer CreateEntity()
		{
			return new AnimationEntityDrawer(this);
		}

		public void Dispose()
		{
			Resources.RemoveAll();
		}

		#endregion

		#region private variables

		internal Dictionary<string, AnimationDefinition> DictionaryAnimationDefinitions { get; }
		internal Dictionary<string, Animation> DictionaryAnimations { get; }
		internal Dictionary<string, Frame> DictionaryFrames { get; }

		#endregion

		#region private methods

		internal void NotifyFrameChanged(AnimationEntityDrawer animationEntityDrawer)
		{
			FrameChanged?.Invoke(animationEntityDrawer);
		}

		private bool OnResourceTilesetLoad(string fileName, out ISurface surface)
		{
			var fullPath = Path.Combine(BasePath, fileName);
			surface = Drawing.CreateSurface(fullPath,
				new System.Drawing.Color[]
				{
					System.Drawing.Color.FromArgb(255, 255, 0, 255),
					System.Drawing.Color.FromArgb(255, 255, 128, 0),
				});
			return surface != null;
		}

		private void OnResourceTilesetUnload(string fileName, ISurface surface)
		{
			surface?.Dispose();
		}

		#endregion
	}
}
