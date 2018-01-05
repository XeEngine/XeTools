using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Xe.Tools.Wpf.Controls
{
    public abstract class WindowEx : Window
    {
        public static readonly DependencyProperty AskExitConfirmationProperty =
            DependencyProperty.Register(
                "AskExitConfirmation",
                typeof(bool),
                typeof(WindowEx),
                new PropertyMetadata(false, new PropertyChangedCallback(OnAskExitConfirmationPropertyChanged)),
                new ValidateValueCallback(ValidateBoolean));

        public bool AskExitConfirmation
        {
            get => (bool)GetValue(AskExitConfirmationProperty);
            set => SetValue(AskExitConfirmationProperty, value);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (AskExitConfirmation)
            {
                e.Cancel = !DoAskExitConfirmation().HasValue;
            }
            base.OnClosing(e);
        }

        protected virtual bool? DoAskExitConfirmation()
        {
            const string Message = "There are pending changes. Do you want to save them?";
            const string Title = "Save confirmation";
            const MessageBoxButton Buttons = MessageBoxButton.YesNoCancel;
            const MessageBoxImage Icon = MessageBoxImage.Warning;

            bool? result;
            switch (MessageBox.Show(Message, Title, Buttons, Icon))
            {
                case MessageBoxResult.None:
                    result = null;
                    break;
                case MessageBoxResult.OK:
                    result = true;
                    break;
                case MessageBoxResult.Cancel:
                    result = null;
                    break;
                case MessageBoxResult.Yes:
                    result = true;
                    break;
                case MessageBoxResult.No:
                    result = false;
                    break;
                default:
                    result = null;
                    break;
            }

            if (result == true)
            {
                if (!DoSaveChanges())
                {
                    const string SaveErrMessage = "There was an error during saving.";
                    const string SaveErrTitle = "Save error";
                    const MessageBoxButton SaveErrButtons = MessageBoxButton.OK;
                    const MessageBoxImage SaveErrIcon = MessageBoxImage.Error;
                    MessageBox.Show(SaveErrMessage, SaveErrTitle, SaveErrButtons, SaveErrIcon);
                    result = null;
                }
            }
            return result;
        }

        protected abstract bool DoSaveChanges();

        private static void OnAskExitConfirmationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(d as WindowEx)
        }
        private static bool ValidateBoolean(object o)
        {
            return o is bool;
        }
    }
}
