using System;
using Microsoft.AspNetCore.Builder;

namespace NotDeadYet.Samples.AspNetCore
{
    public static class HealthCheckAppBuilderExtensions
    {
          //
        // Summary:
        //     Captures synchronous and asynchronous database related exceptions from the pipeline
        //     that may be resolved using Entity Framework migrations. When these exceptions
        //     occur an HTML response with details of possible actions to resolve the issue
        //     is generated.
        //
        // Parameters:
        //   app:
        //     The Microsoft.AspNetCore.Builder.IApplicationBuilder to register the middleware
        //     with.
        //
        //   options:
        //     A Microsoft.AspNetCore.Builder.DatabaseErrorPageOptions that specifies options
        //     for the middleware.
        //
        // Returns:
        //     The same Microsoft.AspNetCore.Builder.IApplicationBuilder instance so that multiple
        //     calls can be chained.
        public static IApplicationBuilder UseHealthCheck( this IApplicationBuilder app, Action<HealthCheckOptions> setupAction)
        {
            var options = new HealthCheckOptions();
            setupAction?.Invoke(options);

            app.UseMiddleware<HealthCheckMiddleware>(options);

            return app;
        }

        //
        // Summary:
        //     Captures synchronous and asynchronous database related exceptions from the pipeline
        //     that may be resolved using Entity Framework migrations. When these exceptions
        //     occur an HTML response with details of possible actions to resolve the issue
        //     is generated.
        //
        // Parameters:
        //   app:
        //     The Microsoft.AspNetCore.Builder.IApplicationBuilder to register the middleware
        //     with.
        //
        //   options:
        //     A Microsoft.AspNetCore.Builder.DatabaseErrorPageOptions that specifies options
        //     for the middleware.
        //
        // Returns:
        //     The same Microsoft.AspNetCore.Builder.IApplicationBuilder instance so that multiple
        //     calls can be chained.
        public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app)
        {
            return app.UseHealthCheck(null);
        }
    }
}
