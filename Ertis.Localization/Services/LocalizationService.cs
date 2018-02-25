using Ertis.Infrastructure.Events;
using Ertis.Localization.Events;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace Ertis.Localization.Services
{
    public class LocalizationService : ILocalizationService
    {
        #region Services

        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Fields

        private string currentLocaleCode;

        #endregion

        #region Properties

        public System.Windows.FlowDirection UiFlowDirection { get; private set; }

        public string CurrentLocaleCode
        {
            get
            {
                return currentLocaleCode;
            }

            private set
            {
                string oldCultureCode = this.currentLocaleCode;
                string newCultureCode = value;

                this.currentLocaleCode = value;
                this.UiFlowDirection = this.GetFlowDirection(newCultureCode);

                if (this.IsNeedToChangeUiFlowDirection(oldCultureCode, newCultureCode))
                {
                    this.eventAggregator.GetEvent<FlowDirectionChangeRequestEvent>().Publish(this.UiFlowDirection);
                }
            }
        }

        #endregion

        #region Consructors

        /// <summary>
        /// Consructor
        /// </summary>
        public LocalizationService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        #endregion

        #region Methods

        public void UpdateTranslationResources(string cultureCode)
        {
            this.UpdateTranslationResources(cultureCode, new HashSet<string>());
        }

        public void UpdateTranslationResources(string cultureCode, HashSet<string> ignoredLocalizations)
        {
            var cvsProvider = Ertis.Localization.ErtisLocalizationProvider.Instance;
            
            cvsProvider.FileName = "Strings";
            cvsProvider.HasHeader = true;
            cvsProvider.IgnoredLocalizations = ignoredLocalizations;
            cvsProvider.UpdateAvailableCultures();

            LocalizeDictionary.Instance.DefaultProvider = cvsProvider;

            var culture = this.ChangeUICulture(cultureCode);
            LocalizeDictionary.Instance.Culture = culture;
            //LocalizeDictionary.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        public CultureInfo ChangeUICulture(string cultureCode)
        {
            string cultureStr = (String.IsNullOrEmpty(cultureCode) ? "en" : cultureCode);
            var culture = System.Globalization.CultureInfo.GetCultureInfo(cultureStr);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            LocalizeDictionary.Instance.Culture = culture;
            this.CurrentLocaleCode = cultureCode;

            this.eventAggregator.GetEvent<LanguageChangedEvent>().Publish(culture);
            
            return culture;
        }

        private bool IsNeedToChangeUiFlowDirection(string oldCultureCode, string newCultureCode)
        {
            var oldFlowDirection = this.GetFlowDirection(oldCultureCode);
            var newFlowDirection = this.GetFlowDirection(newCultureCode);

            return oldFlowDirection != newFlowDirection;
        }

        private System.Windows.FlowDirection GetFlowDirection(string cultureCode)
        {
            if (cultureCode == null)
                return System.Windows.FlowDirection.LeftToRight;

            if (cultureCode == "ar" || cultureCode == "ar-sa")
                return System.Windows.FlowDirection.RightToLeft;
            else
                return System.Windows.FlowDirection.LeftToRight;
        }

        #endregion

        #region Event Handlers

        /*
        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Culture")
            {
                var locale = LocalizeDictionary.Instance.Culture.TwoLetterISOLanguageName;
                this.ChangeUICulture(locale);
            }
        }
        */

        #endregion
    }
}
