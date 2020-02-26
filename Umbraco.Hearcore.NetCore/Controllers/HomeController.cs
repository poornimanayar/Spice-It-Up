using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Umbraco.Headless.Client.Net.Delivery;
using Umbraco.Headless.Client.Net.Management;
using Umbraco.Heartcore.NetCore.Models;

namespace Umbraco.Heartcore.NetCore.Controllers
{
    public class HomeController : Controller
    {
       private IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
           this._configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            //get the Umbraco section from appsettings
            var umbracoConfig = this._configuration.GetSection("Umbraco");

            //get the project alias from the section config obtained above
            var projectAlias = umbracoConfig.GetValue<string>("ProjectAlias");

            //get an instance of the Content Delivery API
            var contentDeliveryService = new ContentDeliveryService(projectAlias);

            //get the content of type spiceFact
            var spiceFacts = await contentDeliveryService.Content.GetByType("spiceFact", null, page: 1, pageSize: 25);

            //pass the content obtained as the model to my View 
            return this.View(spiceFacts);
        }

        /// <summary>
        /// Action to like the spice fact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ThatsSpicy([FromForm]string id)
        {
            //get the Umbraco section from appsettings
            var umbracoConfig = this._configuration.GetSection("Umbraco");

            //get the project alias from the section config obtained above
            var projectAlias = umbracoConfig.GetValue<string>("ProjectAlias");

            //get the api key from the section config obtained above
            var apiKey = umbracoConfig.GetValue<string>("APIKey");

            //get an instance of the Content Management API
            var contentManagementService = new ContentManagementService(projectAlias, apiKey);

            //get the 
            var contentItem = await contentManagementService.Content.GetById(Guid.Parse(id));

            int spicyCounter = contentItem.Properties["spicyCounter"]["$invariant"] == string.Empty ? 0 : Convert.ToInt32(contentItem.Properties["spicyCounter"]["$invariant"]);
            int spicyCounterUpdated = spicyCounter + 1;
            contentItem.SetValue("spicyCounter", spicyCounterUpdated);
            var updateItem = await contentManagementService.Content.Update(contentItem);
            var updatedItem = await contentManagementService.Content.GetById(updateItem.Id);
            var publishedItem = await contentManagementService.Content.Publish(updatedItem.Id);
            
            return this.Json(new { message = "This is a JSON result.", date = DateTime.Now, spiceFactor = spicyCounterUpdated });
        }
        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
