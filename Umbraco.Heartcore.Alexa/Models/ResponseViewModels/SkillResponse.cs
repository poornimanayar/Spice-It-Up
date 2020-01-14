﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.ResponseViewModels
{
    public class SkillResponse
    {
        [JsonProperty("version")]
        public string Version {get; set;}

        [JsonProperty("sessionAttributes", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> SessionAttributes {get; set;}

        [JsonProperty("response")]
        public Response Response {get; set;}
    }
}
