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
using System.Windows.Shapes;
using Xe.Tools.Components.AnimationEditor.ViewModels;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowMapping.xaml
    /// </summary>
    public partial class WindowMapping : Window
    {
        private AnimationsMappingViewModel ViewModel => DataContext as AnimationsMappingViewModel;

        public WindowMapping()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AnimationDefs.Add(new Game.Animations.AnimationDefinition()
            {
                Name = "<new animation>"
            });
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsAnimationDefSelected)
            {
                ViewModel.AnimationDefs.Remove(ViewModel.SelectedAnimationDef);
            }
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dialog = new SingleInputDialog()
            {
                Text = ViewModel.SelectedAnimationDef.Name,
                Description = "Insert the animation name"
            };

            if (dialog.ShowDialog() == true)
            {
                ViewModel.ChangeAnimationDefinitionName(dialog.Text);
            }
        }
    }
}
