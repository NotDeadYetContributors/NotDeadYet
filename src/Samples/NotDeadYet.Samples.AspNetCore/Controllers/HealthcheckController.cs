using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NotDeadYet.Samples.AspNetCore.Controllers
{
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using NotDeadYet.Results;

    public class HealthcheckController : Controller
    {
        private static IHealthChecker _healthChecker;

        static HealthcheckController()
        {
            var thisAssembly = typeof(HealthcheckController).GetTypeInfo().Assembly;
            _healthChecker = new HealthCheckerBuilder()
                .WithHealthChecksFromAssemblies(thisAssembly)
                .Build();
        }

        [HttpGet("healthcheck")]
        public ContentResult Index()
        {
            var result = _healthChecker.Check();

            var json = JsonConvert.SerializeObject(result, Formatting.Indented, new StringEnumConverter());

            var ret = new ContentResult()
            {
                Content = json,
                ContentType = "text/plain",
                StatusCode = result.Status == HealthCheckStatus.Okay ? 200 : 503
            };
            this.Response.Headers.Add("Content-Disposition", "inline");
            this.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            this.Response.Headers.Add("Pragma", "no-cache");
            return ret;
        }
    }
}