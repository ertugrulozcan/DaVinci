using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using Ertis.Shared.Services.Contracts;
using Microsoft.Practices.ServiceLocation;

namespace Ertis.Shared.ViewModels
{
    public abstract class BaseViewModel : DependencyObject, INotifyPropertyChanged, IMVVMDockingProperties, INavigationAware, IDisposable
    {
        #region Services

        private readonly IWindowNavigationService windowNavigationService;

        #endregion

        #region Fields

        private string targetName;
        private string viewCaption;

        #endregion

        #region Properties

        public string TargetName
        {
            get
            {
                return this.targetName;
            }

            set
            {
                this.targetName = value;
            }
        }

        #endregion

        #region Dependency Properties

        public string ViewCaption
        {
            get { return (string)GetValue(ViewCaptionProperty); }
            set { SetValue(ViewCaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewCaption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewCaptionProperty = DependencyProperty.Register("ViewCaption", typeof(string), typeof(BaseViewModel), new PropertyMetadata("", OnViewCaptionChangedCallback));

        private static void OnViewCaptionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as BaseViewModel;
            if (e.NewValue != null && self.ViewCaption != e.NewValue.ToString())
                self.ViewCaption = e.NewValue == null ? null : e.NewValue.ToString();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseViewModel(string guid)
        {
            this.TargetName = guid;

            this.windowNavigationService = ServiceLocator.Current.GetInstance<IWindowNavigationService>();
        }

        #endregion

        #region Navigation Methods

        protected abstract void OnNavigatedTo(object parameter);
        protected abstract void OnNavigatedFrom();
        
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            string viewName = navigationContext.Uri.OriginalString;
            object parameter = this.windowNavigationService.GetNavigationParameter(viewName);
            this.OnNavigatedTo(parameter);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.OnNavigatedFrom();
        }

        public void ForceFireNavigateEvent(object parameter)
        {
            this.OnNavigatedTo(parameter);
        }
        
        #endregion

        #region RaisePropertyChanged

        /// <summary>
        /// Helper to set dependency property value.
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="target">Target Dependency property</param>
        /// <param name="value">Value to set</param>
        /// <param name="changedProperties">argument list on changed property names we going notify about notify</param>
        /// <returns></returns>
        protected virtual bool SetValue<T>(ref T target, T value, params string[] changedProperties)
        {
            if (Object.Equals(target, value))
            {
                return false; // no changes, same value
            }

            target = value;

            foreach (string property in changedProperties)
            {
                RaisePropertyChanged(property);
            }

            return true;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Dispose

        public void Dispose()
        {

        }
        
        #endregion
    }
}
