using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Skybrud.Umbraco.Dashboard.Controllers;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.UI.JavaScript;

namespace Skybrud.Umbraco.Dashboard {

    internal class DashboardStartup : ApplicationEventHandler {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {

            // Add server variables for the Dashboard
            ServerVariablesParser.Parsing += ServerVariablesParserOnParsing;

        }

        private void ServerVariablesParserOnParsing(object sender, Dictionary<string, object> e) {

            // Create a new UrlHelper (can we grab the one from the DashboardContext?)
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            // Get the default service URL
            string serviceUrl = url.GetUmbracoApiService<DashboardController>("ActionName").TrimEnd("ActionName");

            // Hijack/replace Umbraco's default dashboard controller
            Dictionary<string, object> umbracoUrls = e["umbracoUrls"] as Dictionary<string, object>;
            if (umbracoUrls != null) umbracoUrls["dashboardApiBaseUrl"] = serviceUrl;

            // Add our own server variables for the dashboard
            e.Add("SkybrudDashboard", new {
                serviceUrl
            });

        }

    }

}