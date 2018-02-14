using System;
using System.Reflection;
using NotDeadYet.Results;

namespace NotDeadYet
{
    public class HealthCheckOutcome
    {
        private readonly string _message;
        private readonly IndividualHealthCheckResult[] _results;
        private readonly DateTimeOffset _timestamp;
        private readonly HealthCheckStatus _status;

        public HealthCheckOutcome(HealthCheckStatus status, string message, IndividualHealthCheckResult[] results, DateTimeOffset timestamp)
        {
            _status = status;
            _message = message;
            _results = results;
            _timestamp = timestamp;
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

        public DateTimeOffset Timestamp
        {
            get { return _timestamp; }
        }

        public string NotDeadYet
        {
            get
            {
#if NETSTANDARD1_6
                return typeof(HealthCheckOutcome).GetTypeInfo().Assembly.GetName().Version.ToString();
#else
                return typeof(HealthCheckOutcome).Assembly.GetName().Version.ToString();
#endif
            }
        }
    }
}