using System;
using System.Runtime.Serialization;

namespace NotDeadYet
{
    [Serializable]
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

        protected HealthCheckFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}