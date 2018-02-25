using Ertis.Main.ViewModels;
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

namespace Ertis.Main.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel viewModel;

        public SettingsView(SettingsViewModel viewModel)
        {
            this.DataContext = this.viewModel = viewModel;
            InitializeComponent();


            this.Loaded += SettingsView_Loaded;
        }

        private void SettingsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.viewModel.SettingsVmiCollection != null && this.viewModel.SettingsVmiCollection.Count > 0)
            {
                this.viewModel.SelectedSettingsVMI = this.viewModel.SettingsVmiCollection.First();
            }
        }
    }
}
