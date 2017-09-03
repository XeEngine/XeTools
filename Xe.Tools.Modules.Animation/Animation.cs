using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Animations;
using Xe.Tools.Services;

namespace Xe.Tools.Modules
{
    public partial class Animation : ModuleBase
    {
        private AnimationData AnimationData { get; set; }

        public Animation(ModuleInit init) : base(init) { }

        public override bool OpenFileData(FileStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                AnimationData = JsonConvert.DeserializeObject<AnimationData>(reader.ReadToEnd());
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
            return AnimationData.Textures.Select(x =>
            {
                return Path.Combine(basePath, x.Name);
            });
        }

        public override IEnumerable<string> GetSecondaryOutputFileNames()
        {
            var basePath = Path.GetDirectoryName(OutputFileName);
            return AnimationData.Textures.Select(x =>
            {
                return Path.Combine(basePath, x.Name);
            });
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
            foreach (var texture in AnimationData.Textures)
            {
                var input = Path.Combine(inputBasePath, texture.Name);
                var output = Path.Combine(outputBasePath, texture.Name);
                if (File.Exists(output))
                    File.Delete(output);

                if (texture.Transparencies?.Length > 0)
                {
                    ImageService.MakeTransparent(output, input, texture.Transparencies
                        .Select(x => new Color()
                        {
                            r = (byte)((x >> 24) & 0xFF),
                            g = (byte)((x >> 16) & 0xFF),
                            b = (byte)((x >> 8) & 0xFF)
                        }).ToArray());
                }
                else
                {
                    File.Copy(input, output);
                }
            }
        }
    }
}
