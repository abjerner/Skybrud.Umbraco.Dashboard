using System;
using System.Linq;
using Newtonsoft.Json;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics.Blocks {
    
    public class VisitsBlock : AnalyticsChildBlock {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }
            
        public VisitsBlock() : base("AnalyticsVisits") {
            Title = DashboardContext.Current.Translate("analytics_title_visits");
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
                    AnalyticsMetric.Visits,
                    AnalyticsMetric.Pageviews,
                    AnalyticsMetric.NewVisits,
                    AnalyticsMetric.AvgTimeOnSite
                },
                Sorting = new AnalyticsSortOptions().AddDescending(AnalyticsMetric.Visits)
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
                    query.FormatVisitDataInt32(AnalyticsMetric.Visits, row1, row2),
                    query.FormatVisitDataInt32(AnalyticsMetric.Pageviews, row1, row2),
                    query.FormatVisitDataInt32(AnalyticsMetric.NewVisits, row1, row2),
                    query.FormatVisitDataTime(AnalyticsMetric.AvgTimeOnSite, row1, row2)
                }
            };

        }
    
    }

}