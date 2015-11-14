using System;

namespace NotDeadYet.Results
{
    public class FailedIndividualHealthCheckResult : IndividualHealthCheckResult
    {
        private readonly string _failureReason;

        internal FailedIndividualHealthCheckResult(string name, string description, string failureReason, TimeSpan elapsedTime) : base(name, description, elapsedTime)
        {
            _failureReason = failureReason;
        }

        public override HealthCheckStatus Status
        {
            get { return HealthCheckStatus.NotOkay; }
        }

        public string FailureReason
        {
            get { return _failureReason; }
        }
    }
}