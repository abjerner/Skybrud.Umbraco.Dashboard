using System.Collections.Generic;
using Skybrud.Umbraco.Dashboard.Models;

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

        /// <summary>
        /// Method called when the Dashboard requests a list of sites.
        /// </summary>
        /// <param name="sites">The list of sites.</param>
        void GetSites(List<IDashboardSite> sites);

        /// <summary>
        /// Gets the site by the specified <code>siteId</code>.
        /// </summary>
        /// <param name="siteId">The ID of the site.</param>
        IDashboardSite GetSiteById(int siteId);

        /// <summary>
        /// Gets the page by the specified <code>pageId</code>.
        /// </summary>
        /// <param name="pageId">The ID of the page.</param>
        IDashboardPage GetPageById(int pageId);

    }

}