using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses;
using Skybrud.Umbraco.Dashboard.Exceptions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Blocks {
    
    public class PopularBlock : AnalyticsChildBlock {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }
            
        public PopularBlock() : base("AnalyticsPopular") {
            Title = DashboardContext.Current.Translate("analytics_title_popular");
        }

        public static PopularBlock GetBlock(DataQuery query, int max) {

            AnalyticsDataResponse data;

            // Fetch the data
            try {

                data = query.GetCachedData("Popular", new AnalyticsDataOptions {
                    StartDate = query.CurrentStartDate,
                    EndDate = query.CurrentEndDate,
                    Metrics = AnalyticsMetric.Visits,
                    Dimensions = AnalyticsDimension.PagePath,
                    Sorting = new AnalyticsSortOptions().AddDescending(AnalyticsMetric.Visits),
                    MaxResults = 10
                });

            } catch (Exception ex) {

                throw new DashboardException(ex.Message + " (Unable to fetch data for period from " + query.PreviousStartDate.ToString("yyyy-MM-dd") + " to " + query.PreviousEndDate.ToString("yyyy-MM-dd") + " for block \"Popular\")");

            }

            // Get the root node of the site
            IPublishedContent rootNode = UmbracoContext.Current.ContentCache.GetById(query.Site.Id);

            List<object> items = new List<object>();


            foreach (AnalyticsDataRow row in data.Rows) {
                int value = row.GetInt32(AnalyticsMetric.Visits);
                string path = row.GetString(AnalyticsDimension.PagePath);
                items.Add(new {
                    url = path,
                    fullurl = (rootNode == null ? null : rootNode.UrlWithDomain().TrimEnd('/') + path),
                    visits = new {
                        raw = value,
                        text = DashboardContext.Current.Format(value)
                    }
                });
            }

            return new PopularBlock {
                HasData = data.Rows.Length > 0,
                Items = items.Take(max).ToArray()
            };

        }
        
    }

}