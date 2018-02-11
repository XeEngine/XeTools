using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Drawing;
using Xe.Game.Sequences;
using Xe.Game.Tilemaps;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.SequenceEditor.Controls
{
	public class SequenceSimulator : DrawingControl
	{
		private SequenceDrawer _drawer;
		private ProjectService _projectService;

		public SequenceSimulator()
		{
#if DEBUG
			Configurator.Configurator.Initialize();
			var project = new XeGsProj().Open(@"D:\Repository\vladya\soc\data\soc.game.proj.json");
			_projectService = new ProjectService(project);
#endif
		}

		protected override void OnDrawingCreated()
		{
			base.OnDrawingCreated();
			_drawer = new SequenceDrawer(_projectService, Drawing);

#if DEBUG
			var file = _projectService.Project.GetFilesByFormat("tilemap")
				.FirstOrDefault(x => x.Name == "debug_01.tmx");
			var tiledMap = new Tiled.Map(file.FullPath);
			var tileMap = new TilemapTiled().Open(tiledMap, Modules.ObjectExtensions.SwordsOfCalengal.Extensions);
			_drawer.Map = tileMap;
#endif
		}

		protected override void OnDrawRequired()
		{
			_drawer.Update(DeltaTime);
			_drawer.Render(new RectangleF()
			{
				X = 0,
				Y = 0,
				Width = (float)ActualWidth,
				Height = (float)ActualHeight
			}, true);
			base.OnDrawRequired();
		}

		protected override void OnDrawCompleted()
		{
			base.OnDrawCompleted();
		}
	}
}
