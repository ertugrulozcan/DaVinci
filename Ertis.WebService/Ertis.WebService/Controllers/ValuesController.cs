using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ertis.WebService.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ertis.WebService.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public ValuesController()
        {
            var organizationService = Services.ServiceProvider.Current.OrganizationService;
            var company = organizationService.GetCompany();
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
