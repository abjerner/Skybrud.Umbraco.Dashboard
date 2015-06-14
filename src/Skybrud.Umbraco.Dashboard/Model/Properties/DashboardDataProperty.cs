using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard.Model.Properties {
    
    /// <summary>
    /// Class describing a data based dashboard property.
    /// </summary>
    public class DashboardDataProperty : DashboardProperty {

        #region Properties

        /// <summary>
        /// Gets or sets the data property. The value can be any object that can be serialized
        /// using JSON.net.
        /// </summary>
        [JsonProperty("data")]
        public object Data { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new dashboard property with the specified <code>alias</code>.
        /// </summary>
        /// <param name="alias">The alias of the property.</param>
        public DashboardDataProperty(string alias) : base(alias) { }

        /// <summary>
        /// Initializes a new dashboard property with the specified <code>alias</code>.
        /// </summary>
        /// <param name="alias">The alias of the property.</param>
        /// <param name="view">THe URL to the view of the property.</param>
        public DashboardDataProperty(string alias, string view) : base(alias, view) { }

        #endregion

    }

}