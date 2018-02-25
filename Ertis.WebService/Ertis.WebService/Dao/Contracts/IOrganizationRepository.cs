using System;
using System.Collections.Generic;
using Ertis.Core.Organization;
using Ertis.WebService.Models;

namespace Ertis.WebService.Dao.Contracts
{
    public interface IOrganizationRepository
    {
        // Company
        Company GetCompany();

        // Department
        Department GetDepartment(int id);
        List<Department> GetDepartmentList();
        Department AddDepartment(string name, int managerId);
        Department AddDepartment(Department department);
        bool UpdateDepartment(int id, DepartmentForm form);
        bool UpdateDepartment(int id, string name = null, int? managerId = null);
        bool RemoveDepartment(int id);

        // Section
        Section GetSection(int id);
        List<Section> GetSectionList();
        Section AddSection(string name, int departmentId, int authorId);
        Section AddSection(Section section);
        bool UpdateSection(int id, SectionForm form);
        bool UpdateSection(int id, string name = null, int? departmentId = null, int? authorId = null);
        bool RemoveSection(int id);

        // Team
        Team GetTeam(int id);
        List<Team> GetTeamList();
        Team AddTeam(string name, int departmentId, int sectionId, int teamLeadId);
        Team AddTeam(Team team);
        bool UpdateTeam(int id, TeamForm form);
        bool UpdateTeam(int id, string name, int? departmentId, int? sectionId, int? teamLeadId);
        bool RemoveTeam(int id);
    }
}
