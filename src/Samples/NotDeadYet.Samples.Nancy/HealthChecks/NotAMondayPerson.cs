using System;

namespace NotDeadYet.Samples.Nancy.HealthChecks
{
    public class NotAMondayPerson : IHealthCheck
    {
        public string Description
        {
            get { return "This check will fail on Mondays. Nobody likes Mondays."; }
        }

        public void Check()
        {
            if (DateTimeOffset.Now.DayOfWeek == DayOfWeek.Monday) throw new HealthCheckFailedException("I'm just not a Monday person.");
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