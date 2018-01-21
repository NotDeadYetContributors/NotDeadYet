using System;
using System.Linq;
using System.Reflection;

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
                .Where(t => typeof(IHealthCheck).IsAssignableFrom(t))
                .Select(InstantiateHealthCheck)
                .ToArray();
        }

        private static IHealthCheck InstantiateHealthCheck(Type healthCheckType)
        {
            return (IHealthCheck) Activator.CreateInstance(healthCheckType);
        }
    }
}