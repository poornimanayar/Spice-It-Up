using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    public class CardImage
    {
        [JsonProperty("smallImageUrl")]
        public string SmallImageUrl {get; set;}

        [JsonProperty("largeImageUrl")]
        public string LargeImageUrl {get; set;}
    }
}
