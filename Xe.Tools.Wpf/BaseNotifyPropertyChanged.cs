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

        public void OnAllPropertiesChanged()
        {
            foreach (var property in GetType().GetProperties())
            {
                OnPropertyChanged(property.Name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
