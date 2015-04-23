using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard.Model.Properties {
    
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

        public DashboardDataProperty(string alias) : base(alias) { }

        #endregion

    }

}