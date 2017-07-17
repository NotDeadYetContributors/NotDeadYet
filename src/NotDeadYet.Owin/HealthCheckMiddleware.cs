using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NotDeadYet.Results;
using System.Net;
using System.Threading.Tasks;

namespace NotDeadYet.Owin
{
    public class HealthCheckMiddleware : OwinMiddleware
    {
        public PathString Path { get; set; }
        private readonly IHealthChecker _healthChecker;

        public HealthCheckMiddleware(OwinMiddleware next, IHealthChecker healthChecker, PathString path) : base(next)
        {
            Path = path;
            _healthChecker = healthChecker;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (!context.Request.Path.Equals(Path))
                return Next.Invoke(context);

            var result = _healthChecker.Check();

            var json = JsonConvert.SerializeObject(result, Formatting.Indented, new StringEnumConverter());
            var httpStatusCode = result.Status == HealthCheckStatus.Okay ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable;
            var response = context.Response;

            response.StatusCode = (int)httpStatusCode;
            response.ContentType = "application/json";
            response.Headers["Cache-control"] = "no-cache";
            return response.WriteAsync(json);
        }
    }
}
