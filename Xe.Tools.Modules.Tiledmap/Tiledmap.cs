using System;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap : ModuleBase
    {
        private TilemapTiled _tiledmap;

        public Tiledmap(ModuleInit init) : base(init) { }
        
        public override bool OpenFileData(string fileName)
        {
            _tiledmap = new TilemapTiled(fileName);
            return true;
        }
        public override bool OpenFileData(FileStream stream) { return true; }

        public override string GetOutputFileName()
        {
            var extIndex = InputFileName.IndexOf(".json");
            if (extIndex >= 0)
            {
                return InputFileName.Substring(0, extIndex);
            }
            return InputFileName;
        }

        public override IEnumerable<string> GetSecondaryInputFileNames()
        {
            var basePath = _tiledmap.BasePath;
            var filesList = new List<string>(_tiledmap.Tilesets.Count);
            foreach (var item in _tiledmap.Tilesets)
            {
                if (item is TilemapTiled.Tileset tileset)
                {
                    var tsx = tileset.ExternalTileset;
                    if (!string.IsNullOrEmpty(tsx))
                        filesList.Add(Path.Combine(basePath, tsx));
                    var imagePath = tileset.ImageSource;
                    if (!string.IsNullOrEmpty(imagePath))
                        filesList.Add(Path.Combine(basePath, imagePath));
                }
                else
                {
                    filesList.Add(item.ImagePath);
                }
            }
            return filesList;
        }

        public override IEnumerable<string> GetSecondaryOutputFileNames()
        {
            var basePath = Path.GetDirectoryName(InputFileName);
            var tilesetImage = $"{Path.GetFileNameWithoutExtension(OutputFileName)}.png";
            return new string[]
            {
                Path.Combine(basePath, tilesetImage)
            };
        }

        public override void Export()
        {
            if (_tiledmap.TileSize.Width != 16 ||
                _tiledmap.TileSize.Height != 16)
                throw new Exception($"TileSize {_tiledmap.TileSize.Width}x{_tiledmap.TileSize.Height} is unsupported, expected 16x16.");

            var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
            using (var fStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                Export(fStream);
            }
        }
    }
}
