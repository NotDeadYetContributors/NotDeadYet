using System;

namespace NotDeadYet
{
    public class HealthCheckFailedException : Exception
    {
        public HealthCheckFailedException()
        {
        }

        public HealthCheckFailedException(string message) : base(message)
        {
        }

        public HealthCheckFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}