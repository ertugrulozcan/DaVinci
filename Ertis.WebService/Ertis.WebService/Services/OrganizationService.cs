using System;
using System.Collections.Generic;
using System.Linq;
using Ertis.Core.Data;
using Ertis.Core.Human;
using Ertis.Core.Organization;
using Ertis.Core.Server;
using Ertis.WebService.Dao.Contracts;
using Ertis.WebService.Models;
using Ertis.WebService.Services.Contracts;

namespace Ertis.WebService.Services
{
    public class OrganizationService : IOrganizationService
    {
        #region Services

        private readonly IOrganizationRepository organizationRepository;
        private readonly IStaffRepository staffRepository;
        private readonly ICredentialsRepository credentialsRepository;
        private IUserService userService;

        #endregion

        #region Fields

        private Company organization;
        private List<Department> departmentList;
        private List<Section> sectionList;
        private List<Team> teamList;
        private List<Staff> staffList;
        private List<Credentials> credentialsList;
        private List<Position> positionList;

        #endregion

        #region Properties

        public IUserService UserService
        {
            get
            {
                if (this.userService == null)
                    this.userService = ServiceProvider.Current.UserService;

                return this.userService;
            }
        }

        public Company Organization
        {
            get
            {
                return this.organization;
            }

            private set
            {
                this.organization = value;
            }
        }

        public List<Department> DepartmentList
        {
            get
            {
                return this.departmentList;
            }

            private set
            {
                this.departmentList = value;
            }
        }

        public List<Section> SectionList
        {
            get
            {
                return this.sectionList;
            }

            private set
            {
                this.sectionList = value;
            }
        }

        public List<Team> TeamList
        {
            get
            {
                return this.teamList;
            }

            private set
            {
                this.teamList = value;
            }
        }

        public List<Staff> StaffList
        {
            get
            {
                return this.staffList;
            }

            private set
            {
                this.staffList = value;
            }
        }

        public List<Credentials> CredentialsList
        {
            get
            {
                return this.credentialsList;    
            }

            private set
            {
                this.credentialsList = value;
            }
        }

        public List<Position> PositionList
        {
            get
            {
                return this.positionList;
            }

            private set
            {
                this.positionList = value;
            }
        }

        #endregion

        #region Constructors

        public OrganizationService(IOrganizationRepository organizationRepository, IStaffRepository staffRepository, ICredentialsRepository credentialsRepository)
        {
            this.organizationRepository = organizationRepository;
            this.staffRepository = staffRepository;
            this.credentialsRepository = credentialsRepository;

            this.Initialize();
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            this.Organization = this.organizationRepository.GetCompany();
            this.DepartmentList = this.organizationRepository.GetDepartmentList();
            this.SectionList = this.organizationRepository.GetSectionList();
            this.TeamList = this.organizationRepository.GetTeamList();
            this.StaffList = this.staffRepository.GetStaffList();
            this.CredentialsList = this.credentialsRepository.GetList();
            this.PositionList = this.staffRepository.GetPositionList();

            this.SetRelations();
        }

        private void SetRelations()
        {
            foreach (var staff in this.StaffList)
            {
                staff.ErtisUser = this.UserService.GetUser(staff.UserId);
                staff.Position = this.PositionList.FirstOrDefault(x => x.Id == staff.PositionId);
                staff.Department = this.DepartmentList.FirstOrDefault(x => x.Id == staff.DepartmentId);
                staff.Section = this.SectionList.FirstOrDefault(x => x.Id == staff.SectionId);
                staff.Team = this.TeamList.FirstOrDefault(x => x.Id == staff.TeamId);
                staff.UserCredentials = this.CredentialsList.FirstOrDefault(x => x.Id == staff.CredentialsId);
            }

            foreach (var team in this.TeamList)
            {
                team.Section = this.SectionList.FirstOrDefault(x => x.Id == team.SectionId);
                team.Department = this.DepartmentList.FirstOrDefault(x => x.Id == team.DepartmentId);
                //team.TeamLead = this.StaffList.FirstOrDefault(x => x.Id == team.TeamLeadId);
                team.StaffList.AddRange(this.StaffList.Where(x => x.TeamId == team.Id));
            }

            foreach (var section in this.SectionList)
            {
                section.Department = this.DepartmentList.FirstOrDefault(x => x.Id == section.ParentUnitId && section.ParentUnit is Department);
                section.ParentUnit = this.SectionList.FirstOrDefault(x => x.Id == section.ParentUnitId);
                // section.Author = this.StaffList.FirstOrDefault(x => x.Id == section.AuthorId);
                section.SubUnits.AddRange(this.SectionList.Where(x => x.ParentUnitId == section.Id));
                section.SubUnits.AddRange(this.TeamList.Where(x => x.SectionId == section.Id));
                section.StaffList.AddRange(this.StaffList.Where(x => x.TeamId == section.Id));
            }

            foreach (var department in this.DepartmentList)
            {
                // department.Manager = this.StaffList.FirstOrDefault(x => x.Id == department.ManagerId);
                department.UnitList.AddRange(this.SectionList.Where(x => x.ParentUnit.Id == department.Id));
                department.UnitList.AddRange(this.TeamList.Where(x => x.DepartmentId == department.Id));
                department.StaffList.AddRange(this.StaffList.Where(x => x.DepartmentId == department.Id));
            }
        }

