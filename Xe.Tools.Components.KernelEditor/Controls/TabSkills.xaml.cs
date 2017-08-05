using System;
using System.Collections.Generic;
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
using Xe.Tools.Components.KernelEditor.Dialogs;
using Xe.Tools.Components.KernelEditor.ViewModels;

namespace Xe.Tools.Components.KernelEditor.Controls
{
    /// <summary>
    /// Interaction logic for TabSkills.xaml
    /// </summary>
    public partial class TabSkills : UserControl
    {
        private SkillsViewModel ViewModel => DataContext as SkillsViewModel;

        public SkillViewModel SelectedItem => SkillsList.SelectedItem as SkillViewModel;

        public TabSkills()
        {
            InitializeComponent();
        }
        
        private void SkillsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SkillsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedItem != null)
            {
                var dialog = new NameEditor()
                {
                    ViewModel = new NameViewModel(SelectedItem.Name, SelectedItem.MessageNameId,
                    SelectedItem.MessageDescriptionId, SelectedItem.Messages)
                };
                if (dialog.ShowDialog() == true)
                {
                    SelectedItem.Name = dialog.ViewModel.Id;
                    SelectedItem.MessageNameId = dialog.ViewModel.Name?.Id ?? Guid.NewGuid();
                    SelectedItem.MessageDescriptionId = dialog.ViewModel.Description?.Id ?? Guid.NewGuid();
                }
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Add();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveAt(SkillsList.SelectedIndex);
        }
    }
}
