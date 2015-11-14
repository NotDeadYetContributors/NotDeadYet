namespace NotDeadYet.HealthChecks
{
    public class AppPoolIsOnline : IHealthCheck
    {
        public string Description
        {
            get { return "Checks whether the application pool is online. If this check can run then it should pass."; }
        }

        public void Check()
        {
            // Nothing to see here...
        }
    }
}