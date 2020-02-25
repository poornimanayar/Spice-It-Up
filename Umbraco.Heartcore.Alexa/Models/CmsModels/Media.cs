using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.Models.CmsModels
{
 public class Media
    {
       
        [JsonProperty("_url")]
        public string Url { get; set; }
        
    }
}
