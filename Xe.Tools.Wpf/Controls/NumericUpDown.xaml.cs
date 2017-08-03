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
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(int),
                typeof(NumericUpDown),
                new PropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)),
                new ValidateValueCallback(ValidateValue));
        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(
                "MinimumValue",
                typeof(int),
                typeof(NumericUpDown),
                new PropertyMetadata(0, new PropertyChangedCallback(OnMinimumValuePropertyChanged)),
                new ValidateValueCallback(ValidateValue));
        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(
                "MaximumValue",
                typeof(int),
                typeof(NumericUpDown),
                new PropertyMetadata(100, new PropertyChangedCallback(OnMaximumValuePropertyChanged)),
                new ValidateValueCallback(ValidateValue));

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public int MinimumValue
        {
            get => (int)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }
        public int MaximumValue
        {
            get => (int)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        public NumericUpDown()
        {
            InitializeComponent();
            TextNumber.DataContext = this;
        }
        
        private void CheckBoundaries(int value, int minimumValue, int maximumValue)
        {
            var minReached = value != minimumValue;
            var maxReached = value != maximumValue;
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
            int value;
            switch (e.Key)
            {
                case Key.Up:
                    value = Value;
                    if (value < MaximumValue)
                        Value = value + 1;
                    break;
                case Key.Down:
                    value = Value;
                    if (value > MinimumValue)
                        Value = value - 1;
                    break;
            }
        }

        private void TextNumber_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            int value;
            int move = e.Delta / 120;
            if (move > 0)
            {
                value = Value;
                if (value < MaximumValue)
                    Value = value + 1;
            }
            else if (move < 0)
            {
                value = Value;
                if (value > MinimumValue)
                    Value = value - 1;
            }
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

        private void OnValueChanged(int oldValue, int newValue)
        {
            var minimumValue = MinimumValue;
            var maximumValue = MaximumValue;
            int realValue = Math.Max(minimumValue, Math.Min(maximumValue, newValue));
            if (realValue != newValue)
            {
                Value = realValue;
            }
            else if (newValue != oldValue)
            {
                TextNumber.TextChanged -= TextBox_TextChanged;
                TextNumber.Text = newValue.ToString();
                TextNumber.TextChanged += TextBox_TextChanged;
                CheckBoundaries(newValue, minimumValue, maximumValue);
            }
        }
        private void OnMinimumValueChanged(int oldValue, int newValue)
        {
            var minimumValue = newValue;
            var maximumValue = MaximumValue;
            var value = Value;
            
            if (minimumValue > maximumValue)
                minimumValue = maximumValue;
            if (minimumValue != oldValue)
            {
                MinimumValue = minimumValue;
                if (Value < minimumValue)
                    Value = minimumValue;
                else
                    CheckBoundaries(value, minimumValue, maximumValue);
            }
        }
        private void OnMaximumValueChanged(int oldValue, int newValue)
        {
            var minimumValue = MinimumValue;
            var maximumValue = newValue;
            var value = Value;

            if (maximumValue < minimumValue)
                maximumValue = minimumValue;
            if (maximumValue != oldValue)
            {
                MaximumValue = maximumValue;
                if (Value > maximumValue)
                    Value = maximumValue;
                else
                    CheckBoundaries(value, minimumValue, maximumValue);
            }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericUpDown).OnValueChanged((int)e.OldValue, (int)e.NewValue);
        }
        private static void OnMinimumValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericUpDown).OnMinimumValueChanged((int)e.OldValue, (int)e.NewValue);
        }
        private static void OnMaximumValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericUpDown).OnMaximumValueChanged((int)e.OldValue, (int)e.NewValue);
        }
        private static bool ValidateValue(object o)
        {
            return o is int;
        }
    }
}
