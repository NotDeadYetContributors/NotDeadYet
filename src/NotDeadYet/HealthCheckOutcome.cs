using NotDeadYet.Results;

namespace NotDeadYet
{
    public class HealthCheckOutcome
    {
        private readonly string _message;
        private readonly IndividualHealthCheckResult[] _results;
        private readonly HealthCheckStatus _status;

        public HealthCheckOutcome(HealthCheckStatus status, string message, IndividualHealthCheckResult[] results)
        {
            _status = status;
            _message = message;
            _results = results;
        }

        public HealthCheckStatus Status
        {
            get { return _status; }
        }

        public IndividualHealthCheckResult[] Results
        {
            get { return _results; }
        }

        public string Message
        {
            get { return _message; }
        }
    }
}