using System.Collections.Generic;
using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    public class Response
    {
        [JsonProperty("outputSpeech", NullValueHandling = NullValueHandling.Ignore)]
        public OutputSpeech OutputSpeech {get; set;}

        [JsonProperty("card", NullValueHandling = NullValueHandling.Ignore)]
        public Card Card {get; set;}

        [JsonProperty("reprompt", NullValueHandling = NullValueHandling.Ignore)]
        public Reprompt Reprompt {get; set;}

        [JsonProperty("shouldEndSession")]
        public bool ShouldEndSession {get; set;}

        [JsonProperty("directives", NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDirective> Directives {get; set;} = new List<IDirective>();
    }
}

