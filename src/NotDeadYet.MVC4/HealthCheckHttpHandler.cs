using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NotDeadYet.Results;

namespace NotDeadYet.MVC4
{
    public class HealthCheckHttpHandler : IHttpHandler
    {
        private readonly IHealthChecker _healthChecker;

        public HealthCheckHttpHandler(IHealthChecker healthChecker)
        {
            _healthChecker = healthChecker;
        }

        public void ProcessRequest(HttpContext context)
        {
            var result = _healthChecker.Check();

            var response = context.Response;
            var json = JsonConvert.SerializeObject(result, Formatting.Indented, new StringEnumConverter());

            response.StatusCode = result.Status == HealthCheckStatus.Okay ? 200 : 503;
            response.ContentType = "text/plain";
            response.Headers["Content-Disposition"] = "inline";
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Write(json);
            response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}