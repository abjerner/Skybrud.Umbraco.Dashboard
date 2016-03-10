using Skybrud.Umbraco.Dashboard.Interfaces;

namespace Skybrud.Umbraco.Dashboard.Models.Blocks {
    
    /// <summary>
    /// Class representing a basic block for the dashboard.
    /// </summary>
    public class DashboardBlock : IDashboardBlock {

        #region Properties

        public string Alias { get; private set; }

        public string View { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new dashboard block based on the specified <code>alias</code>.
        /// </summary>
        /// <param name="alias">The alias of the block.</param>
        public DashboardBlock(string alias) {
            Alias = alias;
            View = DashboardHelpers.GetCachableUrl("/App_Plugins/Skybrud.Dashboard/Views/Blocks/" + alias + ".html");
        }

        /// <summary>
        /// Initializes a new dashboard block based on the specified <code>alias</code> and <code>view</code>.
        /// </summary>
        /// <param name="alias">The alias of the block.</param>
        /// <param name="view">The URL of the view.</param>
        public DashboardBlock(string alias, string view) {
            Alias = alias;
            View = DashboardHelpers.GetCachableUrl(view);
        }

        #endregion

    }

}