using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Skybrud.Social.Google;
using Skybrud.Umbraco.Dashboard.Config;
using Skybrud.Umbraco.Dashboard.Config.Analytics;
using Skybrud.Umbraco.Dashboard.Constants;
using Skybrud.Umbraco.Dashboard.Exceptions;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Models.Analytics;
using Skybrud.Umbraco.Dashboard.Models.Analytics.Cached;
using Skybrud.WebApi.Json;
using Skybrud.WebApi.Json.Meta;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

using Skybrud.Umbraco.Dashboard.Extensions.HttpRequestMessage;

namespace Skybrud.Umbraco.Dashboard.Controllers {

    /// <summary>
    /// WebAPI controller for accessing the Google Analnytics API and related data from within Umbraco.
    /// </summary>
    [PluginController("SkybrudDashboard")]
    [JsonOnlyConfiguration]
    public class AnalyticsController : UmbracoAuthorizedApiController {

        #region Properties

        /// <summary>
        /// Gets a reference to the current instance of <see cref="DashboardConfiguration"/>.
        /// </summary>
        protected DashboardConfiguration Config {
            get { return DashboardContext.Current.Configuration; }
        }

        /// <summary>
        /// Gets whether there is a valid configuration for Google Analytics, which basically means that the
        /// <code>analytics</code> property should be present in the configuration file.
        /// </summary>
        public bool HasAnalytics {
            get { return Config.HasAnalytics; }
        }

        /// <summary>
        /// Gets a reference to an instance of <see cref="AnalyticsConfiguration"/> representing the configuration
        /// specific to Google Analytics. If the <code>analytics</code> property isn't part of the configuration file,
        /// this property will return <code>null</code>.
        /// </summary>
        public AnalyticsConfiguration Analytics {
            get { return Config.Analytics; }
        }

        #endregion

        #region Public API methods

        /// <summary>
        /// Gets statistics from Google Analytics for the site with the specified <paramref name="siteId"/>.
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
        public object GetAccounts(string query = null, bool cache = true) {

            // Validate the configuration
            if (!HasAnalytics) return Request.CreateResponse(DashboardError.AnalyticsNotConfigured);
            if (Analytics.Clients.Length == 0) return Request.CreateResponse(DashboardError.AnalyticsNoClients);
            if (!Analytics.Clients.Any(client => client.Users.Any())) return Request.CreateResponse(DashboardError.AnalyticsNoUsers);

            TimeSpan lifetime = TimeSpan.FromMinutes(30);

            JArray usersArray = new JArray();

            foreach (AnalyticsClientConfiguration client in DashboardContext.Current.Configuration.Analytics.Clients) {

                foreach (AnalyticsUserConfiguration user in client.Users) {

                    string path = DashboardContext.Current.MapPath(DashboardConstants.AnalyticsCachePath + "/Users/" + user.Id + ".json");

                    AnalyticsCachedUser cachedUser;

                    if (cache && System.IO.File.Exists(path) && System.IO.File.GetLastWriteTimeUtc(path) > DateTime.UtcNow.Add(lifetime)) {

                        // Load the user from the disk
                        cachedUser = AnalyticsCachedUser.Load(path);

                    } else {

                        GoogleService service = GoogleService.CreateFromRefreshToken(client.ClientId, client.ClientSecret, user.RefreshToken);

                        var response1 = service.Analytics.Management.GetAccounts();
                        var response2 = service.Analytics.Management.GetWebProperties();
                        var response3 = service.Analytics.Management.GetProfiles();

                        cachedUser = AnalyticsCachedUser.GetFromResponses(user, response1, response2, response3);

                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        cachedUser.Save(path);

                    }
                    
                    // Only include accounts/web properties/profiles that match "query"
                    JArray accountsArray = Search(cachedUser, (query ?? "").ToLower());

                    usersArray.Add(new JObject {
                        {"id", user.Id},
                        {"email", user.Email},
                        {"name", user.Name},
                        {"accounts", accountsArray}
                    });

                }

            }

            return usersArray;

        }

        #endregion

        private JArray Search(AnalyticsCachedUser user, string query) {
            
            JArray accountsArray = new JArray();

            foreach (AnalyticsCachedAccount account in user.Accounts) {

                // Is the account a direct match?
                bool match1 = account.IsMatch(query);

                JArray webPropertiesArray = new JArray();

                foreach (AnalyticsCachedWebProperty property in account.WebProperties) {

                    // Is the web property a direct match?
                    bool match2 = match1 || property.IsMatch(query);

                    JArray profilesArray = new JArray();
                    foreach (AnalyticsCachedProfile profile in property.Profiles) {
                        if (match2 || profile.IsMatch(query)) {
                            profilesArray.Add(JObject.FromObject(profile));
                        }
                    }

                    if (profilesArray.Count > 0) {
                        webPropertiesArray.Add(new JObject {
                            {"id", property.Id},
                            {"name", property.Name},
                            {"profiles", profilesArray}
                        });
                    }

                }

                if (webPropertiesArray.Count > 0) {
                    accountsArray.Add(new JObject {
                        {"id", account.Id},
                        {"name", account.Name},
                        {"webProperties", webPropertiesArray}
                    });
                }

            }

            return accountsArray;

        }

    }

}