using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Model.Properties;

namespace Skybrud.Umbraco.Dashboard.Model {
    
    public class DashboardTab {

        #region Properties

        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("active")]
        internal bool IsActive { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("properties")]
        public List<DashboardProperty> Properties { get; set; }

        #endregion

    }

}