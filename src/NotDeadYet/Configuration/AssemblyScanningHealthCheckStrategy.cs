using System;
using System.Linq;
using System.Reflection;
using ThirdDrawer.Extensions.StringExtensionMethods;
using ThirdDrawer.Extensions.TypeExtensionMethods;

namespace NotDeadYet.Configuration
{
    internal class AssemblyScanningHealthCheckStrategy
    {
        private readonly Assembly[] _assemblies;

        public AssemblyScanningHealthCheckStrategy(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public IHealthCheck[] ScanForHealthChecks()
        {
            return _assemblies
                .SelectMany(a => a.GetExportedTypes())
                .OrderBy(t => t.FullName)
                .Distinct()
                .Where(t => t.IsAssignableTo<IHealthCheck>())
                .Where(t => t.IsInstantiable())
                .Select(InstantiateHealthCheck)
                .ToArray();
        }

        private static IHealthCheck InstantiateHealthCheck(Type healthCheckType)
        {
            try
            {
                return (IHealthCheck) Activator.CreateInstance(healthCheckType);
            }
            catch (Exception ex)
            {
                var description = "The {0} health check type could not be instantiated.".FormatWith(healthCheckType.FullName);
                return new ImmediateFailHealthCheck(description, ex);
            }
        }
    }
}