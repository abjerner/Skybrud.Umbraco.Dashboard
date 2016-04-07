using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Controllers;
using Skybrud.Umbraco.Dashboard.Interfaces;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Blocks {

    public class AnalyticsBlock : IDashboardBlock {

        /// <summary>
        /// Gets a reference to the dashboard context.
        /// </summary>
        [JsonIgnore]
        public DashboardContext Dashboard {
            get { return DashboardContext.Current; }
        }

        public string Alias {
            get { return "Analytics"; }
        }

        public string View {
            get { return DashboardHelpers.GetCachableUrl("/App_Plugins/Skybrud.Dashboard/Views/Blocks/Analytics.html"); }
        }

        [JsonProperty("title")]
        public string Title {
            get { return Dashboard.Translate("dashboard/analyticsTitle"); }
        }

        /// <summary>
        /// Gets the URL of the Google Analytics service.
        /// </summary>
        [JsonProperty("serviceurl")]
        public string ServiceUrl {
            get { return Dashboard.GetServiceUrl<AnalyticsController>(); }
        }

        [JsonProperty("periods")]
        public object Periods {
            get {
                return new[] {
                    new { alias = "yesterday", text = Dashboard.Translate("dashboard/analyticsYesterday")},
                    new { alias = "lastweek", text = Dashboard.Translate("dashboard/analyticsLastWeek")},
                    new { alias = "lastmonth", text = Dashboard.Translate("dashboard/analyticsLastMonth")},
                    new { alias = "lastyear", text = Dashboard.Translate("dashboard/analyticsLastYear")},
                };
            }
        }

        [JsonProperty("actions")]
        public object Actions {
            get {
                return new[] {
                    new {
                        text = Dashboard.Translate("dashboard/analyticsGoTo"),
                        url = "https://www.google.com/analytics/web/",
                        target = "_blank",
                        classes = "next"
                    }
                };
            }
        }
    
    }

}
