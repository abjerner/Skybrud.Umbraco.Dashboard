using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Interfaces;

namespace Skybrud.Umbraco.Dashboard.Model.Blocks.StatusBox {

    public class StatusBoxItem {

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("values")]
        public StatusBoxItemValue[] Values { get; set; }

        [JsonProperty("link")]
        public StatusBoxItemLink Link { get; set; }

    }

    public class StatusBoxItemLink {

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

    }

    public class StatusBoxItemSummary {

        public string Id { get; set; }
        public string Alias { get; set; }
        public StatusBoxItemSummaryValue[] Values { get; set; }
        public string LinkUrl { get; set; }
        public string Singular { get; set; }
        public string Plural { get; set; }

    }

    public class StatusBoxItemSummaryValue {

        public string Type { get; set; }
        public int Count { get; set; }

        public StatusBoxItemSummaryValue() { }

        public StatusBoxItemSummaryValue(string type, int count) {
            Type = type;
            Count = count;
        }

    }

    public class StatusBoxItemValue {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("text")]
        public StatusBoxItemValueText Text { get; set; }

    }

    public class StatusBoxItemValueText {

        [JsonProperty("full")]
        public string Full { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("tooltip")]
        public string Tooltip { get; set; }

        public StatusBoxItemValueText() { }

        public StatusBoxItemValueText(string full, string number) : this(full, number, null) { }

        public StatusBoxItemValueText(string full, string number, string tooltip) {
            Full = full;
            Number = number;
            Tooltip = tooltip ?? full;
        }

    }

    public class StatusBoxItemsResponse {

        [JsonProperty("site")]
        public IDashboardSite Site { get; set; }

        [JsonProperty("page")]
        public DashboardPage Page { get; set; }

        [JsonProperty("cached")]
        public bool IsCached { get; set; }

        [JsonProperty("result")]
        public StatusBoxItemsResult Result { get; set; }

    }

    public class StatusBoxItemsResult {

        [JsonProperty("settings")]
        public object Settings { get; set; }

        [JsonProperty("items")]
        public StatusBoxItem[] Items { get; set; }

    }

}

































