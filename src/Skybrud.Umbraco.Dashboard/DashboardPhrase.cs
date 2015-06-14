using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard {
    
    /// <summary>
    /// Class describing a phrase for the dashboard.
    /// </summary>
    public class DashboardPhrase {

        /// <summary>
        /// Gets or sets the key of the phrase.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the phrase.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

    }

}