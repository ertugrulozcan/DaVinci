using Ertis.DaVinci.HtmlModels;
using Ertis.DaVinci.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ertis.DaVinci.Views
{
    /// <summary>
    /// Interaction logic for AddSectionView.xaml
    /// </summary>
    public partial class AddSectionView : UserControl
    {
        private AddSectionViewModel viewModel;

        public AddSectionView(AddSectionViewModel viewModel)
        {
            try
            {
                this.DataContext = this.viewModel = viewModel;
                InitializeComponent();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void SectionTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedSection = (sender as RadioButton).DataContext as Section;
            this.viewModel.SelectedSection = selectedSection;
        }
    }
}