        #endregion

        #region Company

        public Company GetCompany()
        {
            return this.Organization;
        }

        #endregion

        #region Staffs

        public Staff GetStaff(int id)
        {
            return this.StaffList.FirstOrDefault(x => x.Id == id);
        }

        public List<Staff> GetStaffList()
        {
            return this.StaffList;
        }

        public Result AddStaff(StaffForm form)
        {
            var newStaff = this.staffRepository.AddStaff(form);
            if (newStaff == null)
                return new Result(Errors.STAFF_CREATE_ERROR, 500);

            this.StaffList.Add(newStaff);
            return new Result(true, 200, "Staff successfully added.", newStaff);
        }

        public Result UpdateStaff(int id, StaffForm form)
        {
            bool isSuccess = this.staffRepository.UpdateStaff(id, form);
            if (!isSuccess)
                return new Result(Errors.STAFF_UPDATE_ERROR, 500);

            if (this.StaffList.Any(x => x.Id == id))
            {
                var updatedStaff = this.StaffList.First(x => x.Id == id);
                int indexOfUpdatedStaff = this.StaffList.IndexOf(updatedStaff);
                this.StaffList.RemoveAt(indexOfUpdatedStaff);

                var newStaff = this.staffRepository.GetStaff(id);
                this.StaffList.Add(newStaff);

                return new Result(true, 200, "Staff successfully updated.", newStaff);
            }
            else
            {
                return new Result(false, 404, string.Format("Staff[{0}] could not found in service list.", id));
            }
        }

        public Result RemoveStaff(int id)
        {
            bool isSuccess = this.staffRepository.RemoveStaff(id);
            if (!isSuccess)
                return new Result(Errors.STAFF_DELETE_ERROR, 500);

            if (this.StaffList.Any(x => x.Id == id))
            {
                var deletedStaff = this.StaffList.First(x => x.Id == id);
                this.StaffList.Remove(deletedStaff);

                var userList = this.UserService.GetUserList();
                if (!userList.Any(x => x.Card.Id == deletedStaff.CredentialsId))
                {
                    this.UserService.RemoveCredentials(deletedStaff.CredentialsId);
                }

                return new Result(true, 200, "Staff successfully deleted.", deletedStaff);
            }

            return new Result(true, 200, "Staff could not found in service list.", null);
        }

        #endregion

        #region Positions

        public Position GetPosition(int id)
        {
            return this.PositionList.FirstOrDefault(x => x.Id == id);
        }

        public List<Position> GetPositionList()
        {
            return this.PositionList;
        }

        public Result AddPosition(PositionForm form)
        {
            var newPosition = this.staffRepository.AddPosition(form);
            if (newPosition == null)
                return new Result(Errors.POSITION_CREATE_ERROR, 500);

            this.PositionList.Add(newPosition);
            return new Result(true, 200, "Position successfully added.", newPosition);
        }

