using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    public class SimpleCard 
    {
        [JsonRequired]
        [JsonProperty("type")]
        public string Type {get { return "Simple"; }}

        [JsonProperty("title")]
        public string Title {get; set;}

        [JsonProperty("content")]
        public string Content {get; set;}
    }
}
