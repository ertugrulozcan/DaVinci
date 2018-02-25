using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ertis.WebService.Controllers
{
    [Route("api/[controller]")]
    public class LocalizationController : Controller
    {
        // GET api/localization/tr-TR
        [HttpGet("{culture}")]
        public string Get(string culture)
        {
            return Localization.LocalizationManager.Current.GetDictionaryAsJson(culture);
        }
    }
}
