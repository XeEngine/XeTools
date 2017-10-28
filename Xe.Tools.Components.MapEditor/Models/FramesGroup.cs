using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xe.Drawing;

namespace Xe.Tools.Components.MapEditor.Models
{
    public class FramesGroup
    {
        public ISurface Texture { get; set; }

        public IEnumerable<Frame> Frames { get; set; }
    }
}
