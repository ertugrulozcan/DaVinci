using Ertis.Shared.ViewModels;
using Ertis.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Main.ViewModels
{
    public class AppearenceSettingsViewModel : BaseViewModel
    {
        #region Fields

        private Theme selectedTheme;

        #endregion

        #region Properties

        public List<Theme> ThemeList
        {
            get
            {
                return ThemeManager.Current.Themes;
            }
        }

        public Theme SelectedTheme
        {
            get
            {
                return selectedTheme;
            }

            set
            {
                this.selectedTheme = value;
                this.RaisePropertyChanged("SelectedTheme");

                if (this.SelectedTheme != null)
                    Themes.ThemeManager.Current.ChangeTheme(this.SelectedTheme.Key);
            }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AppearenceSettingsViewModel() : base(Guid.NewGuid().ToString())
        {
            this.SelectedTheme = Themes.ThemeManager.Current.CurrentTheme;
        }

        #endregion

        #region Navigation Methods

        protected override void OnNavigatedTo(object parameter)
        {

        }

        protected override void OnNavigatedFrom()
        {

        }

        #endregion
    }
}
