using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace SPR.TestDataStorage.WebService.Controllers
{
    [Route("version")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return new JsonResult(new VersionModel { Version = version?.ToString() });
        }

        public class VersionModel
        {
            public string? Version { get; set; }
        }
    }
}
