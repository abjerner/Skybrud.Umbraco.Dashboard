using System;
using System.Collections.Generic;
using System.Web.Http;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Model;
using Skybrud.WebApi.Json;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Dashboard.Controllers {
    
    [PluginController("Skybrud")]
    [JsonOnlyConfiguration]
    public class DashboardController : UmbracoAuthorizedApiController {

        [HttpGet]
        public object GetDashboard(string section) {

            DashboardHelpers.EnsureCurrentUserCulture();

            // Initialize a new list for the tabs
            List<DashboardTab> tabs = new List<DashboardTab>();

            // Iterate through each of the added plugins
            foreach (IDashboardPlugin plugin in DashboardContext.Current.Plugins) {
                try {
                    plugin.GetDashboard(section, tabs);
                } catch (Exception ex) {
                    LogHelper.Error<DashboardController>("Plugin of type " + plugin.GetType() + " has failed for GetDashboard()", ex);
                }
            }

            // Update the "Id" and "IsActive" properties
            int i = 0;
            foreach (DashboardTab tab in tabs) {
                tab.IsActive = (i == 0);
                tab.Id = i++;
            }

            // Return the list and let JSON.net do the magic
            return tabs;

        }

    }

}