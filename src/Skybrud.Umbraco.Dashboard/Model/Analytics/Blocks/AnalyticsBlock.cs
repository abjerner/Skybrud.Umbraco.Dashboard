using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Controllers;
using Skybrud.Umbraco.Dashboard.Interfaces;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics.Blocks {

    public class AnalyticsBlock : IDashboardBlock {

        public string Alias {
            get { return "Analytics"; }
        }

        public string View {
            get { return DashboardHelpers.GetCachableUrl("/App_Plugins/Skybrud.Dashboard/Views/Blocks/Analytics.html"); }
        }

        [JsonProperty("title")]
        public string Title {
            get { return "Statistik"; }
        }

        /// <summary>
        /// Gets the URL of the Google Analytics service.
        /// </summary>
        [JsonProperty("serviceurl")]
        public string ServiceUrl {
            get { return DashboardContext.Current.GetServiceUrl<AnalyticsController>(); }
        }

        [JsonProperty("periods")]
        public object Periods {
            get {
                return new[] {
                    new { alias = "yesterday", text = "I går"},
                    new { alias = "lastweek", text = "Sidste uge"},
                    new { alias = "lastmonth", text = "Sidste måned"},
                    new { alias = "lastyear", text = "Sidste år"}
                };
            }
        }

        [JsonProperty("actions")]
        public object Actions {
            get {
                return new[] {
                    new {
                        text = "Gå til Google Analytics",
                        url = "https://www.google.com/analytics/web/",
                        target = "_blank",
                        classes = "next"
                    }
                };
            }
        }
    
    }

}
