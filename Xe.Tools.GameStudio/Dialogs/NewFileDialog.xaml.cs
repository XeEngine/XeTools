using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Xe.Tools.Components;
using Xe.Tools.GameStudio.Utility;

namespace Xe.Tools.GameStudio.Dialogs
{
    /// <summary>
    /// Interaction logic for NewFileDialog.xaml
    /// </summary>
    public partial class NewFileDialog : Window
    {
        private ComponentInfo _componentInfo;
        private ComponentInfo ComponentInfo
        {
            get => _componentInfo;
            set
            {
                _componentInfo = value;
                SetTextValue(value.ModuleName, labelModule, textModule);
                SetTextValue(value.Editor, labelEditor, textEditor);
                SetTextValue(value.Description, labelDescription, textDescription);
            }
        }

        public IEnumerable<Component> Components { get; set; }

        public Component SelectedComponent { get; private set; }

        public string FileName
        {
            get => textFileName.Text;
            set => textFileName.Text = value;
        }


        public NewFileDialog()
        {
            InitializeComponent();

            Components = Globals.Components;
            ComponentInfo = new ComponentInfo();
            PopulateComponentList();
        }

        private void PopulateComponentList()
        {
            foreach (var component in Components)
            {
                listComponents.Items.Add(component);
            }
        }

        private void listComponents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            SelectedComponent = listBox.SelectedItem as Component;
            ComponentInfo = SelectedComponent.ComponentInfo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileName = textFileName.Text;
            DialogResult = true;
            Close();
        }

        private void SetTextValue(string value, TextBlock label, TextBlock text)
        {
            if (!string.IsNullOrEmpty(value))
            {
                label.Visibility = Visibility.Visible;
                text.Visibility = Visibility.Visible;
                text.Text = value;
            }
            else
            {
                label.Visibility = Visibility.Hidden;
                text.Visibility = Visibility.Hidden;
            }
        }
    }
}
