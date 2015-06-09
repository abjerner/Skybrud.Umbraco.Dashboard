using System.Collections.Generic;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Skybrud.Umbraco.Dashboard.Model;

namespace Skybrud.Umbraco.Dashboard.Plugins {

    /// <summary>
    /// The <code>IDashboardPlugin</code> interface currently only has a single method, but more
    /// will be added in the future. If you don't want to implement all methods, you can inherit
    /// from this class rather than the interface, and since you can stick to implementing the
    /// methods that you actually need.
    /// </summary>
    public abstract class DashboardPluginBase : IDashboardPlugin {

        public virtual void GetDashboard(string section, List<DashboardTab> tabs) { }

        public virtual void GetSites(List<IDashboardSite> sites) { }

        public virtual IDashboardSite GetSiteById(int siteId) {
            return null;
        }

    }

}