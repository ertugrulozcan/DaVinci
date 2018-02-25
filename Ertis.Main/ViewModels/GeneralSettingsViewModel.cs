using Ertis.Localization.Services;
using Ertis.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace Ertis.Main.ViewModels
{
    public class GeneralSettingsViewModel : BaseViewModel
    {
        private static bool IS_FIRST_NAVIGATE = true;

        #region Services

        private readonly ILocalizationService localizationService;

        #endregion

        #region Dependency Properties

        public CultureInfo CurrentLocCulture
        {
            get { return (CultureInfo)GetValue(CurrentLocCultureProperty); }
            set { SetValue(CurrentLocCultureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentLocCulture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentLocCultureProperty =
            DependencyProperty.Register("CurrentLocCulture", typeof(CultureInfo), typeof(GeneralSettingsViewModel), new PropertyMetadata(null, OnCurrentLocCultureChangedCallback));

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public GeneralSettingsViewModel(ILocalizationService localizationService) : base(Guid.NewGuid().ToString())
        {
            this.localizationService = localizationService;

            if (IS_FIRST_NAVIGATE)
            {
                // remove invariant culture
                LocalizeDictionary.Instance.MergedAvailableCultures.RemoveAt(0);

                IS_FIRST_NAVIGATE = false;
            }

            // Set current language
            this.CurrentLocCulture = LocalizeDictionary.Instance.Culture;
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

        #region Callback Methods

        private static void OnCurrentLocCultureChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as GeneralSettingsViewModel;
            if (self.CurrentLocCulture != null && LocalizeDictionary.Instance.Culture.NativeName != self.CurrentLocCulture.NativeName)
                self.localizationService.ChangeUICulture(self.CurrentLocCulture.TwoLetterISOLanguageName);
        }

        #endregion
    }
}
