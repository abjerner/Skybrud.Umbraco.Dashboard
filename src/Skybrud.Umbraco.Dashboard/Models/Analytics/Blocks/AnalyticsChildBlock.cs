using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Models.Blocks;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Blocks {
    
    public class AnalyticsChildBlock : DashboardBlock {
        
        protected AnalyticsChildBlock(string alias) : base(alias) { }

        /// <summary>
        /// Gets whether the block has any data.
        /// </summary>
        [JsonProperty("hasData")]
        public bool HasData { get; protected set; }
    
    }

}
