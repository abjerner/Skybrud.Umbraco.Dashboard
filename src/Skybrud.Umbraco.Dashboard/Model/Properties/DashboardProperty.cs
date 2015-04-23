using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard.Model.Properties {
    
    public class DashboardProperty {

        #region Properties

        [JsonProperty("showOnce")]
        public bool ShowOnce { get; protected set; }

        [JsonProperty("addPanel")]
        public bool AddPanel { get; protected set; }

        [JsonProperty("serverSide")]
        public bool ServerSide { get; protected set; }

        [JsonProperty("path")]
        public string Path {
            get { return View; }
            protected set { View = value; }
        }

        [JsonProperty("view")]
        public string View { get; protected set; }

        [JsonProperty("caption")]
        public string Caption { get; protected set; }

        #endregion

        #region Constructors

        protected DashboardProperty() { }

        public DashboardProperty(string alias) {
            // TODO: Should we store the alias?
            View = DashboardHelpers.GetCachableUrl("/App_Plugins/Skybrud.Dashboard/Views/Properties/" + alias + ".html");
        }

        public DashboardProperty(bool showOnce, bool addPanel, bool serverSide, string path, string caption) {
            ShowOnce = showOnce;
            AddPanel = addPanel;
            ServerSide = serverSide;
            View = path;
            Caption = caption;
        }

        #endregion

    }

}