using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Drawing;
using Xe.Game.Tilemaps;
using Xe.Tools.Services;
using Xe.Tools.Tilemap;

namespace Xe.Game.Drawing
{
	public partial class MapDrawer : IDisposable
	{
		public ProjectService ProjectService { get; }

		public AnimationService AnimationService { get; }

		public IDrawing Drawing { get; }

		public TilemapDrawer TilemapDrawer { get; }

		public ResourceService<string, AnimationDrawer> AnimationResources { get; }

		public Map Map
		{
			get => TilemapDrawer.Map;
			set
			{
				TilemapDrawer.Map = value;
				LoadEntities();
			}
		}

		public Dictionary<ObjectEntry, Entity> Entities { get; private set; }
		public Dictionary<string, IEnumerable<Entity>> EntitiesByName { get; private set; }
		public Dictionary<string, IEnumerable<Entity>> EntitiesByGroup { get; private set; }

		public MapDrawer(ProjectService projService, IDrawing drawing)
		{
			ProjectService = projService;
			AnimationService = new AnimationService(projService);
			Drawing = drawing;
			TilemapDrawer = new TilemapDrawer(Drawing)
			{
				ActionDrawObject = RenderObject
			};

			AnimationResources = new ResourceService<string, AnimationDrawer>(
				OnAnimationResourceLoad, OnAnimationResourceUnload
				);

			LoadEntities();
		}

		public virtual void Update(double deltaTime)
		{
			foreach (var entity in Entities)
				entity.Value.Update(deltaTime);
		}

		public virtual void Render(RectangleF rect, bool drawInvisibleObjects = false)
		{
			TilemapDrawer.DrawBackground(rect);
			TilemapDrawer.DrawMap(rect, drawInvisibleObjects);
		}

		public void Dispose()
		{
			TilemapDrawer?.Dispose();
			AnimationResources?.RemoveAll();
		}

		protected void LoadEntities()
		{
			Entities = Map?.Layers
				.FlatterLayers<LayerObjects>()
				.SelectMany(x => x.Objects)
				.Select(x => new Entity(x))
				.ToDictionary(x => x.Entry, x => x) ??
				new Dictionary<ObjectEntry, Entity>();
		}

		protected void ResetEntities()
		{
			foreach (var entity in Entities)
				entity.Value.Reset();
		}

		private void RenderObject(ObjectEntry entry, float x, float y, float opacity)
		{
			if (entry.AnimationData == null)
				return;

			var entity = Entities[entry];
			if (entity.Drawer == null)
				entity.Drawer = AnimationResources[entity.Entry.AnimationData];
			entity.Draw(x, y, opacity);
		}

		private bool OnAnimationResourceLoad(string filePath, out AnimationDrawer drawer)
		{
			var file = AnimationService.ProjectFiles
				.FirstOrDefault(x => x.Path == filePath);
			if (file != null)
			{
				var animationData = AnimationService.GetAnimationData(file);
				var basePath = Path.GetDirectoryName(file.FullPath);

				drawer = new AnimationDrawer(animationData, Drawing, basePath);
				return true;
			}
			drawer = null;
			return false;
		}

		private void OnAnimationResourceUnload(string filePath, AnimationDrawer drawer)
		{
			drawer.Dispose();
		}
	}
}
