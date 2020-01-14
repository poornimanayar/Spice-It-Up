using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    public class DialogDirective : IDirective
    {
        [JsonProperty("type")]
        public string Type {get; set;}
    }
}
