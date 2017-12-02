using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        private Map _tileMap;
        private IProjectFile _file;

        #region Delegates and events
        public delegate void TilemapChangedHandler(MapEditorViewModel sender, Map tileMap);
        public event TilemapChangedHandler OnTilemapChanged;
        #endregion

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

        public Map TileMap
        {
            get => _tileMap;
            set
            {
                _tileMap = value;
                OnTilemapChanged?.Invoke(this, _tileMap);
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsTilemapLoaded));
            }
        }

        public string MapName => Path.GetFileNameWithoutExtension(_file?.Name ?? "<unknown>");

        public bool IsTilemapLoaded => TileMap != null;

        public bool OpenTileMap(IProjectFile file)
        {
            var result = (TileMap = TilemapService.Open(file.FullPath)) != null;
            if (result)
                _file = file;
            return result;
        }

        public void Save()
        {
            TilemapTiled.Save(TileMap, new Tiled.Map()
            {
                FileName = _file.FullPath
            }).Save(_file.FullPath);
        }
        
#if DEBUG
        private MapEditorViewModel()
        {
            var projPath = @"D:\Xe\Repo\vladya\soc\data\soc.game.proj.json";
            if (File.Exists(projPath))
            {
                Project = new XeGsProj().Open(projPath);
                if (Project != null)
                {
                    var file = Project.GetFilesByFormat("tilemap").SingleOrDefault(x => x.Name == "debug_01.tmx");
                    if (file != null)
                        OpenTileMap(file);
                }
            }
        }
#endif
    }
}
