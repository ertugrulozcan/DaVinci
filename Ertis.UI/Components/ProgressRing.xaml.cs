﻿using System;
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

namespace Ertis.Shared.Components
{
    /// <summary>
    /// Interaction logic for ProgressRing.xaml
    /// </summary>
    public partial class ProgressRing : UserControl
    {
        public ProgressRing()
        {
            InitializeComponent();
        }

        private void WaitSpinner_Loaded(object sender, RoutedEventArgs e)
        {
            var spinner = (sender as Image);
            var indicatorSB = spinner.FindResource("WaitStoryboard") as System.Windows.Media.Animation.Storyboard;

            indicatorSB.Begin();
        }
    }
}