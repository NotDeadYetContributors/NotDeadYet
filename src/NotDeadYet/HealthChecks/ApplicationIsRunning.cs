namespace NotDeadYet.HealthChecks
{
    public class ApplicationIsRunning : IHealthCheck
    {
        public string Description
        {
            get { return "Checks whether the application is running. If this check can run then it should pass."; }
        }

        public void Check()
        {
            // Nothing to see here...
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}