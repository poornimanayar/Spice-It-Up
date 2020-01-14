using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.RequestViewModels
{
    public class Application
    {
        [JsonProperty("applicationID")]
        public string ApplicationId {get; set;}
    }
}
