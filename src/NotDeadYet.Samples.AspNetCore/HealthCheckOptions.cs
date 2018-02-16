namespace NotDeadYet.Samples.AspNetCore
{
    public class HealthCheckOptions
    {
        /// <summary>
        /// Set a custom route for accessing the healthcheck endpoint
        /// </summary>
        public string Path { get; set; } = "/healthcheck";

    }
}
