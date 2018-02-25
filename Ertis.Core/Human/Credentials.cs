using System;
namespace Ertis.Core.Human
{
    public class Credentials : ICloneable
    {
        #region Fields

        private int id;
        private string name = string.Empty;
        private string surname = string.Empty;
        private DateTime? birthDate;
        private string emailAddress;
        private string phoneNumber;

        #endregion

        #region Properties

        public int Id
        {
            get
            {
                return id;
            }
            protected set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
            }
        }

        public string FullName
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }

        public DateTime? BirthDate
        {
            get
            {
                return birthDate;
            }
            set
            {
                birthDate = value;
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
                emailAddress = value;
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
                phoneNumber = value;
            }
        }

        #endregion

        #region Constructors

        public Credentials()
        {

        }

        public Credentials(int id, string name, string surname)
        {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
        }

        public Credentials(int id, string name, string surname, DateTime? birthDate, string emailAddress, string phoneNumber)
        {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
            this.BirthDate = birthDate;
            this.EmailAddress = emailAddress;
            this.PhoneNumber = phoneNumber;
        }

        #endregion

        #region Methods

        public void Override(Credentials credentials, bool onlyChangedFields = true)
        {
            if (onlyChangedFields)
            {
                if (!string.IsNullOrEmpty(credentials.Name))
                    this.Name = credentials.Name;

                if (!string.IsNullOrEmpty(credentials.Surname))
                    this.Surname = credentials.Surname;

                if (!string.IsNullOrEmpty(credentials.EmailAddress))
                    this.EmailAddress = credentials.EmailAddress;

                if (!string.IsNullOrEmpty(credentials.PhoneNumber))
                    this.PhoneNumber = credentials.PhoneNumber;

                if (credentials.BirthDate != null)
                    this.BirthDate = credentials.BirthDate;
            }
            else
            {
                this.Name = credentials.Name;
                this.Surname = credentials.Surname;
                this.BirthDate = credentials.BirthDate;
                this.EmailAddress = credentials.EmailAddress;
                this.PhoneNumber = credentials.PhoneNumber;
            }
        }

        public override string ToString()
        {
            return this.FullName;
        }

        public object Clone()
        {
            return new Credentials()
            {
                Id = this.Id,
                Name = this.Name,
                Surname = this.Surname,
                BirthDate = this.BirthDate,
                EmailAddress = this.EmailAddress,
                PhoneNumber = this.PhoneNumber,
            };
        }

        #endregion
    }
}