using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.GameStudio.Models
{
    public class WindowPropertiesModel
    {
        const int MIN_WIDTH = 128;
        const int DEF_WIDTH = 640;
        const int MIN_HEIGHT = 160;
        const int DEF_HEIGHT = 480;

        private int _width;
        private int _height;

        public int Width
        {
            get => _width >= MIN_WIDTH ? _width : DEF_WIDTH;
            set
            {
                _width = value;
                Save();
            }
        }

        public int Height
        {
            get => _height >= MIN_HEIGHT ? _height : DEF_HEIGHT;
            set
            {
                _height = value;
                Save();
            }
        }

        private void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
