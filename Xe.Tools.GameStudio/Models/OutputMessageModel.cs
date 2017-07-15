using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Xe.Tools.GameStudio.Models
{
    public class OutputMessageModel
    {
        public Log.Level Level { get; private set; }
        public DateTime Time { get; private set; }
        public string StrTime { get => Time.ToString("HH:mm:ss.fff"); }
        public string Description { get; private set; }
        public string Module => string.Empty;
        public System.Windows.Media.DrawingImage Image { get; private set; }

        public OutputMessageModel(Log.Level type, string description)
        {
            Level = type;
            Time = DateTime.Now;
            Description = description;

            string strResourceName;
            switch (Level)
            {
                case Log.Level.Error:
                    strResourceName = "StatusCriticalError_16x";
                    break;
                case Log.Level.Warning:
                    strResourceName = "StatusWarning_16x";
                    break;
                case Log.Level.Message:
                    strResourceName = "StatusInformation_16x";
                    break;
                default:
                    strResourceName = null;
                    break;
            }
            Image = Application.Current.Resources[strResourceName] as
                System.Windows.Media.DrawingImage;
        }
    }
}
