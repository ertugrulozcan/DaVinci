using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Core.Data;
using Ertis.Core.Human;
using Ertis.Core.Organization;
using Ertis.WebService.Models;
using Ertis.WebService.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ertis.WebService.Controllers
{
    [Authorize]
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    public class OrganizationController : Controller
    {
        #region Services

        public IOrganizationService organizationService { get; }

        #endregion

        #region Constructors

        public OrganizationController()
        {
            this.organizationService = Services.ServiceProvider.Current.OrganizationService;
        }

        #endregion

        #region Company Methods

        [Route("company")]
        public Company GetCompany(int id)
        {
            return this.organizationService.GetCompany();
        }

        #endregion

        #region Department Methods

        [Route("departments/{id}")]
        public Department GetDepartment(int id)
        {
            return this.organizationService.GetDepartment(id);
        }

        [Route("departments")]
        public IEnumerable<Department> GetDepartmentList()
        {
            return this.organizationService.GetDepartmentList();
        }

        [HttpPost("departments")]
        public Department AddDepartment([FromBody]DepartmentForm form)
        {
            int managerId = 0;
            if (form.ManagerId != null)
                managerId = form.ManagerId.Value;
            
            var result = this.organizationService.AddDepartment(form.Name, managerId);
            if (result.Success)
                return result.Data as Department;
            else
                return null;
        }

        [HttpPut("departments/{id}")]
        public Department UpdateDepartment(int id, [FromBody]DepartmentForm form)
        {
            var result = this.organizationService.UpdateDepartment(id, form);
            if (result.Success)
                return result.Data as Department;
            else
                return null;
        }

        [HttpDelete("departments/{id}")]
        public Result DeleteDepartment(int id)
        {
            return this.organizationService.RemoveDepartment(id);
        }

        #endregion

        #region Section Methods

        [Route("sections/{id}")]
        public Section GetSection(int id)
        {
            return this.organizationService.GetSection(id);
        }

        [Route("sections")]
        public IEnumerable<Section> GetSectionList()
        {
            return this.organizationService.GetSectionList();
        }

        [HttpPost("sections")]
        public Section AddSection([FromBody]SectionForm form)
        {
            int authorId = 0;
            if (form.AuthorId != null)
                authorId = form.AuthorId.Value;

            int parentUnitId = 0;
            if (form.ParentUnitId != null)
                parentUnitId = form.ParentUnitId.Value;

            var result = this.organizationService.AddSection(form.Name, authorId, parentUnitId);
            if (result.Success)
                return result.Data as Section;
            else
                return null;
        }

        [HttpPut("sections/{id}")]
        public Section UpdateSection(int id, [FromBody]SectionForm form)
        {
            var result = this.organizationService.UpdateSection(id, form);
            if (result.Success)
                return result.Data as Section;
            else
                return null;
        }

        [HttpDelete("sections/{id}")]
        public Result DeleteSection(int id)
        {
            return this.organizationService.RemoveSection(id);
        }

        #endregion

        #region Team Methods

        [Route("teams/{id}")]
        public Team GetTeam(int id)
        {
            return this.organizationService.GetTeam(id);
        }

        [Route("teams")]
        public IEnumerable<Team> GetTeamList()
        {
            return this.organizationService.GetTeamList();
        }

        [HttpPost("teams")]
        public Team AddTeam([FromBody]TeamForm form)
        {
            int teamLeadId = 0;
            if (form.TeamLeadId != null)
                teamLeadId = form.TeamLeadId.Value;

            int parentUnitId = 0;
            if (form.DepartmentId != null)
                parentUnitId = form.DepartmentId.Value;

            int sectionId = 0;
            if (form.SectionId != null)
                sectionId = form.SectionId.Value;

            var result = this.organizationService.AddTeam(form.Name, teamLeadId, parentUnitId, sectionId);
            if (result.Success)
                return result.Data as Team;
            else
                return null;
        }

        [HttpPut("teams/{id}")]
        public Team UpdateTeam(int id, [FromBody]TeamForm form)
        {
            var result = this.organizationService.UpdateTeam(id, form);
            if (result.Success)
                return result.Data as Team;
            else
                return null;
        }

        [HttpDelete("teams/{id}")]
        public Result DeleteTeam(int id)
        {
            return this.organizationService.RemoveTeam(id);
        }

        #endregion
    }
}
