using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NotDeadYet.Results;

namespace NotDeadYet.Samples.AspNetCore.Controllers
{
    public class HealthCheckController : Controller
    {

        public HealthCheckController(IHealthChecker healthChecker)
        {
            HealthChecker = healthChecker;
        }

        public IHealthChecker HealthChecker { get; }

        // GET healthcheck
        [HttpGet("healthcheckm")]
        public ContentResult Get()
        {
            var result = HealthChecker.Check();
            var json = JsonConvert.SerializeObject(result, Formatting.Indented, new StringEnumConverter());

            var contentResult = new ContentResult()
            {
                Content = json,
                ContentType = "text/plain",
                StatusCode = result.Status == HealthCheckStatus.Okay ? 200 : 503
            };
            Response.Headers.Add("Content-Disposition", "inline");
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            return contentResult;
        }



    }
}
