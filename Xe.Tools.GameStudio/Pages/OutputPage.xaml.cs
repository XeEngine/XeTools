using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Xe.Tools.GameStudio.ViewModels;

namespace Xe.Tools.GameStudio.Pages
{
    /// <summary>
    /// Interaction logic for OutputPage.xaml
    /// </summary>
    public partial class OutputPage : Page
    {
        private OuputMessagesViewModel model;

        public OutputPage()
        {
            InitializeComponent();

            model = new OuputMessagesViewModel();
            DataContext = model;
            ErrorsList.DataContext = model;
        }
    }

    public abstract class GenericCountConverter : IValueConverter
    {
        public abstract string Name { get; }

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return $"{value} {Name} ";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class ErrorCountConverter : GenericCountConverter
    {
        public override string Name => "Errors";
    }
    public class WarningCountConverter : GenericCountConverter
    {
        public override string Name => "Warnings";
    }
    public class MessageCountConverter : GenericCountConverter
    {
        public override string Name => "Messages";
    }
}
