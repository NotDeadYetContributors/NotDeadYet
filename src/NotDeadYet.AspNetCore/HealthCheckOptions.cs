namespace NotDeadYet.AspNetCore
{
    public class HealthCheckOptions
    {
        /// <summary>
        /// Set a the path for accessing the healthcheck endpoint
        /// </summary>
        public string Path { get; set; } = "/healthcheck";

    }
}