        public Result UpdatePosition(int id, PositionForm form)
        {
            bool isSuccess = this.staffRepository.UpdatePosition(id, form);
            if (!isSuccess)
                return new Result(Errors.POSITION_UPDATE_ERROR, 500);

            if (this.PositionList.Any(x => x.Id == id))
            {
                var updatedPosition = this.PositionList.First(x => x.Id == id);
                int indexOfUpdatedPosition = this.PositionList.IndexOf(updatedPosition);
                this.PositionList.RemoveAt(indexOfUpdatedPosition);

                var newPosition = this.staffRepository.GetPosition(id);
                this.PositionList.Add(newPosition);

                return new Result(true, 200, "Position successfully updated.", newPosition);
            }
            else
            {
                return new Result(false, 404, string.Format("Position[{0}] could not found in service list.", id));
            }
        }

        public Result RemovePosition(int id)
        {
            bool isSuccess = this.staffRepository.RemovePosition(id);
            if (!isSuccess)
                return new Result(Errors.POSITION_DELETE_ERROR, 500);

            if (this.PositionList.Any(x => x.Id == id))
            {
                var deletedPosition = this.PositionList.First(x => x.Id == id);
                this.PositionList.Remove(deletedPosition);

                return new Result(true, 200, "Position successfully deleted.", deletedPosition);
            }

            return new Result(true, 200, "Position could not found in service list.", null);
        }

        #endregion

        #region Departments

        public Department GetDepartment(int id)
        {
            return this.DepartmentList.FirstOrDefault(x => x.Id == id);
        }

        public List<Department> GetDepartmentList()
        {
            return this.DepartmentList;
        }

        public Result AddDepartment(string name, int managerId)
        {
            var newDepartment = this.organizationRepository.AddDepartment(name, managerId);
            if (newDepartment == null)
                return new Result(Errors.DEPARTMENT_CREATE_ERROR, 500);

            this.DepartmentList.Add(newDepartment);
            return new Result(true, 200, "Department successfully added.", newDepartment);
        }

        public Result AddDepartment(Department department)
        {
            var newDepartment = this.organizationRepository.AddDepartment(department);
            if (newDepartment == null)
                return new Result(Errors.DEPARTMENT_CREATE_ERROR, 500);

            this.DepartmentList.Add(newDepartment);
            return new Result(true, 200, "Department successfully added.", newDepartment);
        }

        public Result UpdateDepartment(int id, DepartmentForm form)
        {
            bool isSuccess = this.organizationRepository.UpdateDepartment(id, form);
            if (!isSuccess)
                return new Result(Errors.DEPARTMENT_UPDATE_ERROR, 500);

            if (this.DepartmentList.Any(x => x.Id == id))
            {
                var updatedDepartment = this.DepartmentList.First(x => x.Id == id);
                int indexOfUpdatedDepartment = this.DepartmentList.IndexOf(updatedDepartment);
                this.DepartmentList.RemoveAt(indexOfUpdatedDepartment);

                var newDepartment = this.organizationRepository.GetDepartment(id);
                this.DepartmentList.Add(newDepartment);

                return new Result(true, 200, "Department successfully updated.", newDepartment);
            }
            else
            {
                return new Result(false, 404, string.Format("Department[{0}] could not found in service list.", id));
            }
        }

        public Result RemoveDepartment(int id)
        {
            bool isSuccess = this.organizationRepository.RemoveDepartment(id);
            if (!isSuccess)
                return new Result(Errors.DEPARTMENT_DELETE_ERROR, 500);

            if (this.DepartmentList.Any(x => x.Id == id))
            {
                var deletedDepartment = this.DepartmentList.First(x => x.Id == id);
                this.DepartmentList.Remove(deletedDepartment);
                return new Result(true, 200, "Department successfully deleted.", deletedDepartment);
            }
            else
            {
                return new Result(false, 404, string.Format("Department[{0}] could not found in service list.", id), null);
            }
        }

        #endregion

        #region Sections

        public Section GetSection(int id)
        {
            return this.SectionList.FirstOrDefault(x => x.Id == id);
        }

        public List<Section> GetSectionList()
        {
            return this.SectionList;
        }

        public Result AddSection(string name, int authorId, int parentUnitId)
        {
            var newSection = this.organizationRepository.AddSection(name, parentUnitId, authorId);
            if (newSection == null)
                return new Result(Errors.SECTION_CREATE_ERROR, 500);

            this.SectionList.Add(newSection);
            return new Result(true, 200, "Section successfully added.", newSection);
        }

