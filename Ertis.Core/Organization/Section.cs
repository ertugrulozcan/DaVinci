using System;
using System.Collections.Generic;
using Ertis.Core.Human;
using Newtonsoft.Json;

namespace Ertis.Core.Organization
{
    public class Section : Unit
    {
        #region Fields

        private int authorId;
        private List<Unit> subUnits;
        private int parentUnitId;
        private OrganizationUnit parentUnit;

        #endregion

        #region Properties

        public int AuthorId
        {
            get
            {
                return this.authorId;
            }

            private set
            {
                this.authorId = value;
            }
        }

        public List<Unit> SubUnits
        {
            get
            {
                return subUnits;
            }

            internal set
            {
                subUnits = value;
            }
        }

        [JsonIgnore]
        public override OrganizationUnit ParentUnit
        {
            get
            {
                if (this.parentUnit == null)
                    return this.Department;
                else
                    return this.parentUnit;
            }

            set
            {
                if (value is Department)
                    this.Department = value as Department;

                this.parentUnit = value;
            }
        }

        public int ParentUnitId
        {
            get
            {
                if (this.ParentUnit != null)
                    return this.ParentUnit.Id;

                return this.parentUnitId;
            }

            private set
            {
                this.parentUnitId = value;
            }
        }

        #endregion

        #region Constructors

        public Section(int id, string name, OrganizationUnit parentUnit, int authorId) : base(id, name, FindParentDepartment(parentUnit))
        {
            this.SubUnits = new List<Unit>();
            this.parentUnit = parentUnit;
            this.AuthorId = authorId;
        }

        public Section(int id, string name, int parentUnitId, int authorId) : base(id, name, null)
        {
            this.SubUnits = new List<Unit>();
            this.ParentUnitId = parentUnitId;
            this.AuthorId = authorId;
        }

        #endregion

        #region Methods

        private static Department FindParentDepartment(OrganizationUnit unit)
        {
            if (unit == null)
                return null;
            
            if (unit is Department)
                return unit as Department;

            return FindParentDepartment(unit.ParentUnit);
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
