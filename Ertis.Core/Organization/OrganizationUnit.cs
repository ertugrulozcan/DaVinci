using System;
using System.Linq;
using System.Collections.Generic;
using Ertis.Core.Human;
using Newtonsoft.Json;

namespace Ertis.Core.Organization
{
    public abstract class OrganizationUnit
    {
        #region Fields

        private int id;
        private string name;
        private List<Staff> staffList;

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

            set
            {
                name = value;
            }
        }

        [JsonIgnore]
        public List<Staff> StaffList
        {
            get
            {
                return this.staffList;
            }

            internal set
            {
                this.staffList = value;
            }
        }

        public List<int> StaffIdList
        {
            get
            {
                if (this.StaffList == null)
                    return new List<int>();
                
                return this.staffList.Select(x => x.Id).ToList();
            }
        }

        [JsonIgnore]
        public abstract OrganizationUnit ParentUnit { get; set; }

        #endregion

        #region Constructors

        protected OrganizationUnit(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.StaffList = new List<Staff>();
        }

        #endregion
    }
}
