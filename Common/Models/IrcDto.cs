using Newtonsoft.Json;

namespace Common.Models
{
    public class IrcDto
    {
        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
