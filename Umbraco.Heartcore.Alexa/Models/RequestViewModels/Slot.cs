using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.RequestViewModels
{
    public class Slot
    {
        [JsonProperty("name")]
        public string Name {get; set;}

        [JsonProperty("value")]
        public string Value {get; set;}
    }
}
