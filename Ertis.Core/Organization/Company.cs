using System;
namespace Ertis.Core.Organization
{
    public class Company
    {
        #region Fields

        private int id;
        private string name;
        private string shortName;

        #endregion

        #region Properties

        public int Id
        {
            get
            {
                return id;
            }

            private set
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

            private set
            {
                name = value;
            }
        }

        public string ShortName
        {
            get
            {
                return shortName;
            }

            private set
            {
                shortName = value;
            }
        }

        #endregion

        #region Constructors

        public Company(int id, string name, string shortName)
        {
            this.Id = id;
            this.Name = name;
            this.ShortName = shortName;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
