using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Model.Blocks;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics.Blocks {
    
    public class AnalyticsChildBlock : DashboardBlock {
        
        protected AnalyticsChildBlock(string alias) : base(alias) { }

        /// <summary>
        /// Gets whether the block has any data.
        /// </summary>
        [JsonProperty("hasData")]
        public bool HasData { get; protected set; }
    
    }

}
