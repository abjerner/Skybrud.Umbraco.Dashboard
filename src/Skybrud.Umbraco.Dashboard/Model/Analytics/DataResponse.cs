using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Model.Analytics.Blocks;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics {

    public class DataResponse {

        List<AnalyticsChildBlock> _blocks = new List<AnalyticsChildBlock>();

        private AnalyticsChildBlock _lineChart = new LineChartBlock { Items = new object[0] };

        [JsonProperty("days")]
        public int Days { get; set; }

        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public object Page { get; set; }

        [JsonProperty("linechart")]
        public AnalyticsChildBlock LineChart {
            get { return _lineChart; }
            set { _lineChart = value ?? new LineChartBlock { Items = new object[0] }; }
        }

        [JsonProperty("blocks")]
        public AnalyticsChildBlock[] Blocks {
            get { return _blocks.ToArray(); }
        }

        public void AddBlock(AnalyticsChildBlock block) {
            if (block == null) return;
            _blocks.Add(block);
        }

    }

}