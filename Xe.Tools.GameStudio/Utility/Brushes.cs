using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Xe.Tools.GameStudio.Utility
{
    public static class Brushes
    {
        public static Brush White = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public static Brush Black = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        public static Brush Transparent = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
    }
}
