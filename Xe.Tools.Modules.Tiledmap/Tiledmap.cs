﻿using System;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Tilemaps;
using Xe.Tools.Modules.ObjectExtensions;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap : ModuleBase
    {
        private Map _tiledmap;
        private string _outputFileNameTilesetImage;
        private ObjectExtensionDefinition[] ObjectExtensionDefinitions = SwordsOfCalengal.Extensions;

        public Tiledmap(ModuleInit init) : base(init) { }
        
        public override bool OpenFileData(string fileName)
        {
            _tiledmap = new TilemapTiled().Open(fileName, ObjectExtensionDefinitions);
            return true;
        }
        public override bool OpenFileData(FileStream stream) { return true; }

        public override string GetOutputFileName()
        {
            return InputFileName.Replace(".tmx", ".map");
        }

        public override IEnumerable<string> GetSecondaryInputFileNames()
        {
            var basePath = Path.GetDirectoryName(_tiledmap.FileName);
            var filesList = new List<string>(_tiledmap.Tilesets.Count);
            foreach (var item in _tiledmap.Tilesets)
            {
                if (!string.IsNullOrEmpty(item.ExternalTileset))
                {
                    if (!Path.IsPathRooted(item.ExternalTileset))
                        filesList.Add(Path.Combine(basePath, item.ExternalTileset));
                    else
                        filesList.Add(item.ExternalTileset);
                }
                if (!string.IsNullOrEmpty(item.ImagePath))
                {
                    if (!Path.IsPathRooted(item.ImagePath))
                        filesList.Add(Path.Combine(basePath, item.ImagePath));
                    else
                        filesList.Add(item.ImagePath);
                }
            }
            return filesList;
        }

        public string GetTilesetFileName()
        {
            var basePath = Path.GetDirectoryName(InputFileName);
            var tilesetImage = $"{Path.GetFileNameWithoutExtension(OutputFileName)}.png";
            return Path.Combine(basePath, tilesetImage);
        }

        public override IEnumerable<string> GetSecondaryOutputFileNames()
        {
            _outputFileNameTilesetImage = Path.Combine(OutputWorkingPath, GetTilesetFileName());
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
