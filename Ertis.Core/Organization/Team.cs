using System;
using Ertis.Core.Human;
using Newtonsoft.Json;

namespace Ertis.Core.Organization
{
    public class Team : Unit
    {
        #region Fields

        private int teamLeadId;
        private Section section;
        private int sectionId;
        private int departmentId;

        #endregion

        #region Properties

        public int TeamLeadId
        {
            get
            {
                return this.teamLeadId;
            }

            private set
            {
                this.teamLeadId = value;
            }
        }

        public Section Section
        {
            get
            {
                return section;
            }

            set
            {
                section = value;
            }
        }

        [JsonIgnore]
        public override OrganizationUnit ParentUnit
        {
            get
            {
                if (this.Section != null)
                    return this.Section;
                else
                    return this.Department;
            }

            set
            {
                if (value is Section)
                    this.section = value as Section;
                else if (value is Department)
                    this.Department = value as Department;
            }
        }

        public int SectionId
        {
            get
            {
                if (this.Section != null)
                    return this.Section.Id;

                return this.sectionId;
            }

            private set
            {
                this.sectionId = value;
            }
        }

        public int DepartmentId
        {
            get
            {
                if (this.Department != null)
                    return this.Department.Id;

                return this.departmentId;
            }

            private set
            {
                this.departmentId = value;
            }
        }

        #endregion

        #region Constructors

        public Team(int id, string name, Section section, Department department, int teamLeadId) : base(id, name, department)
        {
            this.Section = section;
            this.TeamLeadId = teamLeadId;
        }

        public Team(int id, string name, int sectionId, int departmentId, int teamLeadId) : base(id, name, null)
        {
            this.SectionId = sectionId;
            this.DepartmentId = departmentId;
            this.TeamLeadId = teamLeadId;
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
