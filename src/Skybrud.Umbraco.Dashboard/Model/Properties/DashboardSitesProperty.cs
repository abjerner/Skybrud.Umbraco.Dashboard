using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Umbraco.Web;

namespace Skybrud.Umbraco.Dashboard.Model.Properties {

    /// <summary>
    /// Dashboard property with statistics about the sites identified by the installed Dashboard plugin.
    /// </summary>
    public class DashboardSitesProperty : DashboardProperty {

        #region Properties

        /// <summary>
        /// Gets the ID of the default site of the current user, or <code>0</code> if the user doesn't yet have a default site.
        /// </summary>
        [JsonProperty("defaultSite", Order = 10)]
        public int DefaultSite { get; private set; }

        /// <summary>
        /// Gets an array of the sites.
        /// </summary>
        [JsonProperty("sites", Order = 10)]
        public object[] Sites { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new sites property.
        /// </summary>
        /// <param name="includeBlocks">Indicates whether the blocks of the site should be included in the output.</param>
        public DashboardSitesProperty(bool includeBlocks = true) : base("DashboardSites") {

            List<object> temp = new List<object>();

            DefaultSite = DashboardHelpers.UserSettings.GetDefaultSiteId(UmbracoContext.Current.Security.CurrentUser);

            foreach (IDashboardSite site in DashboardContext.Current.GetSites()) {

                Dictionary<string, object> obj = JObject.FromObject(site).ToObject<Dictionary<string, object>>();

                if (includeBlocks) {
                    obj.Add("blocks", site.GetBlocks());
                }

                temp.Add(obj);

            }

            Sites = temp.ToArray();


        }

        #endregion

    }

}