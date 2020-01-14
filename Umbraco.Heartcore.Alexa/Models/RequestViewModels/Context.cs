using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.RequestViewModels
{
    public class Context
    {
        [JsonProperty("System")]
        public SystemObject System {get; set;}

//   [JsonProperty("AudioPlayer")]
//   public AudioPlayer AudioPlayer {get; set;}
    }
}
