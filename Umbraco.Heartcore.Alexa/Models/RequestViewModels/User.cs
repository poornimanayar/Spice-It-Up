using Newtonsoft.Json;

namespace Umbraco.Heartcore.Alexa.RequestViewModels
{
    public class User
    {
        [JsonProperty ("userId")]
        public string UserId {get; set;}

        [JsonProperty("accessToken")]
        public string AccessToken {get; set;}
    }
}
