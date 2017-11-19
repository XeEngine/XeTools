using System;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap : ModuleBase
    {
        private TilemapTiled _tiledmap;
        private string _outputFileNameTilesetImage;

        public Tiledmap(ModuleInit init) : base(init) { }
        
        public override bool OpenFileData(string fileName)
        {
            _tiledmap = new TilemapTiled(fileName);
            return true;
        }
        public override bool OpenFileData(FileStream stream) { return true; }

        public override string GetOutputFileName()
        {
            return InputFileName.Replace(".tmx", ".map");
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
            _outputFileNameTilesetImage = Path.Combine(OutputWorkingPath, Path.Combine(basePath, tilesetImage));
            return new string[]
            {
                _outputFileNameTilesetImage
            };
        }

        public override void Export()
        {
            var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
            using (var fStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                Export(fStream);
            }
        }
    }
}
