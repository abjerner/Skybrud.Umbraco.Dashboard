using System.Collections.Generic;
using Skybrud.Umbraco.Dashboard.Models;
using Skybrud.Umbraco.Dashboard.Models.Properties;
using Umbraco.Web.Editors;
using Umbraco.Web.Models.ContentEditing;

namespace Skybrud.Umbraco.Dashboard.Plugins.Umbraco {

    /// <summary>
    /// Dashboard plugin reponsible for adding tabs as specified in <code>/config/Dashboard.config</code>.
    /// </summary>
    public class UmbracoDashboardPlugin : DashboardPluginBase {

        public override void GetDashboard(string section, List<DashboardTab> tabs) {

            // Since a lot of the dashboard logic and security is internal, we grab the tabs and
            // controls from Umbraco's own Dashboard controller, and then wrap them in our own types
            foreach (Tab<DashboardControl> t in new DashboardController().GetDashboard(section)) {
                
                // Initialize the tab
                DashboardTab tab = new DashboardTab {
                    Id = t.Id,
                    Alias = t.Alias,
                    IsActive = t.IsActive,
                    Label = t.Label
                };

                // Add any controls to the tab
                foreach (DashboardControl c in t.Properties) {
                    tab.Properties.Add(new DashboardProperty(c.ShowOnce, c.AddPanel, c.ServerSide, c.Path, c.Caption));
                }

                tabs.Add(tab);

            }

        }

    }

}