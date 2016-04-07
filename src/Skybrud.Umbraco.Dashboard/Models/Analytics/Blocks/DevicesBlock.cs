using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Skybrud.Social;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses;
using Skybrud.Umbraco.Dashboard.Exceptions;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Blocks {
    
    public class DevicesBlock : AnalyticsChildBlock {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }
            
        public DevicesBlock() : base("AnalyticsDevices") {
            Title = DashboardContext.Current.Translate("dashboard/analyticsDevicesTitle");
        }

        private static string FirstCharToUpper(string str) {
            // TODO: Method will be available in Skybrud.Social.Core
            return String.IsNullOrEmpty(str) ? "" : String.Concat(str.Substring(0, 1).ToUpper(), str.Substring(1));
        }

        private static string UnderscoreToUpperCamelCase(string str) {
            // TODO: Method will be available in Skybrud.Social.Core
            return String.IsNullOrWhiteSpace(str) ? str : String.Join("", from piece in str.Split('_') select FirstCharToUpper(piece));
        }

        public static DevicesBlock GetBlock(DataQuery query) {

            // Declare the data options
            var options = new AnalyticsDataOptions {
                StartDate = query.CurrentStartDate,
                EndDate = query.CurrentEndDate,
                Metrics = AnalyticsMetric.Visits + AnalyticsMetric.Pageviews,
                Dimensions = AnalyticsDimension.DeviceCategory
            };

            switch (query.Type) {
                case DataQueryType.Page:
                    options.Filters = query.CreateFilterOptionsFromPageUrls();
                    break;
            }

            // Fetch the data
            AnalyticsDataResponse data;
            try {

                data = query.GetCachedData("Devices", options);

            } catch (Exception ex) {

                throw new DashboardException(ex.Message + " (Unable to fetch date for period from " + query.PreviousStartDate.ToString("yyyy-MM-dd") + " to " + query.PreviousEndDate.ToString("yyyy-MM-dd") + " for block \"Devices\")");

            }

            // Filter out any rows where the the "ga:visits" value is either "NaN" or "0"
            AnalyticsDataRow[] rows = data.Rows.Where(x => x.GetString(AnalyticsMetric.Visits) != "NaN" && x.GetString(AnalyticsMetric.Visits) != "0").ToArray();

            // Calculate the total visits and pageviews
            int totalVisits = 0;
            int totalPageviews = 0;
            foreach (AnalyticsDataRow row in rows) {
                totalVisits += row.GetInt32(AnalyticsMetric.Visits);
                totalPageviews += row.GetInt32(AnalyticsMetric.Pageviews);
            }
            
            if (data.Rows.Length == 0) {
                return new DevicesBlock {
                    HasData = false,
                    Items = new object[0]
                };
            }

            List<object> items = new List<object>();

            // Iterate through the rows
            foreach (AnalyticsDataRow row in rows) {

                int visits = row.GetInt32(AnalyticsMetric.Visits);
                int pageviews = row.GetInt32(AnalyticsMetric.Pageviews);

                double visitsPercent = visits / (double) totalVisits * 100;
                double pageviewsPercent = pageviews / (double) totalPageviews * 100;

                string category = row.GetString(AnalyticsDimension.DeviceCategory);

                items.Add(new {
                    category,
                    text = query.Context.Translate("dashboard/analyticsDevices" + FirstCharToUpper(category)),
                    visits = new {
                        raw = visits,
                        text = query.Context.Format(visits),
                        percent = new {
                            raw = visitsPercent,
                            text = query.Context.Format(visitsPercent) + "%"
                        }
                    },
                    pageviews = new {
                        raw = pageviews,
                        text = DashboardContext.Current.Format(pageviews),
                        percent = new {
                            raw = pageviewsPercent,
                            text = query.Context.Format(pageviewsPercent) + "%"
                        }
                    }
                });

            }
           
            return new DevicesBlock {
                HasData = items.Count > 0,
                Items = items.ToArray()
            };

        }
        
    }

}