using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Xe.Tools.Components.MapEditor.Models
{
    public class BitmapFrame
    {
        public BitmapImage Image { get; set; }

        public Point Pivot { get; set; }
    }
}
