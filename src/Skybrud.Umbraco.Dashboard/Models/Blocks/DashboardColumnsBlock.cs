using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Interfaces;

namespace Skybrud.Umbraco.Dashboard.Models.Blocks {
    
    /// <summary>
    /// Class describing a column based block.
    /// </summary>
    public class DashboardColumnsBlock : DashboardBlock {

        #region Properties

        /// <summary>
        /// Gets a list of all columns (blocks) added to the block.
        /// </summary>
        [JsonProperty("columns")]
        public List<IDashboardBlock> Columns { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new columns block.
        /// </summary>
        public DashboardColumnsBlock() : base("Columns") {
            Columns = new List<IDashboardBlock>();
        }

        /// <summary>
        /// Initializes a new columns block based on the specified <code>alias</code>.
        /// </summary>
        /// <param name="alias">The alias of the block.</param>
        public DashboardColumnsBlock(string alias) : base(alias) {
            Columns = new List<IDashboardBlock>();
        }

        /// <summary>
        /// Initializes a new columns block based on the specified <code>alias</code> and <code>view</code>.
        /// </summary>
        /// <param name="alias">The alias of the block.</param>
        /// <param name="view">The URL of the view.</param>
        public DashboardColumnsBlock(string alias, string view) : base(alias, view) {
            Columns = new List<IDashboardBlock>();
        }

        #endregion

    }

}
