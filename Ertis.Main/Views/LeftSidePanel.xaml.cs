using Ertis.Infrastructure.Events;
using Ertis.Main.ViewModels;
using Ertis.Shared.Models;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ertis.Main.Views
{
    /// <summary>
    /// Interaction logic for LeftSidePanel.xaml
    /// </summary>
    public partial class LeftSidePanel : UserControl
    {
        private readonly IEventAggregator eventAggregator;

        private Storyboard SubMenuExpandCollapseSB;

        private DoubleAnimationUsingKeyFrames SubMenuExpandAnimation;
        private DoubleAnimationUsingKeyFrames SubMenuCollapseAnimation;

        public LeftSidePanel(LeftSidePanelViewModel viewModel, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.DataContext = viewModel;
            InitializeComponent();

            this.Loaded += LeftSidePanel_Loaded;
        }

        private void InitializeSubMenuAnimations()
        {
            this.SubMenuExpandCollapseSB = new Storyboard();
            this.SubMenuExpandCollapseSB.Duration = TimeSpan.FromMilliseconds(300);
            this.SubMenuExpandCollapseSB.FillBehavior = FillBehavior.HoldEnd;

            // Expand
            this.SubMenuExpandAnimation = new DoubleAnimationUsingKeyFrames();
            this.SubMenuExpandAnimation.KeyFrames = new DoubleKeyFrameCollection()
            {
                new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), new CubicEase()),
                new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300)), new CubicEase()),
            };

            // Collapse
            this.SubMenuCollapseAnimation = new DoubleAnimationUsingKeyFrames();
            this.SubMenuCollapseAnimation.KeyFrames = new DoubleKeyFrameCollection()
            {
                new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), new CubicEase()),
                new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300)), new CubicEase()),
            };
        }

        private void LeftSidePanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitializeSubMenuAnimations();
            this.eventAggregator.GetEvent<LeftSidePanelLoadedEvent>().Publish(new LeftSidePanelLoadedEvent());
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IViewMenuItem collapsedVMI = null;
            IViewMenuItem expandedVMI = null;

            if (e.RemovedItems != null && e.RemovedItems.Count > 0 && e.RemovedItems[0] is IViewMenuItem)
                collapsedVMI = e.RemovedItems[0] as IViewMenuItem;

            if (e.AddedItems != null && e.AddedItems.Count > 0 && e.AddedItems[0] is IViewMenuItem)
                expandedVMI = e.AddedItems[0] as IViewMenuItem;

            this.BeginMenuAnimations(collapsedVMI, expandedVMI);
        }

        private void BeginMenuAnimations(IViewMenuItem collapsedVMI, IViewMenuItem expandedVMI)
        {
            if (collapsedVMI != null)
            {
                var listBoxItem = this.MenuListBox.ItemContainerGenerator.ContainerFromItem(collapsedVMI) as ListBoxItem;
                var subMenuBorder = (listBoxItem.Template as ControlTemplate).FindName("SubMenuBorder", listBoxItem) as Border;
                Storyboard.SetTargetProperty(this.SubMenuCollapseAnimation, new PropertyPath(Border.HeightProperty));
                Storyboard.SetTarget(this.SubMenuCollapseAnimation, subMenuBorder);

                this.SubMenuCollapseAnimation.KeyFrames[0].Value = subMenuBorder.ActualHeight;
                this.SubMenuExpandCollapseSB.Children.Add(this.SubMenuCollapseAnimation);
            }
            else
            {
                this.SubMenuExpandCollapseSB.Children.Remove(this.SubMenuCollapseAnimation);
            }

            if (expandedVMI != null)
            {
                var listBoxItem = this.MenuListBox.ItemContainerGenerator.ContainerFromItem(expandedVMI) as ListBoxItem;
                var subMenuBorder = (listBoxItem.Template as ControlTemplate).FindName("SubMenuBorder", listBoxItem) as Border;
                Storyboard.SetTargetProperty(this.SubMenuExpandAnimation, new PropertyPath(Border.HeightProperty));
                Storyboard.SetTarget(this.SubMenuExpandAnimation, subMenuBorder);

                this.SubMenuExpandAnimation.KeyFrames[1].Value = expandedVMI.Children.Count * 30 + 4;
                this.SubMenuExpandCollapseSB.Children.Add(this.SubMenuExpandAnimation);
            }
            else
            {
                this.SubMenuExpandCollapseSB.Children.Remove(this.SubMenuExpandAnimation);
            }
            
            this.SubMenuExpandCollapseSB.Begin();
        }
    }
}
