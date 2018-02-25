using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Ertis.Shared.ModalWindow.Views
{
    /// <summary>
    /// Interaction logic for WaitProgressDialogControl.xaml
    /// </summary>
    partial class WaitProgressDialogControl : UserControl, INotifyPropertyChanged
    {
        internal WaitProgressDialogControl(bool showWaitAnimation)
        {
            AnimationVisibility = showWaitAnimation ? Visibility.Visible : Visibility.Collapsed;
            ProgressVisibility = !showWaitAnimation ? Visibility.Visible : Visibility.Collapsed;

            InitializeComponent();
        }

        private Visibility _animationVisibility;
        public Visibility AnimationVisibility
        {
            get { return _animationVisibility; }
            private set
            {
                _animationVisibility = value;
                OnPropertyChanged("AnimationVisibility");
            }
        }

        private Visibility _progressVisibility;
        public Visibility ProgressVisibility
        {
            get { return _progressVisibility; }
            private set
            {
                _progressVisibility = value;
                OnPropertyChanged("ProgressVisibility");
            }
        }

        private string _displayText;
        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                _displayText = "Please wait... (" + value + ")";
                _displayText = value;
                OnPropertyChanged("DisplayText");
            }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                if (_progress < 0)
                    _progress = 0;
                else if (_progress > 100)
                    _progress = 100;

                OnPropertyChanged("Progress");
            }
        }

        public void Finish()
        {
            AnimationVisibility = Visibility.Collapsed;
            ProgressVisibility = Visibility.Collapsed;
        }

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
