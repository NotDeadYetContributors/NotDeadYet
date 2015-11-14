using System;

namespace NotDeadYet
{
    internal class ImmediateFailHealthCheck : IHealthCheck
    {
        private readonly string _description;
        private readonly Exception _exception;

        public ImmediateFailHealthCheck(string description, Exception exception)
        {
            _description = description;
            _exception = exception;
        }

        public string Description
        {
            get { return _description; }
        }

        public void Check()
        {
            throw new HealthCheckFailedException(_description, _exception);
        }
    }
}