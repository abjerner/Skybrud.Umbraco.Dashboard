using System;
using System.Globalization;
using System.IO;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using Skybrud.Social.Google;
using Skybrud.Social.Google.Analytics.Interfaces;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Model.Analytics.Blocks;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics {
    
    public class DataQuery {

        #region Properties
        
        public IAnalyticsSite Site { get; private set; }

        /// <summary>
        /// Gets or sets a reference to the Google service used for communicating with the various
        /// Google APIs.
        /// </summary>
        public GoogleService Service { get; private set; }

        /// <summary>
        /// Gets or sets the amount of days in the specified period.
        /// </summary>
        public int Days { get; private set; }

        /// <summary>
        /// Gets or sets whether caching should be enabled for this query. Unless testing or
        /// intentionally forcing the cache to be updated, this should always be set to true.
        /// </summary>
        public bool EnableCaching { get; private set; }

        public DateTime CurrentStartDate { get; private set; }
        public DateTime CurrentEndDate { get; private set; }

        public DateTime PreviousStartDate { get; private set; }
        public DateTime PreviousEndDate { get; private set; }

        public DataQueryType Type { get; set; }
        public int PageId { get; set; }
        public string PageUrl { get; set; }
        public string[] PageUrls { get; set; }

        /// <summary>
        /// Gets a reference to the current dashboard context.
        /// </summary>
        public DashboardContext Context {
            get { return DashboardContext.Current; }
        }

        #endregion

        #region Constructor

        public DataQuery(IAnalyticsSite site, int days, bool enableCaching) {

            Site = site;

            EnableCaching = enableCaching;

            Days = days;

            Service = GoogleService.CreateFromRefreshToken(site.Analytics.ClientId, site.Analytics.ClientSecret, site.Analytics.RefreshToken);

            CurrentStartDate = DateTime.Today.AddDays(-days);
            CurrentEndDate = DateTime.Today.AddSeconds(-1);

            TimeSpan diff = CurrentEndDate.AddSeconds(1).Subtract(CurrentStartDate);

            PreviousStartDate = CurrentStartDate.Subtract(diff);
            PreviousEndDate = CurrentEndDate.Subtract(diff);

        }

        #endregion

        #region Member methods

        public DataResponse GetResponse() {

            DataResponse response = new DataResponse {
                Days = Days,
                LineChart = LineChartBlock.GetBlock(this)
            };

            switch (Type) {
                case DataQueryType.Page:
                    response.AddBlock(VisitsBlock.GetBlock(this));
                    response.AddBlock(DevicesBlock.GetBlock(this));
                    throw new NotImplementedException();
                    //response.Page = new {
                    //    id = PageId,
                    //    url = PageUrl
                    //};
                    break;
                case DataQueryType.Site:
                    response.AddBlock(VisitsBlock.GetBlock(this));
                    response.AddBlock(PopularBlock.GetBlock(this, max: 5));
                    response.AddBlock(DevicesBlock.GetBlock(this));
                    break;
            }

            return response;

        }


        internal AnalyticsDataResponse GetCachedData(string key, AnalyticsDataOptions options) {

            // Declare the path to the cache directory
            string cacheDir = HttpContext.Current.Server.MapPath("~/App_Data/Skybrud.Dashboard/AnalyticsCache/" + (PageId > 0 ? "Pages/" : ""));

            // Generate a combined key
            string combinedKey = (PageId > 0 ? PageId + "" : Site.Analytics.ProfileId) + "_" + key + "_" + Days + ".json";

            // Declare the path to the cache file (based on the cache key)
            string path = Path.Combine(cacheDir, combinedKey + ".json");

            // Return the cache response if present and not expired
            if (EnableCaching && File.Exists(path) && File.GetLastWriteTimeUtc(path) > DateTime.Today) {
                return AnalyticsDataResponse.LoadJson(path);
            }

            // Get the data from the Analytics API
            AnalyticsDataResponse response = Service.Analytics.GetData(Site.Analytics.ProfileId, options);

            // Save the response to the cache
            response.SaveJson(path);

            // Return the response
            return response;

        }

        ///// <summary>
        ///// Returns an array with exactly two elements. The first element will be data for the
        ///// previous preiod (as defined by properties <code>PreviousStartDate</code> and
        ///// <code>PreviousEndDate</code>). The second element will be data for the current period
        ///// (as defined by <code>CurrentStartDate</code> and <code>CurrentEndDate</code>).
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="options"></param>
        internal void GetCachedDataPreviousAndCurrent(string key, out AnalyticsDataResponse previous, out AnalyticsDataResponse current, AnalyticsDataOptions options) {

            previous = GetCachedData(key + "_Previous", new AnalyticsDataOptions {
                StartDate = PreviousStartDate,
                EndDate = PreviousEndDate,
                Metrics = options.Metrics,
                Dimensions = options.Dimensions,
                Filters = options.Filters,
                Sorting = options.Sorting,
                MaxResults = options.MaxResults
            });

            current = GetCachedData(key + "_Current", new AnalyticsDataOptions {
                StartDate = CurrentStartDate,
                EndDate = CurrentEndDate,
                Metrics = options.Metrics,
                Dimensions = options.Dimensions,
                Filters = options.Filters,
                Sorting = options.Sorting,
                MaxResults = options.MaxResults
            });

        }

        public AnalyticsFilterOptions CreateFilterOptionsFromPageUrls() {
            AnalyticsFilterOptions filters = new AnalyticsFilterOptions();
            foreach (string url in PageUrls) {
                filters.Add(new AnalyticsDimensionFilter(AnalyticsDimension.PagePath, AnalyticsDimensionOperator.ExactMatch, url));
            }
            return filters;
        }

        internal object FormatCell(AnalyticsDataCell cell) {

            string key = cell.Column.Name.Substring(3);

            string text = cell.Value;

            switch (cell.Column.Name) {
                case "ga:date": text = DateTime.ParseExact(text, "yyyyMMdd", null).ToString("d. MMM", new CultureInfo("da-DK")); break;
                case "ga:yearWeek": text = "Uge " + Int32.Parse(text.Substring(4)); break;
            }

            return new OmgDataRow {
                Alias = key,
                Label = Context.Translate(cell.Column.Name),
                Value = new { raw = cell.Value, text }
            };

        }

        internal object FormatInt32(IAnalyticsField field, AnalyticsDataRow row) {

            string key = field.Name.Substring(3);

            int value = (row == null ? 0 : row.GetInt32(field));

            return new OmgDataRow {
                Alias = key,
                Label = Context.Translate(field),
                Value = new { raw = value, text = Context.Format(value) }
            };

        }

        internal object FormatDouble(IAnalyticsField field, AnalyticsDataRow row) {

            string key = field.Name.Substring(3);

            double value = (row == null ? 0 : row.GetDouble(field));

            return new OmgDataRow {
                Alias = key,
                Label = Context.Translate(field),
                Value = new { raw = value, text = Context.Format(value) }
            };

        }

        internal object FormatVisitDataInt32(IAnalyticsField field, AnalyticsDataRow row1, AnalyticsDataRow row2) {

            string key = field.Name.Substring(3);

            int valueOld = (row1 == null ? 0 : row1.GetInt32(field));
            int valueNew = (row2 == null ? 0 : row2.GetInt32(field));

            int change = valueNew - valueOld;

            return new OmgDataRow {
                Alias = key,
                Label = Context.Translate(field),
                Value = new { raw = valueNew, text = Context.Format(valueNew) },
                Change = new { raw = change, text = Context.Format(change) },
            };

        }

        internal object FormatVisitDataDouble(IAnalyticsField field, AnalyticsDataRow row1, AnalyticsDataRow row2) {

            string key = field.Name.Substring(3);

            double valueOld = (row1 == null ? 0 : row1.GetDouble(field));
            double valueNew = (row2 == null ? 0 : row2.GetDouble(field));

            double change = valueNew - valueOld;

            return new OmgDataRow {
                Alias = key,
                Label = Context.Translate(field),
                Value = new { raw = valueNew, text = Context.Format(valueNew) },
                Change = new { raw = change, text = Context.Format(change) },
            };

        }

        internal object FormatVisitDataInt32(IAnalyticsField field, DataRow row1, DataRow row2) {

            string key = field.Name.Substring(3);

            int valueOld = (row1 == null ? 0 : row1.GetInt32(key));
            int valueNew = (row2 == null ? 0 : row2.GetInt32(key));

            int change = valueNew - valueOld;

            return new OmgDataRow {
                Alias = key,
                Label = Context.Translate(field),
                Value = new { raw = valueNew, text = Context.Format(valueNew) },
                Change = new { raw = change, text = Context.Format(change) },
            };

        }

        internal object FormatVisitDataDouble(IAnalyticsField field, DataRow row1, DataRow row2) {

            string key = field.Name.Substring(3);

            double valueOld = (row1 == null ? 0 : row1.GetDouble(key));
            double valueNew = (row2 == null ? 0 : row2.GetDouble(key));

            double change = valueNew - valueOld;

            return new OmgDataRow {
                Alias = key,
                Label = Context.Translate(field),
                Value = new { raw = valueNew, text = Context.Format(valueNew) },
                Change = new { raw = change, text = Context.Format(change) },
            };

        }

        #endregion

        #region Static methods

        /// <summary>
        /// Initializes a new data query for the specified amount of <code>days</code>.
        /// </summary>
        /// <param name="site">The site to get data for.</param>
        /// <param name="days">The amount of days defining the period to get data for.</param>
        /// <param name="enableCaching">Specifies whether caching should be enabled.</param>
        public static DataQuery GetFromDays(IAnalyticsSite site, int days, bool enableCaching) {
            return new DataQuery(site, days, enableCaching);
        }

        /// <summary>
        /// Initializes a new data query for the specified <code>period</code>.
        /// </summary>
        /// <param name="site">The site to get data for.</param>
        /// <param name="period">The period to get data for.</param>
        /// <param name="enableCaching">Specifies whether caching should be enabled.</param>
        public static DataQuery GetFromPeriod(IAnalyticsSite site, string period, bool enableCaching) {
            switch (period) {
                case "lastyear": return new DataQuery(site, 365, enableCaching);
                case "lastmonth": return new DataQuery(site, 31, enableCaching);
                case "lastweek": return new DataQuery(site, 7, enableCaching);
                default: return new DataQuery(site, 1, enableCaching);
            }
        }

        #endregion

        // Not sure whether it it JSON.NET or WebApi, but it seems that
        // properties with the value of NULL (or even just zero) is ignored
        // when generating the JSON. Therefore this class is needed so we can
        // tell JSON.NET to always include the properties.
        private class OmgDataRow {

            [JsonProperty("alias", NullValueHandling = NullValueHandling.Include)]
            public string Alias { get; set; }

            [JsonProperty("label", NullValueHandling = NullValueHandling.Include)]
            public string Label { get; set; }

            [JsonProperty("value", NullValueHandling = NullValueHandling.Include)]
            public object Value { get; set; }

            [JsonProperty("change", NullValueHandling = NullValueHandling.Ignore)]
            public object Change { get; set; }

            [JsonProperty("debug", NullValueHandling = NullValueHandling.Ignore)]
            public object Debug { get; set; }

        }

    }

}