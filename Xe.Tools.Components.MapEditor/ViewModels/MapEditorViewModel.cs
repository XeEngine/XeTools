using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.Services;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class MapEditorViewModel : BaseNotifyPropertyChanged
    {
        public static MapEditorViewModel Instance = new MapEditorViewModel();
        private ITileMap _tileMap;

        public ProjectService ProjectService { get; private set; }
        public AnimationService AnimationService { get; private set; }
        public IProject Project
        {
            get => ProjectService.Project;
            set
            {
                if (value != null)
                {
                    ProjectService = new ProjectService(value);
                    AnimationService = new AnimationService(ProjectService);
                }
            }
        }

        public ITileMap TileMap
        {
            get => _tileMap;
            set
            {
                _tileMap = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsTilemapLoaded));
            }
        }

        public bool IsTilemapLoaded => TileMap != null;

        public bool OpenTileMap(string fileName)
        {
            return (TileMap = TilemapService.Open(fileName)) != null;
        }
        
#if DEBUG
        private MapEditorViewModel()
        {
            var projPath = @"C:\Users\xeeynamo\Desktop\repo\vladya\soc\data\soc.game.proj.json";
            Project = new XeGsProj().Open(projPath);
            if (Project != null)
            {
                var file = Project.GetFilesByFormat("tilemap").SingleOrDefault(x => x.Name == "debug_01.tmx");
                if (file != null)
                {
                    OpenTileMap(file.FullPath);
                }
            }
        }
#endif
    }
}
