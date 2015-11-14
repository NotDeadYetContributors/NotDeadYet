using System;

namespace NotDeadYet.Configuration
{
    public static class HealthCheckerConfiguration
    {
        public delegate IHealthCheck[] GetHealthChecks();

        public delegate void LogError(Exception ex, string message);
    }
}