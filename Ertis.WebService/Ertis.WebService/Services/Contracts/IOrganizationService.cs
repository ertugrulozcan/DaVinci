using System;
using System.Collections.Generic;
using Ertis.Core.Data;
using Ertis.Core.Human;
using Ertis.Core.Organization;
using Ertis.WebService.Models;

namespace Ertis.WebService.Services.Contracts
{
    public interface IOrganizationService
    {
        #region Company

        Company GetCompany();

        #endregion

        #region Staffs

        Staff GetStaff(int id);
        List<Staff> GetStaffList();
        Result AddStaff(StaffForm form);
        Result UpdateStaff(int id, StaffForm form);
        Result RemoveStaff(int id);

        #endregion

        #region Positions

        Position GetPosition(int id);
        List<Position> GetPositionList();
        Result AddPosition(PositionForm form);
        Result UpdatePosition(int id, PositionForm form);
        Result RemovePosition(int id);

        #endregion

        #region Departments

        Department GetDepartment(int id);
        List<Department> GetDepartmentList();
        Result AddDepartment(string name, int managerId);
        Result AddDepartment(Department department);
        Result UpdateDepartment(int id, DepartmentForm form);
        Result RemoveDepartment(int id);

        #endregion

        #region Sections

        Section GetSection(int id);
        List<Section> GetSectionList();
        Result AddSection(string name, int authorId, int parentUnitId);
        Result AddSection(Section section);
        Result UpdateSection(int id, SectionForm form);
        Result RemoveSection(int id);

        #endregion

        #region Teams

        Team GetTeam(int id);
        List<Team> GetTeamList();
        Result AddTeam(string name, int teamLeadId, int departmentId, int sectionId);
        Result AddTeam(Team team);
        Result UpdateTeam(int id, TeamForm form);
        Result RemoveTeam(int id);

        #endregion
    }
}
