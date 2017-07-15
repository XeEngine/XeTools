using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Xe.Tools.GameStudio.Utility;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class StatusViewModel : INotifyPropertyChanged
    {
        private static readonly Color COLOR_INIT = Color.FromArgb(0xFF, 0x68, 0x21, 0x7A);
        private static readonly Color COLOR_IDLE = Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC);
        private static readonly Color COLOR_RUNNING = Color.FromArgb(0xFF, 0xF4, 0x43, 0x36);
        private static readonly Color COLOR_WARNING = Color.FromArgb(0xFF, 0xFF, 0xC1, 0x07);
        private static readonly Color COLOR_ERROR = Color.FromArgb(0xFF, 0xCA, 0x51, 0x00);

        private string _title;
        private ContentControl _content;
        private Brush _brush;
        private Color _color;
        private bool _isProcessing;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public ContentControl Content
        {
            get => _content;
            private set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
        public Brush Background
        {
            get => _brush;
            private set
            {
                _brush = value;
                OnPropertyChanged();
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                Background = new SolidColorBrush(_color);
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                Color = value ? COLOR_RUNNING : COLOR_IDLE;
            }
        }

        public StatusViewModel()
        {
            Color = COLOR_IDLE;
            Title = string.Empty;

            Common.OnMessage += Common_OnMessage; 
        }

        private void Common_OnMessage(Models.MessageModel message)
        {
            Color color;
            switch (message.Type)
            {
                case Models.MessageType.Initialization:
                    color = COLOR_INIT;
                    break;
                case Models.MessageType.Idle:
                    color = COLOR_IDLE;
                    break;
                case Models.MessageType.Processing:
                    color = COLOR_RUNNING;
                    break;
                case Models.MessageType.Warning:
                    color = COLOR_WARNING;
                    break;
                case Models.MessageType.Error:
                    color = COLOR_ERROR;
                    break;
                default:
                    color = COLOR_IDLE;
                    break;
            }
            System.Windows.Application.Current.Dispatcher.Invoke((() =>
            {
                Color = color;
                Title = message.Message;
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
