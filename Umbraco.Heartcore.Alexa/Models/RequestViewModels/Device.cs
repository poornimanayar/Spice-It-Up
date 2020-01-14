using System.Collections.Generic;
using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.RequestViewModels
{
    public class Device
    {
        [JsonProperty("supportedInterfaces")]
        public Dictionary<string, object> SupportedInterfaces {get; set;}

        [JsonProperty("deviceId")]
        public string DeviceID {get; set;}
    }
}
