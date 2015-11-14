using Newtonsoft.Json;

namespace NotDeadYet.Samples.Nancy
{
    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            Formatting = Formatting.Indented;
        }
    }
}