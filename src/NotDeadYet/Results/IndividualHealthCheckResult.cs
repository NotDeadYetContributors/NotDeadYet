using System;

namespace NotDeadYet.Results
{
    public abstract class IndividualHealthCheckResult
    {
        private readonly string _description;
        private readonly TimeSpan _elapsedTime;
        private readonly string _name;

        protected IndividualHealthCheckResult(string name, string description, TimeSpan elapsedTime)
        {
            _name = name;
            _description = description;
            _elapsedTime = elapsedTime;
        }

        public abstract HealthCheckStatus Status { get; }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
        }
    }
}