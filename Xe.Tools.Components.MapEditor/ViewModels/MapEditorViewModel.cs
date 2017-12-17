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
        private Tiled.Map _tiledMap;
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

        public string MapName
        {
            get
            {
                var fileName = TileMap.FileName;
                return string.IsNullOrEmpty(fileName) ?
                    "<unknown>" : Path.GetFileNameWithoutExtension(fileName);
            }
        }

        public bool IsTilemapLoaded => TileMap != null;

        public bool OpenTileMap(IProjectFile file)
        {
            var fileName = file.FullPath;
            if (File.Exists(fileName))
            {
                _file = file;
                _tiledMap = new Tiled.Map(fileName);
                TileMap = new TilemapTiled().Open(_tiledMap, Modules.ObjectExtensions.SwordsOfCalengal.Extensions);
                if (TileMap.LayersDefinition == null)
                {
                    TileMap.LayersDefinition =
                        TilemapService.LayerNames
                        .OrderBy(x => x.Order)
                        .Select(x => new LayerDefinition()
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).ToList();
                }
                foreach (var layer in TileMap.Layers.FlatterLayers())
                    layer.DefinitionId = layer.DefinitionId;
                return true;
            }
            return false;
        }

        public void Save()
        {
            new TilemapTiled().Save(TileMap, _tiledMap).Save(_file.FullPath);
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
