using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ertis.Core.Human;

namespace Ertis.Core.Profile
{
    public class UserCard : Credentials
    {
        #region Fields

        private DateTime? joinDate;

        #endregion

        #region Properties

        public DateTime? JoinDate
        {
            get
            {
                return joinDate;
            }
            set
            {
                joinDate = value;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Constructor 1
        /// </summary>
        public UserCard()
        {

        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        public UserCard(int id)
        {
            this.Id = id;
        }

        /// <summary>
		/// Constructor 3
		/// </summary>
        public UserCard(string name, string surname)
        {
            this.Name = name;
            this.Surname = surname;
        }

        #endregion

        #region Methods

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(this.Name) ||
                string.IsNullOrEmpty(this.Surname) ||
                string.IsNullOrEmpty(this.EmailAddress))
                return false;

            if (!IsValidEmail(this.EmailAddress))
                return false;

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("[User: Name={0}]", this.FullName);
        }

        #endregion
    }
}
