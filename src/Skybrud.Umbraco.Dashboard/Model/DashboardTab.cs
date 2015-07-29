using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Model.Properties;

namespace Skybrud.Umbraco.Dashboard.Model {
    
    /// <summary>
    /// Class describing a dashboard tab.
    /// </summary>
    public class DashboardTab {

        #region Properties

        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("active")]
        internal bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the label (title) of the tab.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the alias of the tab.
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Gets a list containing all the properties added to the tab.
        /// </summary>
        [JsonProperty("properties")]
        public List<DashboardProperty> Properties { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new tab with an empty list of properties.
        /// </summary>
        public DashboardTab() {
            Properties = new List<DashboardProperty>();
        }

        /// <summary>
        /// Initializes a new tab with the specified array of <code>properties</code>.
        /// </summary>
        /// <param name="properties">The properties to be added to the tab.</param>
        public DashboardTab(params DashboardProperty[] properties) {
            Properties = new List<DashboardProperty>();
            if (properties != null) Properties.AddRange(properties);
        }

        #endregion

    }

}