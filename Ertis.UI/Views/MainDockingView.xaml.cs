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

namespace Ertis.Shared.Views
{
    /// <summary>
    /// Interaction logic for MainDockingView.xaml
    /// </summary>
    public partial class MainDockingView : UserControl
    {
        public MainDockingView()
        {
            InitializeComponent();

            this.Loaded += MainDockingView_Loaded;
        }

        private void MainDockingView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
