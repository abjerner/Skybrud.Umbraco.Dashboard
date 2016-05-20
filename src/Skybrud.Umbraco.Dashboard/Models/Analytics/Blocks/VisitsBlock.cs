using System.Linq;
using Newtonsoft.Json;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Blocks {
    
    /// <summary>
    /// Class representing a block with basic visits statistics from Google Analytics.
    /// </summary>
    public class VisitsBlock : AnalyticsChildBlock {

        /// <summary>
        /// Gets or sets the title of the block.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets a list of the items of the block.
        /// </summary>
        [JsonProperty("items")]
        public object[] Items { get; set; }
        
        /// <summary>
        /// Initializea a new instance with default options.
        /// </summary>
        public VisitsBlock() : base("AnalyticsVisits") {
            Title = DashboardContext.Current.Translate("dashboard/analyticsVisitsTitle");
        }

        /// <summary>
        /// Fetches data for the "Visits" block. Calling this method will
        /// result on two calls (it not already cached) to the Analytics API.
        /// The first call will fetch data for the previous period, while the
        /// second call will fetch data for the current period.
        /// </summary>
        public static VisitsBlock GetBlock(DataQuery query) {

            // Declare the data options
            AnalyticsDataOptions options = new AnalyticsDataOptions {
                Metrics = new[] {
                    AnalyticsMetric.Sessions,
                    AnalyticsMetric.Pageviews,
                    AnalyticsMetric.NewUsers,
                    AnalyticsMetric.AvgSessionDuration
                },
                Sorting = new AnalyticsSortOptions().AddDescending(AnalyticsMetric.Sessions)
            };

            switch (query.Type) {
                case DataQueryType.Page:
                    options.Filters = query.CreateFilterOptionsFromPageUrls();
                    break;
            }

            // Fetch the data
            AnalyticsDataResponse data1;
            AnalyticsDataResponse data2;
            query.GetCachedDataPreviousAndCurrent("Visits", out data1, out data2, options);

            // Get the first row of each dataset
            var row1 = data1.Rows.FirstOrDefault();
            var row2 = data2.Rows.FirstOrDefault();

            return new VisitsBlock {
                HasData = (row1 != null && row1.GetInt32(AnalyticsMetric.Pageviews) > 0) || (row2 != null && row2.GetInt32(AnalyticsMetric.Pageviews) > 0),
                Items = new[] {
                    query.FormatVisitDataInt32(AnalyticsMetric.Sessions, row1, row2),
                    query.FormatVisitDataInt32(AnalyticsMetric.Pageviews, row1, row2),
                    query.FormatVisitDataInt32(AnalyticsMetric.NewUsers, row1, row2),
                    query.FormatVisitDataTime(AnalyticsMetric.AvgSessionDuration, row1, row2)
                }
            };

        }
    
    }

}