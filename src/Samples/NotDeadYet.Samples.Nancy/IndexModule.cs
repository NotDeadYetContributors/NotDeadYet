using Nancy;

namespace NotDeadYet.Samples.Nancy
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => "Hello, world! You might be interested in the /healthcheck endpoint";
        }
    }
}