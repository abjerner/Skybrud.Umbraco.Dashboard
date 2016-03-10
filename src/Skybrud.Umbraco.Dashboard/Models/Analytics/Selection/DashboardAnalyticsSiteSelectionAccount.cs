using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Dashboard.Extensions.JObject;
using Skybrud.Umbraco.Dashboard.Models.Json;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Selection {
    
    public class DashboardAnalyticsSiteSelectionAccount : DashboardJsonObject {

        #region Properties

        /// <summary>
        /// Gets the ID of the Analytics account.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the name of the Analytics account.
        /// </summary>
        public string Name { get; internal set; }

        #endregion

        #region Constructors

        internal DashboardAnalyticsSiteSelectionAccount(JObject obj) : base(obj) {
            Id = obj.GetString("id");
            Name = obj.GetString("name");
        }

        #endregion

        #region Static methods

        public static DashboardAnalyticsSiteSelectionAccount Parse(JObject obj) {
            return obj == null ? null : new DashboardAnalyticsSiteSelectionAccount(obj);
        }

        #endregion

    }

}