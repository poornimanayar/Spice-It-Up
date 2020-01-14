using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    [JsonObject("reprompt")]
    public class Reprompt
    {
        [JsonProperty("outputSpeech")]
        public OutputSpeech OutputSpeech { get; set; }

    }
}
