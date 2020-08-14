using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Umbraco.Headless.Client.Net.Delivery;
using Umbraco.Heartcore.Alexa.RequestViewModels;
using Umbraco.Heartcore.Alexa.ResponseViewModels;
using Media = Umbraco.Heartcore.Alexa.Models.CmsModels.Media;

namespace Umbraco.Heartcore.Alexa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpiceController : ControllerBase
    {
        private IConfiguration _configuration;
        static Random random = new Random();
        public SpiceController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        [HttpPost]
        [Route("spicefacts")]
        public async Task<SkillResponse> GetSpiceFactsAsync([FromBody] SkillRequest request)
        {
            SkillResponse skillResponse = new SkillResponse
            {

                Version = AlexaConstants.AlexaVersion,
                SessionAttributes = new Dictionary<string, object>() { { "locale", "en_GB" } }
            };

            switch (request.Request.Type)
            {
                case "LaunchRequest":
                    //handler for launchrequest intent type
                    skillResponse.Response = await this.LaunchRequestHandlerAsync();
                    break;
                case "IntentRequest":
                    //handler for intentrequest intent type
                    skillResponse.Response = await this.IntentRequestHandlerAsync(request);
                    break;
            }

            return skillResponse;
        }

        private async Task<Response> LaunchRequestHandlerAsync()
        {
            // get content delivery service
            var service = this.GetContentDeliveryService();
            // get the spice facts container
            var items = await service.Content.GetByType("SpiceFactsContainer");
            var spiceContainer = items.Content.Items.FirstOrDefault();

            // get the default media value from spice facts container node
            var media = this.GetMedia(spiceContainer.Properties["defaultImage"] as JObject);
            var response = new Response
            {
                OutputSpeech = new OutputSpeech() // what end user hears
                {

                    // get the welcome message text from spice facts container node.
                    //adding some SSML (Speech Synthesis Markup Language) goodness with a speechcon(words pronounced more expressively)
                    //best practice to have a pause using a punctuation
                    Ssml = @$"<speak> <say-as interpret-as=""interjection"">Ola!</say-as><break time=""1s""/>{spiceContainer.Properties["welcomeMessage"]}</speak>",
                    Type = "SSML"
                },
                Reprompt = new Reprompt()
                {
                    OutputSpeech = new OutputSpeech()
                    {
                        Text = spiceContainer.Properties["welcomeRepromptMessage"].ToString(),
                        Type = "SSML",
                        Ssml = $@"<speak>{spiceContainer.Properties["welcomeRepromptMessage"]}</speak>",
                    }
                },
                Card = new Card() // what end user sees on Alexa devices with screen
                {
                    Title = spiceContainer.Properties["alexaSkillName"].ToString(), // get the value of alexaSkillName property from the node
                    Text = spiceContainer.Properties["welcomeMessage"].ToString(),
                    Type = "Standard",
                    Image = new CardImage()
                    {
                        LargeImageUrl = media.Url,
                        SmallImageUrl = media.Url
                    }

                },
                ShouldEndSession = false
            };
            return response;
        }

        private async Task<Response> IntentRequestHandlerAsync(SkillRequest request)
        {
            Response response = null;
            switch (request.Request.Intent.Name)
            {
                case "SpiceIntent":
                case "AMAZON.FallbackIntent":
                    response = await this.SpiceIntentHandlerAsync();
                    break;
                case "AMAZON.CancelIntent": //handling built-in intents
                case "AMAZON.StopIntent":
                    response = await this.CancelOrStopIntentHandler();
                    break;
                case "AMAZON.HelpIntent": //handling built-in intents
                    response = await this.HelpIntentHandler();
                    break;
                default:
                    response = await this.SpiceIntentHandlerAsync();
                    break;
            }

            return response;
        }

        private async Task<Response> SpiceIntentHandlerAsync()
        {
            // get content delivery service
            var service = this.GetContentDeliveryService();

            // get the spice facts container
            var items = await service.Content.GetByType("SpiceFactsContainer");
            var spiceContainer = items.Content.Items.FirstOrDefault();

            // get the default media value from spice facts container node
            var media = this.GetMedia(spiceContainer.Properties["defaultImage"] as JObject);

            // get all spice facts and choose a random node
            var spiceFactItems = await service.Content.GetByType("SpiceFact");
            var next = random.Next(spiceFactItems.Content.Items.Count());
            var item = spiceFactItems.Content.Items.ElementAt(next);

            var response = new Response
            {
                OutputSpeech = new OutputSpeech() // what end user hears
                {
                    //adding some SSML (Speech Synthesis Markup Language) goodness with an emotion and a break
                    Ssml = $@"<speak> <say-as interpret-as=""interjection"">ooh la la, here is a fact about spices! </say-as><break time=""1s""/><amazon:emotion name=""excited"" intensity=""high"">{item.Properties["fact"]}</amazon:emotion></speak>",// get the value of the "fact" property from the node and serve it up as SSML
                    Type = "SSML"
                },

                Card = new Card() // what end user hears
                {
                    Title = spiceContainer.Properties["alexaSkillName"].ToString(), //Skill name from the Spice Facts Container node
                    Text = item.Properties["fact"].ToString(), //the fact from the selected spice fact node
                    Type = "Standard",
                    Image = new CardImage()
                    {
                        LargeImageUrl = media.Url,
                        SmallImageUrl = media.Url
                    }

                },
                ShouldEndSession = false
            };
            return response;
        }

        private async Task<Response> CancelOrStopIntentHandler()
        {
            var service = this.GetContentDeliveryService();
            var items = await service.Content.GetByType("SpiceFactsContainer");
            var spiceContainer = items.Content.Items.FirstOrDefault();
            var media = this.GetMedia(spiceContainer.Properties["defaultImage"] as JObject);
            var response = new Response
            {
                OutputSpeech = new OutputSpeech()
                {
                    //adding some SSML (Speech Synthesis Markup Language) goodness with an applause at the end
                    Ssml = @$"<speak><amazon:emotion name=""disappointed"" intensity=""medium"">{spiceContainer.Properties["stopMessage"]} <audio src=""soundbank://soundlibrary/human/amzn_sfx_crowd_applause_01""/></amazon:emotion></speak>",
                    Type = "SSML"
                },
                Card = new Card()
                {
                    Title = spiceContainer.Properties["alexaSkillName"].ToString(),
                    Text = spiceContainer.Properties["stopMessage"].ToString(),
                    Type = "Standard",
                    Image = new CardImage()
                    {
                        LargeImageUrl = media.Url,
                        SmallImageUrl = media.Url
                    }

                },
                ShouldEndSession = true
            };
            return response;
        }

        private async Task<Response> HelpIntentHandler()
        {
            var service = this.GetContentDeliveryService();
            var items = await service.Content.GetByType("SpiceFactsContainer");
            var spiceContainer = items.Content.Items.FirstOrDefault();
            var media = this.GetMedia(spiceContainer.Properties["defaultImage"] as JObject);
            var response = new Response
            {
                OutputSpeech = new OutputSpeech()
                {
                    Ssml = this.SsmlDecorate(spiceContainer.Properties["helpMessage"].ToString()),
                    Type = "SSML"
                },
                Reprompt = new Reprompt()
                {
                    OutputSpeech = new OutputSpeech()
                    {
                        Text = spiceContainer.Properties["helpMessage"].ToString(),
                        Type = "SSML",
                        Ssml = this.SsmlDecorate(spiceContainer.Properties["helpMessage"].ToString())
                    }
                },
                Card = new Card()
                {
                    Title = spiceContainer.Properties["alexaSkillName"].ToString(),
                    Text = spiceContainer.Properties["helpMessage"].ToString(),
                    Type = "Standard",
                    Image = new CardImage()
                    {
                        LargeImageUrl = media.Url,
                        SmallImageUrl = media.Url
                    }
                },

                ShouldEndSession = false
            };
            return response;
        }

        private ContentDeliveryService GetContentDeliveryService()
        {
            var umbracoConfig = this._configuration.GetSection("Umbraco");
            var projectAlias = umbracoConfig.GetValue<string>("ProjectAlias");
            return new ContentDeliveryService(projectAlias);
        }

        private Media GetMedia(JObject mediaJson)
        {
            return mediaJson.ToObject<Media>();
        }

        private string SsmlDecorate(string speech)
        {
            return "<speak>" + speech + "</speak>";
        }

    }
}