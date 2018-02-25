using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ertis.Core.Data;
using Ertis.Core.Human;
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
    public class StaffController : Controller
    {
        #region Services

        public IOrganizationService organizationService { get; }

        #endregion

        #region Constructors

        public StaffController()
        {
            this.organizationService = Services.ServiceProvider.Current.OrganizationService;
        }

        #endregion

        #region Controller Methods

        // GET: api/values
        [HttpGet]
        public IEnumerable<Staff> Get()
        {
            return this.organizationService.GetStaffList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Staff Get(int id)
        {
            return this.organizationService.GetStaff(id);
        }

        // POST api/values
        [HttpPost]
        public Staff Post([FromBody]StaffForm form)
        {
            var result = this.organizationService.AddStaff(form);
            if (result.Success)
                return result.Data as Staff;
            else
                return null;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public Staff Put(int id, [FromBody]StaffForm form)
        {
            var result = this.organizationService.UpdateStaff(id, form);
            if (result.Success)
                return result.Data as Staff;
            else
                return null;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.organizationService.RemoveStaff(id);
        }

        #endregion

        #region Position Methods

        [Route("positions/{id}")]
        public Position GetPosition(int id)
        {
            return this.organizationService.GetPosition(id);
        }

        [Route("positions")]
        public IEnumerable<Position> GetPositionList()
        {
            return this.organizationService.GetPositionList();
        }

        [HttpPost("positions")]
        public Result AddPosition([FromBody]PositionForm form)
        {
            return this.organizationService.AddPosition(form);
        }

        [HttpPut("positions/{id}")]
        public Result UpdatePosition(int id, [FromBody]PositionForm form)
        {
            return this.organizationService.UpdatePosition(id, form);
        }

        [HttpDelete("positions/{id}")]
        public Result DeletePosition(int id)
        {
            return this.organizationService.RemovePosition(id);
        }

        #endregion
    }
}
