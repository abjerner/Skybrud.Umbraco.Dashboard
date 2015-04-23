using System.Collections.Generic;
using Skybrud.Umbraco.Dashboard.Model;

namespace Skybrud.Umbraco.Dashboard.Interfaces {

    /// <summary>
    /// Interface describing a plugin for the dashboard.
    /// </summary>
    public interface IDashboardPlugin {

        /// <summary>
        /// Method called when Umbraco requests the tabs of the specified <code>section</code>.
        /// </summary>
        /// <param name="section">The current section being requested.</param>
        /// <param name="tabs">The list of tabs that can be manipulated.</param>
        void GetDashboard(string section, List<DashboardTab> tabs);

    }

}