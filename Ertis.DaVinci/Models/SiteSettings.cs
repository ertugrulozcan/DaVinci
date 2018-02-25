using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.DaVinci.Models
{
    public class SiteSettings : INotifyPropertyChanged, ICloneable
    {
        #region Fields

        private string address = "Atakent Mah. 231. Sokak 1I No: 46\nKucukcekmece/Istanbul\n34303\nTURKEY";
        private string phoneNumber = "(0216) 123 4567";
        private string emailAddress = "info@culcuoglu.com.tr";
        private bool hasFacebookAccount = true;
        private bool hasTwitterAccount = true;
        private bool hasInstagramAccount = true;
        private bool hasLinkedinAccount = true;
        private string facebookAccountLink = "https://www.facebook.com/";
        private string twitterAccountLink = "https://www.twitter.com/";
        private string instagramAccountLink = "https://www.instagram.com/";
        private string linkedinAccountLink = "https://www.linkedin.com/";
        private string latitude = "41.039220";
        private string longitude = "28.769581";
        private string footerText = "© 2018 Culcuoglu";

        #endregion

        #region Properties

        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                this.address = value;
                this.RaisePropertyChanged("Address");
            }
        }

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }

            set
            {
                this.phoneNumber = value;
                this.RaisePropertyChanged("PhoneNumber");
            }
        }

        public string EmailAddress
        {
            get
            {
                return emailAddress;
            }

            set
            {
                this.emailAddress = value;
                this.RaisePropertyChanged("EmailAddress");
            }
        }

        public bool HasFacebookAccount
        {
            get
            {
                return hasFacebookAccount;
            }

            set
            {
                this.hasFacebookAccount = value;
                this.RaisePropertyChanged("HasFacebookAccount");
            }
        }

        public bool HasTwitterAccount
        {
            get
            {
                return hasTwitterAccount;
            }

            set
            {
                this.hasTwitterAccount = value;
                this.RaisePropertyChanged("HasTwitterAccount");
            }
        }

        public bool HasInstagramAccount
        {
            get
            {
                return hasInstagramAccount;
            }

            set
            {
                this.hasInstagramAccount = value;
                this.RaisePropertyChanged("HasInstagramAccount");
            }
        }

        public bool HasLinkedinAccount
        {
            get
            {
                return hasLinkedinAccount;
            }

            set
            {
                this.hasLinkedinAccount = value;
                this.RaisePropertyChanged("HasLinkedinAccount");
            }
        }

        public string FacebookAccountLink
        {
            get
            {
                return facebookAccountLink;
            }

            set
            {
                this.facebookAccountLink = value;
                this.RaisePropertyChanged("FacebookAccountLink");
            }
        }

        public string TwitterAccountLink
        {
            get
            {
                return twitterAccountLink;
            }

            set
            {
                this.twitterAccountLink = value;
                this.RaisePropertyChanged("TwitterAccountLink");
            }
        }

        public string InstagramAccountLink
        {
            get
            {
                return instagramAccountLink;
            }

            set
            {
                this.instagramAccountLink = value;
                this.RaisePropertyChanged("InstagramAccountLink");
            }
        }

        public string LinkedinAccountLink
        {
            get
            {
                return linkedinAccountLink;
            }

            set
            {
                this.linkedinAccountLink = value;
                this.RaisePropertyChanged("LinkedinAccountLink");
            }
        }

        public string Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                this.latitude = value;
                this.RaisePropertyChanged("Latitude");
            }
        }

        public string Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                this.longitude = value;
                this.RaisePropertyChanged("Longitude");
            }
        }

        public string FooterText
        {
            get
            {
                return footerText;
            }

            set
            {
                this.footerText = value;
                this.RaisePropertyChanged("FooterText");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SiteSettings()
        {

        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            return new SiteSettings()
            {
                Address = this.Address,
                PhoneNumber = this.PhoneNumber,
                EmailAddress = this.EmailAddress,
                HasFacebookAccount = this.HasFacebookAccount,
                HasTwitterAccount = this.HasTwitterAccount,
                HasInstagramAccount = this.HasInstagramAccount,
                HasLinkedinAccount = this.HasLinkedinAccount,
                FacebookAccountLink = this.FacebookAccountLink,
                TwitterAccountLink = this.TwitterAccountLink,
                InstagramAccountLink = this.InstagramAccountLink,
                LinkedinAccountLink = this.LinkedinAccountLink,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                FooterText = this.FooterText,
            };
        }

        public void Overwrite(SiteSettings settings)
        {
            this.Address = settings.Address;
            this.PhoneNumber = settings.PhoneNumber;
            this.EmailAddress = settings.EmailAddress;
            this.HasFacebookAccount = settings.HasFacebookAccount;
            this.HasTwitterAccount = settings.HasTwitterAccount;
            this.HasInstagramAccount = settings.HasInstagramAccount;
            this.HasLinkedinAccount = settings.HasLinkedinAccount;
            this.FacebookAccountLink = settings.FacebookAccountLink;
            this.TwitterAccountLink = settings.TwitterAccountLink;
            this.InstagramAccountLink = settings.InstagramAccountLink;
            this.LinkedinAccountLink = settings.LinkedinAccountLink;
            this.Latitude = settings.Latitude;
            this.Longitude = settings.Longitude;
            this.FooterText = settings.FooterText;
        }

        #endregion
    }
}
