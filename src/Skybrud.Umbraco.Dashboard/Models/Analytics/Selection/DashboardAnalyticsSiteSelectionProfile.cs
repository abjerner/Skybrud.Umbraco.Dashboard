using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.Dashboard.Models.Json;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Selection {
    
    public class DashboardAnalyticsSiteSelectionProfile : DashboardJsonObject {

        #region Properties

        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Currency { get; internal set; }
        public string Timezone { get; internal set; }
        public string WebsiteUrl { get; internal set; }
        public string Type { get; internal set; }

        #endregion

        #region Constructors

        internal DashboardAnalyticsSiteSelectionProfile(JObject obj) : base(obj) {
            Id = obj.GetString("id");
            Name = obj.GetString("name");
            Currency = obj.GetString("currency");
            Timezone = obj.GetString("timezone");
            WebsiteUrl = obj.GetString("websiteUrl");
            Type = obj.GetString("type");
        }

        #endregion

        #region Static methods

        public static DashboardAnalyticsSiteSelectionProfile Parse(JObject obj) {
            return obj == null ? null : new DashboardAnalyticsSiteSelectionProfile(obj);
        }

        #endregion

    }

}