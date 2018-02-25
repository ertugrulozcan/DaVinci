using System;
using System.Linq;
using System.Collections.Generic;
using Ertis.Core.Human;

namespace Ertis.Core.Organization
{
    public class Department : OrganizationUnit
    {
        #region Fields

        private int managerId;
        private List<Unit> unitList;

        #endregion

        #region Properties

        public int ManagerId
        {
            get
            {
                return this.managerId;
            }

            private set
            {
                this.managerId = value;
            }
        }

        public List<Unit> UnitList
        {
            get
            {
                return unitList;
            }

            internal set
            {
                unitList = value;
            }
        }

        public override OrganizationUnit ParentUnit
        {
            get
            {
                return null;
            }

            set
            {

            }
        }

        #endregion

        #region Constructors

        public Department(int id, string name, int managerId) : base(id, name)
        {
            this.ManagerId = managerId;
            this.UnitList = new List<Unit>();
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Department))
                return false;
            
            return this.Id == (obj as Department).Id;
        }

        #endregion
    }
}
