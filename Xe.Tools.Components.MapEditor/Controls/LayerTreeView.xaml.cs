using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xe.Tools.Components.MapEditor.ViewModels;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for LayerTreeView.xaml
    /// </summary>
    public partial class LayerTreeView : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty MasterNodeProperty =
            DependencyProperty.Register(
                "MasterNode",
                typeof(NodeMapViewModel),
                typeof(LayerTreeView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnMasterNodePropertyChanged)),
                new ValidateValueCallback(ValidateMasterNode));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(LayerTreeView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemPropertyChanged)),
                new ValidateValueCallback(ValidateSelectedItemNode));

        public event PropertyChangedEventHandler PropertyChanged;

        public NodeMapViewModel MasterNode
        {
            get => GetValue(MasterNodeProperty) as NodeMapViewModel;
            set => SetValue(MasterNodeProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value); // muda muda muda
        }

        public LayerTreeView()
        {
            InitializeComponent();
        }

        private static void OnMasterNodePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = e.NewValue as NodeMapViewModel;
            var treeView = (d as LayerTreeView).TreeView;
            if (value != null)
            {
                var list = new List<NodeBaseViewModel>() { value };
                list.AddRange(value.Childs);
                treeView.ItemsSource = new ObservableCollection<NodeBaseViewModel>(list);
            }
            else
                treeView.ItemsSource = null;
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static bool ValidateMasterNode(object o)
        {
            if (o == null) return true;
            return o is NodeMapViewModel;
        }

        private static bool ValidateSelectedItemNode(object o)
        {
            return true;
        }

        private void TreeProject_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
