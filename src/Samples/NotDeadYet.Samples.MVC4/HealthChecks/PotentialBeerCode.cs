using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotDeadYet.Samples.MVC4.HealthChecks
{
    public class PotentialBeerCode : IHealthCheck
    {
        public string Description => "This check will warn on a Friday afternoon and fail after 6pm on a Friday.";

        public void Check()
        {
            if (DateTimeOffset.Now.DayOfWeek != DayOfWeek.Friday) return;

            if (DateTimeOffset.Now.TimeOfDay > TimeSpan.FromHours(18))
            {
                throw new HealthCheckFailedException("It's Friday Evening.");
            }

            if (DateTimeOffset.Now.TimeOfDay > TimeSpan.FromHours(12))
            {
                throw new HealthCheckWarningException("It's Friday Afternoon.");
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}