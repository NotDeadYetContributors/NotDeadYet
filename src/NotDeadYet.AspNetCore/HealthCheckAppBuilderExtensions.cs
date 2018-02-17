using System;
using Microsoft.AspNetCore.Builder;

namespace NotDeadYet.AspNetCore
{
    public static class HealthCheckAppBuilderExtensions
    {
        public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app, Action<HealthCheckOptions> setupAction)
        {
            var options = new HealthCheckOptions();
            setupAction?.Invoke(options);

            app.UseMiddleware<HealthCheckMiddleware>(options);

            return app;
        }

        public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app)
        {
            return app.UseHealthCheck(null);
        }
    }
}
