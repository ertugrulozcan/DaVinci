using Ertis.DaVinci.ViewModels;
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

namespace Ertis.DaVinci.Views
{
    /// <summary>
    /// Interaction logic for SolutionExplorerView.xaml
    /// </summary>
    public partial class SolutionExplorerView : UserControl
    {
        public SolutionExplorerView(SolutionExplorerViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            var treeView = sender as TreeView;
            foreach (var item in treeView.Items)
            {
                var treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                treeViewItem.IsExpanded = true;
            }
        }
    }
}
