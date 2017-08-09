using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xe.Tools.Wpf
{
    public class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
