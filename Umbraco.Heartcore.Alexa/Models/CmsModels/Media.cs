using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.Models.CmsModels
{
 public class Media
    {
       
        [JsonProperty("_url")]
        public string Url { get; set; }
        
    }
}
