using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses;
using Skybrud.Umbraco.Dashboard.Exceptions;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics.Blocks {
    
    public class LineChartBlock : AnalyticsChildBlock {

        [JsonProperty("title")]
        public string Title { get; set; }

        //[JsonProperty("data")]
        [JsonIgnore]
        public object Data { get; set; }

        [JsonProperty("datasets")]
        public object[] Datasets { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }

        [JsonProperty("debug")]
        public object Debug { get; set; }
            
        public LineChartBlock() : base("AnalyticsLineChart") {
            Title = DashboardContext.Current.Translate("analytics_title_linechart");
        }

        public static LineChartBlock GetBlock(DataQuery query) {

            // Declare the data options
            AnalyticsDataOptions options = new AnalyticsDataOptions {
                StartDate = query.CurrentStartDate,
                EndDate = query.CurrentEndDate,
                Metrics = AnalyticsMetric.Visits + AnalyticsMetric.Pageviews
            };

            if (query.Days <= 1) {
                options.Dimensions = AnalyticsDimension.Hour;
            } else if (query.Days <= 31) {
                options.Dimensions = AnalyticsDimension.Date;
                //options.Sorting = new AnalyticsSortOptions().AddAscending(AnalyticsDimension.Date);
            } else {
                options.Dimensions = AnalyticsDimension.YearWeek;
            }

            // Add any extra options?
            switch (query.Type) {
                case DataQueryType.Page:
                    options.Filters = query.CreateFilterOptionsFromPageUrls();
                    break;
            }

            // Fetch the data
            AnalyticsDataResponse data;
            try {
                data = query.GetCachedData("LineChart", options);
            } catch (Exception ex) {
                throw new DashboardException(ex.Message + " (Unable to fetch data for period from " + query.PreviousStartDate.ToString("yyyy-MM-dd") + " to " + query.PreviousEndDate.ToString("yyyy-MM-dd") + " for block \"LineChart\")");
            }

            object ddata;

            if (data.Rows.Length == 0) {
                ddata = new {
                    columns = new object[0],
                    rows = new object[0]
                };
            } else {

                object[] columns = new object[data.ColumnHeaders.Length];
                object[] rows = new object[data.Rows.Length];

                for (int i = 0; i < data.ColumnHeaders.Length; i++) {
                    var column = data.ColumnHeaders[i];
                    columns[i] = new {
                        alias = column.Name.Substring(3),
                        label = query.Context.Translate(column.Name)
                    };
                }

                for (int i = 0; i < data.Rows.Length; i++) {

                    AnalyticsDataRow row = data.Rows[i];

                    object[] rowdata = new object[row.Cells.Length];

                    for (int j = 0; j < row.Cells.Length; j++) {
                        rowdata[j] = GetCellData(row.Cells[j], query);
                    }

                    rows[i] = rowdata;

                }
                
                ddata = new { columns, rows };
            
            }

            var datasets = new object[] {
                new {
                    label = DashboardContext.Current.Translate(AnalyticsMetric.Pageviews),
                    fillColor = "#35353d",
                    strokeColor = "#35353d"
                },
                new  {
                    label = DashboardContext.Current.Translate(AnalyticsMetric.Visits),
                    fillColor = "red",//"rgba(141, 146, 157, 1)",
                    strokeColor = "red"//"rgba(141, 146, 157, 1)"
                }
            };

            object[] items = (
                from row in data.Rows
                let first = row.Cells[0]
                select (object) new {
                    label = query.FormatCell(first),
                    visits = query.FormatInt32(AnalyticsMetric.Visits, row),
                    pageviews = query.FormatInt32(AnalyticsMetric.Pageviews, row)
                }
            ).ToArray();

            return new LineChartBlock {
                HasData = data.Rows.Any(x => x.GetInt32(AnalyticsMetric.Visits) > 0),
                Data = ddata,
                Datasets = datasets,
                Items = items.ToArray(),
                Debug = new {
                    query = data.Query.ToJson(),
                    days = query.Days
                }
            };

        }

        private static object GetCellData(AnalyticsDataCell cell, DataQuery query) {

            object raw;
            object value;

            switch (cell.Column.DataType) {
                case "dINTEGER":
                    raw = cell.GetInt32();
                    value = cell.GetInt32().ToString("N0", query.Context.Culture);
                    break;
                case "dDOUBLE":
                    raw = cell.GetDouble();
                    value = cell.GetDouble().ToString("N2", query.Context.Culture);
                    break;
                default:
                    raw = cell.GetString();
                    value = cell.GetString() + " (" + cell.Column.ColumnType + ")";
                    break;
            }

            return new {
                raw, value
            };

        }
        
    }

}