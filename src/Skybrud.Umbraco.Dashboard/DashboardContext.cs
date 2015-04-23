using System.Web;
using System.Web.Mvc;
using Skybrud.Umbraco.Dashboard.Plugins;
using Skybrud.Umbraco.Dashboard.Plugins.Umbraco;
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

        #endregion

        #region Constructors

        private DashboardContext() { }

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

        #endregion

    }

}