using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Tools.Services;

namespace Xe.Tools.Modules
{
    using Fonts = Game.Fonts;

    public partial class Font : ModuleBase
    {
        private Fonts.Font MyFont { get; set; }

        public Font(ModuleInit init) : base(init) { }

        public override bool OpenFileData(FileStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                MyFont = JsonConvert.DeserializeObject<Fonts.Font>(reader.ReadToEnd());
            }
            return true;
        }

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
            var basePath = Path.GetDirectoryName(InputFileName);
            return MyFont.Tables.Select(x => x.Texture)
                .Distinct()
                .Select(x => Path.Combine(basePath, x));
        }

        public override IEnumerable<string> GetSecondaryOutputFileNames()
        {
            var basePath = Path.GetDirectoryName(OutputFileName);
            return MyFont.Tables.Select(x => x.Texture)
                .Distinct()
                .Select(x => Path.Combine(basePath, x));
        }

        public override void Export()
        {
            var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
            using (var stream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                Export(stream);
            }

            var inputBasePath = Path.Combine(InputWorkingPath, Path.GetDirectoryName(InputFileName));
            var outputBasePath = Path.Combine(OutputWorkingPath, Path.GetDirectoryName(OutputFileName));
            foreach (var table in MyFont.Tables)
            {
                var input = Path.Combine(inputBasePath, table.Texture);
                var output = Path.Combine(outputBasePath, table.Texture);
                if (File.Exists(output))
                    File.Delete(output);
                ImageService.MakeTransparent(output, input, new Color[]
                {
                    new Color() { r = 255, g = 0, b = 255 },
                    new Color() { r = 255, g = 128, b = 0 },
                });
            }
        }
    }
}
