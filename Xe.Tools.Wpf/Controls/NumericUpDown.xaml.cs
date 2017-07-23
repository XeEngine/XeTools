using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Xe.Tools.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        private int _minValue = 0,
           _maxValue = 100,
           _value = 10;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Value
        {
            get => _value;
            set
            {
                int newValue = Math.Max(_minValue, Math.Min(_maxValue, value));
                if (_value != newValue)
                {
                    _value = newValue;
                    OnPropertyChanged();
                    CheckBoundaries();
                }
            }
        }
        public int MinimumValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                if (_minValue > _maxValue)
                    _minValue = _maxValue;
                if (Value < _minValue)
                    Value = _minValue;
                CheckBoundaries();
            }
        }
        public int MaximumValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                if (_maxValue < _minValue)
                    _maxValue = _minValue;
                if (Value > _maxValue)
                    Value = _maxValue;
                CheckBoundaries();
            }
        }

        public NumericUpDown()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CheckBoundaries()
        {
            var minReached = _value != MinimumValue;
            var maxReached = _value != MaximumValue;
            if (ButtonDown.IsEnabled != minReached)
                ButtonDown.IsEnabled = minReached;
            if (ButtonUp.IsEnabled != maxReached)
                ButtonUp.IsEnabled = maxReached;
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            Value--;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //ButtonBase button;
            switch (e.Key)
            {
                case Key.Up:
                    Value++;
                    break;
                case Key.Down:
                    Value--;
                    break;
                /*default:
                    button = null;
                    break;*/
            }
            /*if (button != null)
            {
                button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                typeof(ButtonBase).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(button, new object[] { true });
            }*/
        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //    if (e.Key == Key.Up)
            //        typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ButtonUp, new object[] { false });

            //    if (e.Key == Key.Down)
            //        typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(ButtonDown, new object[] { false });
        }

        private void TextNumber_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            int move = e.Delta / 120;
            Value += move;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var str = TextNumber.Text;
            if (int.TryParse(str, out int value))
            {
                Value = value;
                TextNumber.SelectionStart = str.Length;
            }

        }
    }
}
