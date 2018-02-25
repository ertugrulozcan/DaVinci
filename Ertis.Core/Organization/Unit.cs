using System;
using Newtonsoft.Json;

namespace Ertis.Core.Organization
{
    public abstract class Unit : OrganizationUnit
    {
        #region Fields

        private Department department;

        #endregion

        #region Properties

        [JsonIgnore]
        public Department Department
        {
            get
            {
                return department;
            }

            set
            {
                department = value;
            }
        }

        public int DepartmentId
        {
            get
            {
                if (this.Department == null)
                    return 0;

                return this.Department.Id;
            }
        }

        #endregion

        #region Constructors

        protected Unit(int id, string name, Department department) : base(id, name)
        {
            this.Department = department;
        }

        #endregion
    }
}
