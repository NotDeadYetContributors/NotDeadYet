using System;

namespace NotDeadYet.Results
{
    internal class SuccessfulIndividualHealthCheckResult : IndividualHealthCheckResult
    {
        internal SuccessfulIndividualHealthCheckResult(string name, string description, TimeSpan elapsedTime)
            : base(name, description, elapsedTime)
        {
        }

        public override HealthCheckStatus Status
        {
            get { return HealthCheckStatus.Okay; }
        }
    }
}