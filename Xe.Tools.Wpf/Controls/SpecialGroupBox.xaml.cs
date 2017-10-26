using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for SpecialGroupBox.xaml
    /// </summary>
    public partial class SpecialGroupBox : HeaderedContentControl
    {
        public static readonly DependencyProperty IsContentVisibleProperty = DependencyProperty.Register(
            nameof(IsContentVisible),
            typeof(bool),
            typeof(SpecialGroupBox),
            new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnContentVisibleChanged)));

        [Bindable(true)]
        [Category("Content")]
        [Localizability(LocalizationCategory.Label)]
        public bool IsContentVisible
        {
            get => (bool)GetValue(IsContentVisibleProperty);
            set => SetValue(IsContentVisibleProperty, value);
        }

        public SpecialGroupBox()
        {
            InitializeComponent();
        }

        private void SetContentVisibility(bool visible)
        {
            if (Template.FindName("CtrlContent", this) is ContentPresenter ctrlContent)
                ctrlContent.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            if (Template.FindName("ButtonShow", this) is Button buttonShow)
                buttonShow.Visibility = visible ? Visibility.Collapsed : Visibility.Visible;
            if (Template.FindName("ButtonHide", this) is Button buttonHide)
                buttonHide.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void OnContentVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SpecialGroupBox)d;
            control?.SetContentVisibility((bool?)e.NewValue ?? true);

        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            IsContentVisible = !IsContentVisible;
        }

        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            IsContentVisible = !IsContentVisible;
        }
    }
}
