using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.Dashboard.Models.Json;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Selection {
    
    /// <summary>
    /// Class describing an Analytics site selection.
    /// </summary>
    public class DashboardAnalyticsSiteSelection : DashboardJsonObject {

        #region Properties

        /// <summary>
        /// Gets a reference to the Google user of the selection.
        /// </summary>
        public DashboardAnalyticsSiteSelectionUser User { get; private set; }

        /// <summary>
        /// Gets a reference to the Analytics account of the selection.
        /// </summary>
        public DashboardAnalyticsSiteSelectionAccount Account { get; private set; }

        /// <summary>
        /// Gets a reference to the web property of the selection.
        /// </summary>
        public DashboardAnalyticsSiteSelectionWebProperty WebProperty { get; private set; }

        /// <summary>
        /// Gets a reference to the Analytics profile of the selection.
        /// </summary>
        public DashboardAnalyticsSiteSelectionProfile Profile { get; private set; }

        /// <summary>
        /// Gets whether the selection has a value.
        /// </summary>
        public bool HasValue {
            get { return Profile != null; }
        }

        #endregion

        #region Constructors

        private DashboardAnalyticsSiteSelection(JObject obj) : base(obj) {
            User = obj.GetObject("user", DashboardAnalyticsSiteSelectionUser.Parse);
            Account = obj.GetObject("account", DashboardAnalyticsSiteSelectionAccount.Parse);
            WebProperty = obj.GetObject("webProperty", DashboardAnalyticsSiteSelectionWebProperty.Parse);
            Profile = obj.GetObject("profile", DashboardAnalyticsSiteSelectionProfile.Parse);
        }

        #endregion

        #region Static methods

        public static DashboardAnalyticsSiteSelection Deserialize(string str) {
            if (str == null || !str.StartsWith("{") || !str.EndsWith("}")) return new DashboardAnalyticsSiteSelection(null);
            return Parse(JsonConvert.DeserializeObject<JObject>(str));
        }

        public static DashboardAnalyticsSiteSelection Parse(JObject obj) {
            return obj == null ? null : new DashboardAnalyticsSiteSelection(obj);
        }

        #endregion

    }

}