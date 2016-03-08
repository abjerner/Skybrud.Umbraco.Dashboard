using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Skybrud.Umbraco.Dashboard.Exceptions;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Model.Analytics;
using Skybrud.WebApi.Json;
using Skybrud.WebApi.Json.Meta;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Dashboard.Controllers {

    [PluginController("SkybrudDashboard")]
    [JsonOnlyConfiguration]
    public class AnalyticsController : UmbracoAuthorizedApiController {

        /// <summary>
        /// Gets statistics from Google Analytics for the site with the specified <code>siteId</code>.
        /// </summary>
        /// <param name="siteId">The ID of the site.</param>
        /// <param name="period">The alias of the period for which statistics should be shown.</param>
        /// <param name="cache">Whether we should attempt to read the statistics from the cache.</param>
        /// <returns></returns>
        [HttpGet]
        public object GetSiteData(int siteId, string period, bool cache = true) {

            DashboardHelpers.EnsureCurrentUserCulture();

            try {

                // Get the site
                IDashboardSite site = DashboardContext.Current.GetSiteById(siteId);
                if (site == null) throw new DashboardException(HttpStatusCode.NotFound, "Site ikke fundet", "Et site det angivne ID blev ikke fundet");

                // Get analytics information
                IAnalyticsSite analytics = site as IAnalyticsSite;
                if (analytics == null || !analytics.HasAnalytics) {
                    throw new DashboardException(HttpStatusCode.InternalServerError, "Analytics", "Det valgte side understøtter eller er ikke konfigureret til visning af statistik fra Google Analytics");
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

                return JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, "Oopsie (" + ex.GetType() + "): " + ex.Message, ex.StackTrace.Split('\n'));

            }

        }


        [HttpGet]
        public object GetPageData(int siteId, int pageId, string period, bool cache = true) {

            DashboardHelpers.EnsureCurrentUserCulture();

            try {

                // Look for the site with the specified ID
                IDashboardSite site = DashboardContext.Current.GetSiteById(siteId);
                if (site == null) throw new DashboardException(HttpStatusCode.NotFound, "Site ikke fundet", "Et site det angivne ID blev ikke fundet");

                // Attempt to cast the site to an Analytics site
                IAnalyticsSite analytics = site as IAnalyticsSite;
                if (analytics == null || !analytics.HasAnalytics) {
                    throw new DashboardException(HttpStatusCode.InternalServerError, "Analytics", "Det valgte side understøtter eller er ikke konfigureret til visning af statistik fra Google Analytics");
                }

                // Get the published content of the page
                IPublishedContent content = UmbracoContext.ContentCache.GetById(pageId);
                
                // Build the query
                DataQuery query = DataQuery.GetFromPeriod(analytics, period, cache);
                query.Type = DataQueryType.Page;
                query.PageId = content.Id;

                // Set the URL for the query. The protocol and domain is stripped so we only have the path
                query.PageUrl = Regex.Replace(content.Url, "^http(s|)://[a-z0-9-.]+/", "/");

                // Google Analytics sees the same URL with and without a trailing slash as two different pages, so we should tell the query to check both
                string pageUrlTrimmed = query.PageUrl.TrimEnd('/');
                string pageUrlSlashed = pageUrlTrimmed + '/';
                query.PageUrls = String.IsNullOrEmpty(pageUrlTrimmed) ? new[] { pageUrlSlashed } : new[] { pageUrlTrimmed, pageUrlSlashed };

                // Generate the response object
                DataResponse res = query.GetResponse();

                // Return a nice JSON response
                return JsonMetaResponse.GetSuccess(res);


            } catch (DashboardException ex) {

                return JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, ex.Title + ": " + ex.Message);

            } catch (Exception ex) {

                return JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, "Oopsie (" + ex.GetType() + "): " + ex.Message, ex.StackTrace.Split('\n'));

            }

        }

        [HttpGet]
        public object GetAccounts(string query = null) {
            


        }

    }

}