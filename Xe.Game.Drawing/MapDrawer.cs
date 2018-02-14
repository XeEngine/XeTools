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

		private Dictionary<ObjectEntry, Entity> Entities { get; set; }
		private Dictionary<string, Entity> EntitiesByName { get; set; }
		private Dictionary<string, IEnumerable<Entity>> EntitiesByGroup { get; set; }

		public MapDrawer(ProjectService projService, IDrawing drawing)
		{
			ProjectService = projService;
			AnimationService = new AnimationService(projService);
			Drawing = drawing;
			TilemapDrawer = new TilemapDrawer(Drawing)
			{
				ActionDrawObject = RenderEntity
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

		/// <summary>
		/// Load all the entitire from a map file and initialize them.
		/// </summary>
		protected void LoadEntities()
		{
			// Get all the entities
			var entities = Map?.Layers
				.FlatterLayers<LayerObjects>()
				.SelectMany(x => x.Objects)
				.Select(x => new Entity(x))
				.ToList();

			Entities = entities?
				.ToDictionary(x => x.Entry, x => x) ??
				new Dictionary<ObjectEntry, Entity>();
			EntitiesByName = entities?
				.Where(x => !string.IsNullOrWhiteSpace(x.Name))
				.GroupBy(x => x.Name)
				.ToDictionary(x => x.Key, x => x.FirstOrDefault()) ??
				new Dictionary<string, Entity>();
			EntitiesByGroup = entities?
				.Where(x => !string.IsNullOrWhiteSpace(x.Group))
				.GroupBy(x => x.Group)
				.ToDictionary(x => x.Key, x => x.AsEnumerable()) ??
				new Dictionary<string, IEnumerable<Entity>>();
		}

		/// <summary>
		/// Reset the state of all the entities at the loading state.
		/// </summary>
		protected void ResetEntities()
		{
			foreach (var entity in Entities)
				entity.Value.Reset();
		}

		protected Entity GetEntityByName(string name)
		{
			EntitiesByName.TryGetValue(name, out var entity);
			return entity;
		}

		protected IEnumerable<Entity> GetEntitesByGroup(string group)
		{
			EntitiesByGroup.TryGetValue(group, out var entities);
			return entities;
		}

		/// <summary>
		/// Draw an entity using the animation system.
		/// </summary>
		/// <param name="entry">Entity to render</param>
		/// <param name="x">Horizontal position</param>
		/// <param name="y">Vertical position</param>
		/// <param name="opacity">Level of opacity, from 0 to 1</param>
		private void RenderEntity(ObjectEntry entry, float x, float y, float opacity)
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
