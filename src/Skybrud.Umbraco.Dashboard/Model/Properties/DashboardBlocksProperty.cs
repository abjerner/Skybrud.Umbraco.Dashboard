using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Interfaces;

namespace Skybrud.Umbraco.Dashboard.Model.Properties {
    
    /// <summary>
    /// Class representing a block based dashboard property.
    /// </summary>
    public class DashboardBlocksProperty : DashboardProperty {

        #region Properties
        
        /// <summary>
        /// Gets a list of all the blocks added to the property.
        /// </summary>
        [JsonProperty("blocks")]
        public List<IDashboardBlock> Blocks { get; protected set; }

        #endregion

        #region Constructors

        protected DashboardBlocksProperty() { }

        /// <summary>
        /// Initializes a new dashboard property with the specified <code>alias</code>.
        /// </summary>
        /// <param name="alias">The alias of the property.</param>
        public DashboardBlocksProperty(string alias) : base(alias) {
            Blocks = new List<IDashboardBlock>();
        }

        /// <summary>
        /// Initializes a new dashboard property with the specified <code>alias</code> and <code>view</code>.
        /// </summary>
        /// <param name="alias">The alias of the property.</param>
        /// <param name="view">THe URL to the view of the property.</param>
        public DashboardBlocksProperty(string alias, string view) : base(alias, view) {
            Blocks = new List<IDashboardBlock>();
        }

        #endregion

    }

}