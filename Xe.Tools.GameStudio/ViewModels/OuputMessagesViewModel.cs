using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xe.Tools.GameStudio.Models;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class OuputMessagesViewModel : INotifyPropertyChanged
    {
        private int _logErrCount;
        private int _logWarnCount;
        private int _logMsgCount;
        private bool _isLogErrVisible = true;
        private bool _isLogWarnVisible = true;
        private bool _isLogMsgVisible = true;
        private List<OutputMessageModel> _logList = new List<OutputMessageModel>();
        private ObservableCollection<OutputMessageModel> _logObsList;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ErrorsCount
        {
            get => _logErrCount;
            private set
            {
                _logErrCount = value;
                OnPropertyChanged();
            }
        }
        public int WarningsCount
        {
            get => _logWarnCount;
            private set
            {
                _logWarnCount = value;
                OnPropertyChanged();
            }
        }
        public int MessagesCount
        {
            get => _logMsgCount;
            private set
            {
                _logMsgCount = value;
                OnPropertyChanged();
            }
        }
        public bool IsErrorsVisible
        {
            get => _isLogErrVisible;
            set
            {
                if (_isLogErrVisible != value)
                {
                    _isLogErrVisible = value;
                    OnPropertyChanged();
                    OnFilterChanged();
                }
            }
        }
        public bool IsWarningsVisible
        {
            get => _isLogWarnVisible;
            set
            {
                if (_isLogWarnVisible != value)
                {
                    _isLogWarnVisible = value;
                    OnPropertyChanged();
                    OnFilterChanged();
                }
            }
        }
        public bool IsMessagesVisible
        {
            get => _isLogMsgVisible;
            set
            {
                if (_isLogMsgVisible != value)
                {
                    _isLogMsgVisible = value;
                    OnPropertyChanged();
                    OnFilterChanged();
                }
            }
        }

        public ObservableCollection<OutputMessageModel> Log
        {
            get => _logObsList;
            private set
            {
                _logObsList = value;
                OnPropertyChanged();
            }
        }

        public OuputMessagesViewModel()
        {
            Log = new ObservableCollection<OutputMessageModel>();
            Xe.Log.OnLog += Log_OnLog;
        }

        public void Clear()
        {
            _logList.Clear();
            Log.Clear();
            ErrorsCount = 0;
            WarningsCount = 0;
            MessagesCount = 0;
        }

        private void OnFilterChanged()
        {
            IEnumerable<OutputMessageModel> result = _logList.ToList();
            if (!IsErrorsVisible)
                result = result.Where(x => x.Level != Xe.Log.Level.Error);
            if (!IsWarningsVisible)
                result = result.Where(x => x.Level != Xe.Log.Level.Warning);
            if (!IsMessagesVisible)
                result = result.Where(x => x.Level != Xe.Log.Level.Message);
            Log = new ObservableCollection<OutputMessageModel>(result);
        }

        private void Log_OnLog(Log.Level level, string message, string member, string sourceFile, int sourceLine)
        {
            var item = new OutputMessageModel(level, message);
            _logList.Add(item);
            bool canLog = false;
            switch (level)
            {
                case Xe.Log.Level.Error:
                    ErrorsCount++;
                    if (IsErrorsVisible)
                        canLog = true;
                    break;
                case Xe.Log.Level.Warning:
                    WarningsCount++;
                    if (IsWarningsVisible)
                        canLog = true;
                    break;
                case Xe.Log.Level.Message:
                    MessagesCount++;
                    if (IsMessagesVisible)
                        canLog = true;
                    break;
            }
            if (canLog)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((() =>
                {
                    Log.Add(item);
                }));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
