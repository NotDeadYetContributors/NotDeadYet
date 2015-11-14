using System;

namespace NotDeadYet.Configuration
{
    public class DefaultLoggingStrategy
    {
        public void LogError(Exception ex, string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(ex.ToString());
        }
    }
}