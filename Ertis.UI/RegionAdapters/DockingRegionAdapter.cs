using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Input;
using Ertis.Shared.ViewModels;
using Ertis.Infrastructure.Application;

namespace Ertis.Shared.RegionAdapters
{
    public class DockingRegionAdapter : RegionAdapterBase<LayoutGroup>
    {
        #region Services

        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        #endregion

        #region Fields & Properties

        public DockLayoutManager LayoutManager { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="behaviorFactory"></param>
        /// <param name="eventAggregator"></param>
        public DockingRegionAdapter(IRegionBehaviorFactory behaviorFactory, IEventAggregator eventAggregator, IRegionManager regionManager) : base(behaviorFactory)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
        }

        #endregion

        #region Methods

        private Point GetLocationCenterPoint(Size panelSize)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            double windowWidth = Application.Current.MainWindow.ActualWidth;
            if (windowWidth < 100)
                windowWidth = screenWidth;

            double windowHeight = Application.Current.MainWindow.ActualHeight;
            if (windowHeight < 100)
                windowHeight = screenHeight;

            if (windowWidth <= 0 || windowWidth > screenWidth)
                windowWidth = screenWidth;

            if (windowHeight <= 0 || windowHeight > screenHeight)
                windowHeight = screenHeight;

            return new Point((windowWidth - panelSize.Width) / 2, (windowHeight - panelSize.Height) / 2 - 100);
        }

        #endregion

        #region Methods.AbstractImplementations

        protected override void Adapt(IRegion region, LayoutGroup rootGroup)
        {
            this.LayoutManager = rootGroup.GetDockLayoutManager();
            this.LayoutManager.DisposeOnWindowClosing = true;
            this.LayoutManager.OwnsFloatWindows = true;
            this.LayoutManager.ClosingBehavior = ClosingBehavior.ImmediatelyRemove;
            this.LayoutManager.FloatingDocumentContainer = DevExpress.Xpf.Docking.Base.FloatingDocumentContainer.DocumentHost;
            this.LayoutManager.DockItemClosed += LayoutManager_DockItemClosed;

            //this.LayoutManager.ClosedPanelsBarVisibility = DevExpress.Xpf.Docking.Base.ClosedPanelsBarVisibility.Auto;
            //this.LayoutManager.ClosedPanelsBarPosition = Dock.Bottom;

            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, s, e);
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        #endregion

        #region Methods.CollectionsControl

        private void LayoutManager_DockItemClosed(object sender, DevExpress.Xpf.Docking.Base.DockItemClosedEventArgs e)
        {
            try
            {
                if (e.Item is LayoutPanel)
                {
                    var layoutPanel = e.Item as LayoutPanel;
                    if (layoutPanel.Content is UserControl)
                    {
                        var view = layoutPanel.Content as UserControl;
                        this.regionManager.Regions[RegionNames.MainDockingRegion].Remove(view);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        void OnViewsCollectionChanged(IRegion region, object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        try
                        {
                            List<UserControl> views = e.NewItems.Cast<UserControl>().ToList();

                            foreach (UserControl view in views)
                            {
                                Size floatSize = new Size(500, 500);

                                LayoutPanel panel = this.LayoutManager.DockController.AddPanel(this.GetLocationCenterPoint(floatSize), floatSize);
                                this.SetPanelOptions(panel, (string)region.Context, view, null);

                                if (view.DataContext != null && view.DataContext is BaseViewModel)
                                    this.SetPanelBindings(panel, view.DataContext as BaseViewModel);

                                if (this.LayoutManager.LayoutRoot.Items.Count == 0)
                                {
                                    this.LayoutManager.DockController.Dock(panel, this.LayoutManager.LayoutRoot, DevExpress.Xpf.Layout.Core.DockType.Fill);
                                }
                                else if (this.LayoutManager.LayoutRoot.Items.Count == 1)
                                {
                                    panel.ItemWidth = new GridLength(300);
                                    this.LayoutManager.DockController.Dock(panel, this.LayoutManager.LayoutRoot, DevExpress.Xpf.Layout.Core.DockType.Right);
                                }
                                else if (this.LayoutManager.LayoutRoot.Items.Count == 2)
                                {
                                    panel.ItemHeight = new GridLength(300);
                                    this.LayoutManager.DockController.Dock(panel, this.LayoutManager.LayoutRoot, DevExpress.Xpf.Layout.Core.DockType.Bottom);
                                }
                                else if (this.LayoutManager.LayoutRoot.Items.Count == 3)
                                {
                                    panel.ItemWidth = new GridLength(300);
                                    this.LayoutManager.DockController.Dock(panel, this.LayoutManager.LayoutRoot, DevExpress.Xpf.Layout.Core.DockType.Left);
                                }
                                else
                                {
                                    this.LayoutManager.BringToFront(panel);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var pane in e.OldItems)
                        {
                            if (pane is UserControl)
                            {
                                UserControl view = pane as UserControl;
                                var mainDockingRegion = this.regionManager.Regions[RegionNames.MainDockingRegion];
                                if (mainDockingRegion.Views.Contains(view))
                                    mainDockingRegion.Remove(view);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {

                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    {

                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    {

                    }
                    break;
            }
        }

        private void SetPanelOptions(LayoutPanel panel, string name, UserControl content, ICommand closeCommand)
        {
            panel.ClosingBehavior = ClosingBehavior.ImmediatelyRemove;
            panel.CloseCommand = closeCommand;

            panel.Content = content;
            panel.Caption = content.GetType().Name;
            panel.AllowMaximize = false;
            panel.ShowMaximizeButton = false;
            panel.Name = name;
            panel.MinWidth = 100;
            panel.MinHeight = 100;
        }

        private void SetPanelBindings(LayoutPanel panel, BaseViewModel vmb)
        {
            /*
            Binding binding = new Binding();

            binding = new Binding();
            binding.Source = vmb;
            binding.Path = new PropertyPath(BaseViewModel.ViewCaptionProperty);
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(panel, LayoutPanel.CaptionProperty, binding);
            */
        }

        #endregion
    }
}
