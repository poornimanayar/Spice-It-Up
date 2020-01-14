using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.RequestViewModels
{
    public class SystemObject
    {
        [JsonProperty("user")]
        public User User {get; set;}

        [JsonProperty("device")]
        public Device Device {get; set;}

        [JsonProperty("apiEndpoint")]
        public string ApiEndpoint {get; set;}
    }
}
