using System;
using System.Collections.Generic;
using Skybrud.Social.Google.Analytics.Interfaces;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics {

    public class DataRow {

        private Dictionary<string, object> _cells = new Dictionary<string, object>();

        public Dictionary<string, object> Cells {
            get { return _cells; }
        }

        public string GetString(string key) {
            return (string)_cells[key];
        }

        public string GetString(IAnalyticsField field) {
            return GetString(field.Name.Substring(3));
        }

        public int GetInt32(string key) {
            return (int)_cells[key];
        }

        public int GetInt32(IAnalyticsField field) {
            return GetInt32(field.Name.Substring(3));
        }

        public double GetDouble(string key) {
            return (double)_cells[key];
        }

        public double GetDouble(IAnalyticsField field) {
            return GetDouble(field.Name.Substring(3));
        }

        public static DataRow[] Convert(AnalyticsDataResponse data) {
            List<DataRow> rows = new List<DataRow>();
            foreach (AnalyticsDataRow row in data.Rows) {
                DataRow temp = new DataRow();
                for (int i = 0; i < data.ColumnHeaders.Length; i++) {
                    AnalyticsDataColumnHeader column = data.ColumnHeaders[i];
                    string name = column.Name.Substring(3);
                    string value = row.Cells[i].Value;
                    switch (column.DataType) {
                        case "INTEGER":
                            temp.Cells[name] = Int32.Parse(value);
                            break;
                        case "STRING":
                            temp.Cells[name] = value;
                            break;
                        case "TIME":
                            temp.Cells[name] = Double.Parse(value);
                            break;
                        default:
                            temp.Cells[name] = value + " (" + column.DataType + ")";
                            break;
                    }
                }
                rows.Add(temp);
            }
            return rows.ToArray();
        }

    }

}