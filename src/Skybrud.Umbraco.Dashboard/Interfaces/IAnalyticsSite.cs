using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Model.Analytics;

namespace Skybrud.Umbraco.Dashboard.Interfaces {
    
    public interface IAnalyticsSite : IDashboardSite {

        /// <summary>
        /// Gets settings of the site for integration with Google Analytics.
        /// </summary>
        [JsonIgnore]
        DashboardAnalyticsSettings Analytics { get; }

        /// <summary>
        /// Gets whether this site has any settings for integration with Google Analytics.
        /// </summary>
        [JsonProperty("analytics")]
        bool HasAnalytics { get; }
    }

}