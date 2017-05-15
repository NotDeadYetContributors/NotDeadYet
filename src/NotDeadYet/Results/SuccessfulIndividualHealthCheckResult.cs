using System;

namespace NotDeadYet.Results
{
    public class SuccessfulIndividualHealthCheckResult : IndividualHealthCheckResult
    {
        public SuccessfulIndividualHealthCheckResult(string name, string description, TimeSpan elapsedTime)
            : base(name, description, elapsedTime)
        {
        }

        public override HealthCheckStatus Status
        {
            get { return HealthCheckStatus.Okay; }
        }
    }
}