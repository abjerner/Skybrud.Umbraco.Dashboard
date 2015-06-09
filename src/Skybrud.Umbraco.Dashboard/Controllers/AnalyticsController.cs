using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Skybrud.Umbraco.Dashboard.Exceptions;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Model.Analytics;
using Skybrud.WebApi.Json;
using Skybrud.WebApi.Json.Meta;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Dashboard.Controllers {

    [PluginController("SkybrudDashboard")]
    [JsonOnlyConfiguration]
    public class AnalyticsController : UmbracoAuthorizedApiController {

        [HttpGet]
        public object GetSiteData(int siteId, string period, bool cache = true) {

            try {

                // Get the site
                IDashboardSite site = DashboardContext.Current.GetSiteById(siteId);
                if (site == null) return JsonMetaResponse.GetError(HttpStatusCode.NotFound, "A site with the specified ID couldn't be found.");

                // Get analytics information
                IAnalyticsSite analytics = site as IAnalyticsSite;
                if (analytics == null || !analytics.HasAnalytics) return JsonMetaResponse.GetError(HttpStatusCode.NotFound, "The specified site doesn't have any settings for Google Analytics.");
                
                // Parse the culture (if specified)
                if (HttpContext.Current.Request.QueryString["culture"] != null) {
                    DashboardContext.Current.Culture = new CultureInfo(HttpContext.Current.Request.QueryString["culture"]);
                }

                // Build the query
                DataQuery query = DataQuery.GetFromPeriod(analytics, period, cache);
                query.Type = DataQueryType.Site;

                // Generate the response object
                DataResponse res = query.GetResponse();

                // Return a nice JSON response
                return JsonMetaResponse.GetSuccess(res);


            } catch (DashboardException ex) {

                return JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, ex.Title + ": " + ex.Message);

            } catch (Exception ex) {

                return JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, "Oopsie (" + ex.GetType() + "): " + ex.Message);

            }

        }

    }

}