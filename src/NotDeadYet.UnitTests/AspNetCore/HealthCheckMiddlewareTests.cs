using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NotDeadYet.AspNetCore;
using NotDeadYet.HealthChecks;
using NUnit.Framework;
using Shouldly;

namespace NotDeadYet.UnitTests.AspNetCore
{
    public class HealthCheckMiddlewareTests
    {
        IHealthChecker _healthChecker;
        IHealthChecker _failingHealthChecker;

        public HealthCheckMiddlewareTests()
        {
            _healthChecker = HealthCheckerTests.GetHealthChecker(b => b.WithHealthChecks(() => new[] { new ApplicationIsRunning() }));

            _failingHealthChecker = HealthCheckerTests.GetHealthChecker(b => b.WithHealthChecks(() => new[] { new FailingHealthCheck() }));
        }

        [Test]
        public async Task ShouldRespondWith200StatusCode()
        {
            //Arrange
            var next = GetRequestDelegate();

            var httpContext = new DefaultHttpContext();

            var healthCheckMiddleware = new HealthCheckMiddleware(next, _healthChecker, new HealthCheckOptions());

            //Act
            await healthCheckMiddleware.Invoke(httpContext);

            //Assert
            httpContext.Response.StatusCode = 200;

        }


        [Test]
        public async Task ShouldRespondWith200StatusCodeWithSuccessJsonString()
        {
            //Arrange
            var next = GetRequestDelegate();

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var options = new HealthCheckOptions()
            {
                Path = "/healthcheck"
            };

            httpContext.Request.Path = "/healthcheck";

            var healthCheckMiddleware = new HealthCheckMiddleware(next, _healthChecker, options);

            //Act
            await healthCheckMiddleware.Invoke(httpContext);

            //Assert
            httpContext.Response.StatusCode.ShouldBe(200);
            httpContext.Response.ContentType.ShouldBe("text/plain");
            httpContext.Response.Headers["Content-Disposition"].ToString().ShouldBe("inline");
            httpContext.Response.Headers["Cache-Control"].ToString().ShouldBe("no-cache");

            Assert.IsTrue(ReadBodyAsString(httpContext).Contains("Checks whether the application is running. If this check can run then it should pass."));
        }

        [Test]
        public async Task ShouldRespondWith503StatusCodeWithFailureJsonString()
        {
            //Arrange
            var next = GetRequestDelegate();

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();

            var options = new HealthCheckOptions()
            {
                Path = "/healthcheck"
            };

            httpContext.Request.Path = "/healthcheck";

            var healthCheckMiddleware = new HealthCheckMiddleware(next, _failingHealthChecker, options);

            //Act
            await healthCheckMiddleware.Invoke(httpContext);

            //Assert
            httpContext.Response.StatusCode.ShouldBe(503);
            Assert.IsTrue(ReadBodyAsString(httpContext).Contains("FailingHealthCheck"));
        }


        private static string ReadBodyAsString(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var bytes = ((MemoryStream)(context.Response.Body)).ToArray();
            return Encoding.UTF8.GetString(bytes);
        }

        private RequestDelegate GetRequestDelegate()
        {
            return (httpContext) => Task.FromResult(true);
        }
    }
}
