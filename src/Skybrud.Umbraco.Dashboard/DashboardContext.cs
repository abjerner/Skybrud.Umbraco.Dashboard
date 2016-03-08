using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Skybrud.Social.Google.Analytics.Interfaces;
using Skybrud.Umbraco.Dashboard.Config;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Plugins;
using Skybrud.Umbraco.Dashboard.Plugins.Umbraco;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using Umbraco.Core;

namespace Skybrud.Umbraco.Dashboard {
    
    /// <summary>
    /// Class that keeps track of the dashboard during a given request.
    /// </summary>
    public class DashboardContext {

        #region Private fields

        private static readonly DashboardPluginCollection PluginCollection = new DashboardPluginCollection {
            new UmbracoDashboardPlugin()
        };

        private CultureInfo _cultureInfo;
        private UrlHelper _helper;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the current dashboard context.
        /// </summary>
        public static DashboardContext Current {
            get {
                DashboardContext current = HttpContext.Current.Items["DashboardContext.Current"] as DashboardContext;
                if (current == null) HttpContext.Current.Items["DashboardContext.Current"] = current = new DashboardContext();
                return current;
            }
        }

        /// <summary>
        /// Gets a reference to an URL helper. This helper can eg. be used to resolve the URLs of
        /// WebApi controllers.
        /// </summary>
        public UrlHelper UrlHelper {
            get { return _helper ?? (_helper = new UrlHelper(HttpContext.Current.Request.RequestContext)); }
        }

        /// <summary>
        /// Gets a collection of all dashboard plugins. Even though the <code>DashboardContext</code>
        /// only lives across a single, this property will live during the lifetime of the
        /// application.
        /// </summary>
        public DashboardPluginCollection Plugins {
            get { return PluginCollection; }
        }

        /// <summary>
        /// Gets or sets the culture to be used in the dashboard.
        /// </summary>
        public CultureInfo Culture {
            get { return _cultureInfo ?? new CultureInfo("da-DK"); }
            set { _cultureInfo = value ?? new CultureInfo("da-DK"); }
        }

        /// <summary>
        /// Gets a reference to the Dashboard configuration.
        /// </summary>
        public DashboardConfiguration Configuration { get; private set; }

        #endregion

        #region Constructors

        private DashboardContext() {
            Configuration = DashboardConfiguration.Load(MapPath("~/Config/Skybrud.Dashboard.json"));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the base URL of a WebApi controller of type <code>T</code>.
        /// </summary>
        /// <typeparam name="T">The type of the controller.</typeparam>
        public string GetServiceUrl<T>() where T : UmbracoApiController {
            return GetServiceUrl<T>("ActionName").TrimEnd("ActionName");
        }

        /// <summary>
        /// Gets the URL of an action of a WebApi controller of type <code>T</code>.
        /// </summary>
        /// <typeparam name="T">The type of the controller.</typeparam>
        /// <param name="actionName">The name of the action.</param>
        public string GetServiceUrl<T>(string actionName) where T : UmbracoApiController {
            return UrlHelper.GetUmbracoApiService<T>(actionName);
        }

        #region i18n

        /// <summary>
        /// Formats <code>value</code> using the specified <code>culture</code>. If <code>culture</code> is <code>NULL</code>, the default culture is used instead.
        /// </summary>
        /// <param name="value">The value to be formatted.</param>
        /// <param name="culture">The culture to be used for formatting the value.</param>
        public string Format(int value, CultureInfo culture = null) {
            return value.ToString("N0", culture ?? Culture);
        }

        /// <summary>
        /// Formats <code>value</code> using the specified <code>culture</code>. If <code>culture</code> is <code>NULL</code>, the default culture is used instead.
        /// </summary>
        /// <param name="value">The value to be formatted.</param>
        /// <param name="culture">The culture to be used for formatting the value.</param>
        /// <param name="decimals">The amount of decimals to be shown in the formatted text.</param>
        public string Format(double value, CultureInfo culture = null, int decimals = 2) {
            return value.ToString("N" + decimals, culture ?? Culture);
        }

        /// <summary>
        /// Translates the phrase with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the phrase.</param>
        /// <param name="args">The arguments to be inserted into the phrase.</param>
        public string Translate(string key, params string[] args) {
            return DashboardHelpers.Translate(Culture, key, args);
        }

        /// <summary>
        /// Translates the phrase matching the name of the specified <code>field</code>.
        /// </summary>
        /// <param name="field">The field.</param>
        public string Translate(IAnalyticsField field) {
            return DashboardHelpers.Translate(Culture, field);
        }

        #endregion

        /// <summary>
        /// Gets an array of all sites that should be shown in the dashboard.
        /// </summary>
        public IDashboardSite[] GetSites() {

            // Temporary collection for storing the sites
            List<IDashboardSite> sites = new List<IDashboardSite>();

            // Get the sites specified by each resolver
            foreach (IDashboardPlugin plugin in Plugins) {

                try {

                    plugin.GetSites(sites);

                } catch (Exception ex) {

                    LogHelper.Error<DashboardContext>("Plugin of type " + plugin.GetType() + " has failed for GetSites()", ex);

                }

            }

            return sites.ToArray();

        }

        /// <summary>
        /// Gets the site with the specified <code>siteId</code>, or <code>null</code> if the site couldn't be found.
        /// </summary>
        /// <param name="siteId">The ID of the site.</param>
        public IDashboardSite GetSiteById(int siteId) {

            // Get the sites specified by each resolver
            foreach (IDashboardPlugin plugin in Plugins) {

                try {

                    IDashboardSite site = plugin.GetSiteById(siteId);
                    if (site != null) return site;

                } catch (Exception ex) {

                    LogHelper.Error<DashboardContext>("Plugin of type " + plugin.GetType() + " has failed for GetSiteById(" + siteId + ")", ex);

                }

            }

            return null;

        }

        /// <summary>
        /// Gets the page with the specified <code>pageId</code>, or <code>null</code> if the page couldn't be found.
        /// </summary>
        /// <param name="pageId">The ID of the page.</param>
        public IDashboardPage GetPageById(int pageId) {

            foreach (IDashboardPlugin plugin in Plugins) {

                try {

                    IDashboardPage page = plugin.GetPageById(pageId);

                    if (page != null) return page;

                } catch (Exception ex) {

                    LogHelper.Error<DashboardContext>("Plugin of type " + plugin.GetType() + " has failed for GetPageById()", ex);

                }

            }

            return null;

        }

        /// <summary>
        /// Runs a number of setup steps in order for the Dashboard to work properly.
        /// </summary>
        public object Setup() {
            
            List<object> result = new List<object>();

            // Make sure we have a temporary directive for the Dashboard
            string dashboardTempPath = MapPath(DashboardConstants.DashboardCachePath);
            if (Directory.Exists(dashboardTempPath)) {
                result.Add("Global cache directory already exists");
            } else {
                Directory.CreateDirectory(dashboardTempPath);
                result.Add("Global cache directory successfully created");
            }

            // Make sure we have a temporary directive specific to Analytics
            string analyticsTempPath = MapPath(DashboardConstants.AnalyticsCachePath);
            if (Directory.Exists(analyticsTempPath)) {
                result.Add("Analytics cache directory already exists");
            } else {
                Directory.CreateDirectory(analyticsTempPath);
                result.Add("Analytics cache directory successfully created");
            }

            return result;

        }

        /// <summary>
        /// Gets the absolute path based on the specified <code>virtualPath</code>.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Returns the absolute path.</returns>
        public string MapPath(string virtualPath) {
            return HostingEnvironment.MapPath(virtualPath);
        }

        #endregion

    }

}