using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NotDeadYet.AspNetCore
{
    /// <summary>
    ///     Extensions to scan for <see cref="IHealthCheck"/> classes and register them with the singleton <see cref="IHealthChecker"/> class
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="IHealthChecker"/> instance to the specified <see cref="IServiceCollection"/>.  
        /// Scan for <see cref="IHealthCheck"/> classes in application domain and register them with the singleton <see cref="IHealthChecker"/> class.
        /// </summary>
        public static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            Action<HealthCheckerBuilder> configureBuilder = builder => builder.WithHealthChecksFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            return AddHealthCheck(services, configureBuilder);
        }

        /// <summary>
        /// Adds <see cref="IHealthChecker"/> instance to the specified <see cref="IServiceCollection"/>. 
        /// Scan for <see cref="IHealthCheck"/> classes in assemblies specified and register them with the singleton <see cref="IHealthChecker"/> class.
        /// </summary>
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, params Type[] assemblyMarkerTypes)
        {
            return AddHealthCheck(services, b => b.WithHealthChecksFromAssemblies(assemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly).ToArray()));
        }

        /// <summary>
        /// Adds <see cref="IHealthChecker"/> instance to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, Action<HealthCheckerBuilder> healthCheckerBuilderAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (healthCheckerBuilderAction == null)
            {
                throw new ArgumentNullException(nameof(healthCheckerBuilderAction));
            }

            var builder = new HealthCheckerBuilder();
            healthCheckerBuilderAction(builder);

            // We add it as a singleton to that we are able to use it as part of the middleware pipeline
            return services.AddSingleton<IHealthChecker>(sp => builder.Build());
        }

    }
}