        public Result AddSection(Section section)
        {
            var newSection = this.organizationRepository.AddSection(section);
            if (newSection == null)
                return new Result(Errors.SECTION_CREATE_ERROR, 500);

            this.SectionList.Add(newSection);
            return new Result(true, 200, "Section successfully added.", newSection);
        }

        public Result UpdateSection(int id, SectionForm form)
        {
            bool isSuccess = this.organizationRepository.UpdateSection(id, form);
            if (!isSuccess)
                return new Result(Errors.SECTION_UPDATE_ERROR, 500);

            if (this.SectionList.Any(x => x.Id == id))
            {
                var updatedSection = this.SectionList.First(x => x.Id == id);
                int indexOfUpdatedSection = this.SectionList.IndexOf(updatedSection);
                this.SectionList.RemoveAt(indexOfUpdatedSection);

                var newSection = this.organizationRepository.GetSection(id);
                this.SectionList.Add(newSection);

                return new Result(true, 200, "Section successfully updated.", newSection);
            }
            else
            {
                return new Result(false, 404, string.Format("Section[{0}] could not found in service list.", id));
            }
        }

        public Result RemoveSection(int id)
        {
            bool isSuccess = this.organizationRepository.RemoveSection(id);
            if (!isSuccess)
                return new Result(Errors.SECTION_DELETE_ERROR, 500);

            if (this.SectionList.Any(x => x.Id == id))
            {
                var deletedSection = this.SectionList.First(x => x.Id == id);
                this.SectionList.Remove(deletedSection);
                return new Result(true, 200, "Section successfully deleted.", deletedSection);
            }

            return new Result(true, 200, "Section could not found in service list.", null);
        }

        #endregion

        #region Teams

        public Team GetTeam(int id)
        {
            return this.TeamList.FirstOrDefault(x => x.Id == id);
        }

        public List<Team> GetTeamList()
        {
            return this.TeamList;
        }

        public Result AddTeam(string name, int teamLeadId, int departmentId, int sectionId)
        {
            var newTeam = this.organizationRepository.AddTeam(name, departmentId, sectionId, teamLeadId);
            if (newTeam == null)
                return new Result(Errors.SECTION_CREATE_ERROR, 500);

            this.TeamList.Add(newTeam);
            return new Result(true, 200, "Team successfully added.", newTeam);
        }

        public Result AddTeam(Team team)
        {
            var newTeam = this.organizationRepository.AddTeam(team);
            if (newTeam == null)
                return new Result(Errors.TEAM_CREATE_ERROR, 500);

            this.TeamList.Add(newTeam);
            return new Result(true, 200, "Team successfully added.", newTeam);
        }

        public Result UpdateTeam(int id, TeamForm form)
        {
            bool isSuccess = this.organizationRepository.UpdateTeam(id, form);
            if (!isSuccess)
                return new Result(Errors.TEAM_UPDATE_ERROR, 500);

            if (this.TeamList.Any(x => x.Id == id))
            {
                var updatedTeam = this.TeamList.First(x => x.Id == id);
                int indexOfUpdatedTeam = this.TeamList.IndexOf(updatedTeam);
                this.TeamList.RemoveAt(indexOfUpdatedTeam);

                var newTeam = this.organizationRepository.GetTeam(id);
                this.TeamList.Add(newTeam);

                return new Result(true, 200, "Team successfully updated.", newTeam);
            }
            else
            {
                return new Result(false, 404, string.Format("Team[{0}] could not found in service list.", id));
            }
        }

        public Result RemoveTeam(int id)
        {
            bool isSuccess = this.organizationRepository.RemoveTeam(id);
            if (!isSuccess)
                return new Result(Errors.TEAM_DELETE_ERROR, 500);

            if (this.TeamList.Any(x => x.Id == id))
            {
                var deletedTeam = this.TeamList.First(x => x.Id == id);
                this.TeamList.Remove(deletedTeam);
                return new Result(true, 200, "Team successfully deleted.", deletedTeam);
            }

            return new Result(true, 200, "Team could not found in service list.", null);
        }

        #endregion
    }
}