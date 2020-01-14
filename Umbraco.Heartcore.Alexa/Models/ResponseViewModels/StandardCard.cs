using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    public class StandardCard 
    {
        [JsonRequired]
        [JsonProperty("type")]
        public string Type { get { return "Standard"; } }

        [JsonProperty("title")]
        public string Title {get; set;}

        [JsonProperty("text")]
        public string text {get; set;}

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public CardImage Image {get; set;}
    }
}
