using Ertis.Main.ViewModels;
using Ertis.Shared.Events;
using Microsoft.Practices.Prism.Events;
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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private readonly IEventAggregator eventAggregator;

        public MainView(MainViewModel viewModel, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.DataContext = viewModel;
            InitializeComponent();

            this.Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            this.eventAggregator.GetEvent<MainViewLoadedEvent>().Publish(new MainViewLoadedEvent());
            this.Loaded -= MainView_Loaded;
        }
    }
}
