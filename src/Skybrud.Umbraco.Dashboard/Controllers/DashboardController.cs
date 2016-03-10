using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Dashboard.Exceptions;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Models;
using Skybrud.Umbraco.Dashboard.Models.LastEdited;
using Skybrud.WebApi.Json;
using Skybrud.WebApi.Json.Meta;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Dashboard.Controllers {
    
    [PluginController("SkybrudDashboard")]
    [JsonOnlyConfiguration]
    public class DashboardController : UmbracoAuthorizedApiController {

        /// <summary>
        /// Runs a number of setup steps in order for the Dashboard to work properly.
        /// </summary>
        [HttpGet]
        public object Setup() {
            return DashboardContext.Current.Setup();
        }

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

        [HttpGet]
        public object GetSite(int siteId, bool blocks = true) {

            try {

                // Look for the site with the specified ID
                IDashboardSite site = DashboardContext.Current.GetSiteById(siteId);

                // Throw an exception if the site wasn't found
                if (site == null) {
                    throw new DashboardException(HttpStatusCode.NotFound, "Det efterspurgte site blev ikke fundet.");
                }

                // Convert to JSON
                JObject obj = JObject.FromObject(site);

                if (blocks) {
                    obj.Add("blocks", JToken.FromObject(site.GetBlocks()));
                }

                return obj;

            } catch (DashboardException ex) {

                return Request.CreateResponse(JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, ex.Message));

            } catch (Exception ex) {

                return Request.CreateResponse(JsonMetaResponse.GetError(
                    HttpStatusCode.InternalServerError,
                    "Unknown server error. Check the log for further information."
                ));

            }

        }

        /// <summary>
        /// Sets the default site of the authenticated user.
        /// </summary>
        /// <param name="siteId">The ID of the site.</param>
        [HttpGet]
        public object SetDefaultSite(int siteId) {

            try {

                // Look for the site with the specified ID
                IDashboardSite site = DashboardContext.Current.GetSiteById(siteId);

                // Throw an exception if the site wasn't found
                if (site == null) {
                    throw new DashboardException(HttpStatusCode.NotFound, "Det efterspurgte site blev ikke fundet.");
                }

                DashboardHelpers.UserSettings.SetDefaultSite(UmbracoContext.Security.CurrentUser, site);
                
                return site;

            } catch (DashboardException ex) {

                return Request.CreateResponse(JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, ex.Message));

            } catch (Exception ex) {

                return Request.CreateResponse(JsonMetaResponse.GetError(
                    HttpStatusCode.InternalServerError,
                    "Unknown server error. Check the log for further information."
                ));

            }

        }

        [HttpGet]
        public object GetPage(int pageId) {

            try {

                // Look for the page with the specified ID
                IDashboardPage page = DashboardContext.Current.GetPageById(pageId);
                if (page == null || page.Content == null) throw new DashboardException("Den pågældende side ser ikke til at være publiceret");

                // Throw an exception if the site wasn't found
                if (page.Site == null) {
                    throw new DashboardException(HttpStatusCode.NotFound, "Det efterspurgte site blev ikke fundet.");
                }

                return new {
                    page,
                    site = page.Site,
                    blocks = page.GetBlocks()
                };

            } catch (DashboardException ex) {

                return Request.CreateResponse(JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, ex.Message));

            } catch (Exception ex) {

                return Request.CreateResponse(JsonMetaResponse.GetError(
                    HttpStatusCode.InternalServerError,
                    "Unknown server error. Check the log for further information."
                ));

            }

        }

        [HttpGet]
        public object GetBlocksForSite(int siteId, int save = 0) {

            try {

                // Look for the site with the specified ID
                IDashboardSite site = DashboardContext.Current.GetSiteById(siteId);

                // Throw an exception if the site wasn't found
                if (site == null) {
                    throw new DashboardException(HttpStatusCode.NotFound, "Det efterspurgte site blev ikke fundet.");
                }

                // Set the active site of the user.
                //if (save == 1) DashboardHelpers.UserSettings.SetCurrentSite(umbraco.helper.GetCurrentUmbracoUser(), site);

                // Get the blocks for the site
                IDashboardBlock[] blocks = site.GetBlocks();

                // Return the blocks
                return new {
                    site,
                    blocks
                };

            } catch (DashboardException ex) {

                LogHelper.Error<DashboardController>("Unable to load blocks for site with ID " + siteId, ex);

                return Request.CreateResponse(JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, ex.Message));

            } catch (Exception ex) {

                LogHelper.Error<DashboardController>("Unable to load blocks for site with ID " + siteId, ex);

                return Request.CreateResponse(JsonMetaResponse.GetError(
                    HttpStatusCode.InternalServerError,
                    "Unknown server error. Check the log for further information."
                ));

            }

        }

        [HttpGet]
        public object GetLastEditedData(int siteId, int max = 5) {

            try {

                // Look for the site with the specified ID
                IDashboardSite site = DashboardContext.Current.GetSiteById(siteId);

                // Throw an exception if the site wasn't found
                if (site == null) {
                    throw new DashboardException(HttpStatusCode.NotFound, "Det efterspurgte site blev ikke fundet.");
                }

                // Get a reference to the current user
                //User user = umbraco.BusinessLogic.User.GetCurrent();

                // Get the last editied pages
                var pages = LastEditedItem.GetLastEditedData(site, null, max);

                // Return a nice JSON response
                return new
                {
                    culture = CultureInfo.CurrentCulture.Name,
                    data = pages,

                };

            } catch (DashboardException ex) {

                LogHelper.Error<DashboardController>("Unable to load last edited pages for site with ID " + siteId, ex);

                return Request.CreateResponse(JsonMetaResponse.GetError(HttpStatusCode.InternalServerError, ex.Message));

            } catch (Exception ex) {

                LogHelper.Error<DashboardController>("Unable to load last edited pages for site with ID " + siteId + ": " + ex.Message, ex);

                return Request.CreateResponse(JsonMetaResponse.GetError(
                    HttpStatusCode.InternalServerError,
                    "Unknown server error. Check the log for further information."
                ));

            }

        }

    }

}