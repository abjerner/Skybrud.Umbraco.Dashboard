using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Dashboard.Extensions.JObject;
using Skybrud.Umbraco.Dashboard.Models.Json;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Selection {
    
    /// <summary>
    /// Class describing the Google user of an Analytics site selection.
    /// </summary>
    public class DashboardAnalyticsSiteSelectionUser : DashboardJsonObject {

        #region Properties

        /// <summary>
        /// Gets the ID of the Google user.
        /// </summary>
        public string Id { get; internal set; }

        #endregion

        #region Constructors

        internal DashboardAnalyticsSiteSelectionUser(JObject obj) : base(obj) {
            Id = obj.GetString("id");
        }

        #endregion

        #region Static methods

        public static DashboardAnalyticsSiteSelectionUser Parse(JObject obj) {
            return obj == null ? null : new DashboardAnalyticsSiteSelectionUser(obj);
        }

        #endregion

    }

}