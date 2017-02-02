using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.Dashboard.Models.Json;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Selection {
    
    public class DashboardAnalyticsSiteSelectionWebProperty : DashboardJsonObject {

        #region Properties

        /// <summary>
        /// Gets the ID of the web property.
        /// </summary>
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the name of the web property.
        /// </summary>
        public string Name { get; internal set; }

        #endregion

        #region Constructors

        internal DashboardAnalyticsSiteSelectionWebProperty(JObject obj) : base(obj) {
            Id = obj.GetString("id");
            Name = obj.GetString("name");
        }

        #endregion

        #region Static methods

        public static DashboardAnalyticsSiteSelectionWebProperty Parse(JObject obj) {
            return obj == null ? null : new DashboardAnalyticsSiteSelectionWebProperty(obj);
        }

        #endregion

    }

}