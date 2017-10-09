using System.Windows;
using System.Windows.Media;

namespace Xe.Tools.Wpf.Controls
{
    public class XeWindow : Window
    {
        new Brush Background
        {
            get
            {
                return new SolidColorBrush(Color.FromRgb(0x2d, 0x2d, 0x30));
            }
            set
            {

            }
        }
    }
}
