﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Umbraco.Headless.Client.Net.Delivery;
using Umbraco.Headless.Client.Net.Management;
using Umbraco.Headless.Client.Net.Management.Models;
using Umbraco.Hearcore.NetCore.Models;

namespace Umbraco.Hearcore.NetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this._configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var umbracoConfig = this._configuration.GetSection("Umbraco");
            var projectAlias = umbracoConfig.GetValue<string>("ProjectAlias");
            var contentDeliveryService = new ContentDeliveryService(projectAlias);
            var spiceFacts = await contentDeliveryService.Content.GetByType("spiceFact", null, page: 1, pageSize: 25);
            return View(spiceFacts);
        }

        [HttpPost]
        public async Task<IActionResult> ThatsSpicy([FromForm]string id)
        {
            var umbracoConfig = this._configuration.GetSection("Umbraco");
            var projectAlias = umbracoConfig.GetValue<string>("ProjectAlias");
            var apiKey = umbracoConfig.GetValue<string>("APIKey");
            var contentManagementService = new ContentManagementService(projectAlias, apiKey);
            var contentItem = await contentManagementService.Content.GetById(Guid.Parse(id));

            int spicyCounter = contentItem.Properties["spicyCounter"]["$invariant"] == string.Empty ? 0 : Convert.ToInt32(contentItem.Properties["spicyCounter"]["$invariant"]);
            int spicyCounterUpdated = spicyCounter + 1;
            contentItem.SetValue("spicyCounter", spicyCounterUpdated);
            var updateItem = await contentManagementService.Content.Update(contentItem);
            var updatedItem = await contentManagementService.Content.GetById(updateItem.Id);
            var publishedItem = await contentManagementService.Content.Publish(updatedItem.Id);
            
            return Json(new { message = "This is a JSON result.", date = DateTime.Now, spiceFactor = spicyCounterUpdated });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}