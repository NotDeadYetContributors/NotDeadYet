using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NotDeadYet
{
    [Serializable]
    public class HealthCheckWarningException : Exception
    {
        public HealthCheckWarningException()
        {
        }

        public HealthCheckWarningException(string message) : base(message)
        {
        }

        public HealthCheckWarningException(string message, Exception inner) : base(message, inner)
        {
        }

        protected HealthCheckWarningException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
