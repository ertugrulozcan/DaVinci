using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Application
{
    public class AppSettings
    {
        #region Fields

        private int userID;
        private string culture;
        private string themeName;

        #endregion

        #region Properties

        public int UserID
        {
            get
            {
                return userID;
            }

            set
            {
                this.userID = value;
            }
        }

        public string Culture
        {
            get
            {
                return culture;
            }

            set
            {
                this.culture = value;
            }
        }

        public string ThemeName
        {
            get
            {
                return themeName;
            }

            set
            {
                this.themeName = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppSettings()
        {

        }

        #endregion

        #region Methods

        public static AppSettings GetDefaultSettings()
        {
            return new AppSettings()
            {
                Culture = "tr",
                ThemeName = "ErtisLight"
            };
        }

        internal void Overwrite(AppSettings settings)
        {
            if (settings == null)
                return;

            this.UserID = settings.UserID;
            this.Culture = settings.Culture;
            this.ThemeName = settings.ThemeName;
        }

        #endregion
    }
}
