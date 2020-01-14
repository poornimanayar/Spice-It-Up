using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    public class Card 
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("image")]
        public CardImage Image { get; set; }
    }
}
