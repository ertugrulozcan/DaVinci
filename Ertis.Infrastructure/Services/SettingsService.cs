using Ertis.Infrastructure.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ertis.Infrastructure.Application;
using System.IO;
using Microsoft.Practices.Prism.Events;
using Ertis.Infrastructure.Events;
using System.Globalization;
using Ertis.Themes.Events;
using Ertis.Themes;

namespace Ertis.Infrastructure.Services
{
    public class SettingsService : ISettingsService
    {
        #region Services

        private readonly IEventAggregator eventAggregator;

        #endregion

        #region Constants

        private readonly string APP_SETTINGS_FILE_PATH = System.AppDomain.CurrentDomain.BaseDirectory + @"appSettings.json";

        #endregion

        #region Properties

        private AppSettings[] AllSettings { get; set; }

        #endregion

        #region Constructors

        public SettingsService(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.AllSettings = this.ReadAllSettings();

            this.eventAggregator.GetEvent<LanguageChangedEvent>().Subscribe(OnLanguageChanged);
            this.eventAggregator.GetEvent<ThemeChangedEvent>().Subscribe(OnThemeChanged);
        }

        #endregion

        #region Methods

        private AppSettings[] ReadAllSettings()
        {
            if (File.Exists(this.APP_SETTINGS_FILE_PATH))
            {
                try
                {
                    string settingsJson = File.ReadAllText(this.APP_SETTINGS_FILE_PATH);
                    var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettings[]>(settingsJson);

                    return settings;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("AppSettings.json file cannot read or deserialize!");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            return new AppSettings[0];
        }

        public AppSettings GetLastSettings()
        {
            if (this.AllSettings != null && this.AllSettings.Length > 0)
                return this.AllSettings[0];
            else
                return AppSettings.GetDefaultSettings();
        }

        public AppSettings GetUserSettings(int userId)
        {
            if (this.AllSettings.Any(x => x.UserID == userId))
            {
                return this.AllSettings.First(x => x.UserID == userId);
            }

            return null;
        }

        public void SaveUserSettings(int userId)
        {
            var currentUserSettings = this.GetUserSettings(userId);

            if (currentUserSettings != null)
                this.SaveUserSettings(currentUserSettings);
            else
                this.CreateInitialSettings(userId);
        }

        public void SaveUserSettings(AppSettings settings)
        {
            if (settings == null)
                return;

            var allSettingsList = this.AllSettings.ToList();

            if (allSettingsList.Any(x => x.UserID == settings.UserID))
            {
                var foundSettings = allSettingsList.First(x => x.UserID == settings.UserID);
                allSettingsList.Remove(foundSettings);
            }

            allSettingsList.Insert(0, settings);
            this.AllSettings = allSettingsList.ToArray();

            string settingsJson = Newtonsoft.Json.JsonConvert.SerializeObject(this.AllSettings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(APP_SETTINGS_FILE_PATH, settingsJson);
        }

        public AppSettings CreateInitialSettings(int userId)
        {
            if (!this.AllSettings.Any(x => x.UserID == userId))
            {
                var userSettings = AppSettings.GetDefaultSettings();
                userSettings.UserID = userId;

                this.SaveUserSettings(userSettings);

                return userSettings;
            }
            else
            {
                return this.AllSettings.First(x => x.UserID == userId);
            }
        }

        #endregion

        #region Event Handlers

        private void OnLanguageChanged(CultureInfo culture)
        {
            var lastSettings = this.GetLastSettings();
            lastSettings.Culture = culture.Name;
            this.SaveUserSettings(lastSettings);
        }

        private void OnThemeChanged(Theme theme)
        {
            var lastSettings = this.GetLastSettings();
            lastSettings.ThemeName = theme.Key;
            this.SaveUserSettings(lastSettings);
        }

        #endregion
    }
}
