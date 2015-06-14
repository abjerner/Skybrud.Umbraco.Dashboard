using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard.Model.Properties {
    
    /// <summary>
    /// Class describing a dashboard property.
    /// </summary>
    public class DashboardProperty {

        #region Properties

        /// <summary>
        /// Gets whether this property should only be seen once. This appears to be some legacy from Umbraco 6, since
        /// I can't see where it is used in Umbraco 7.
        /// </summary>
        [JsonProperty("showOnce")]
        public bool ShowOnce { get; protected set; }

        /// <summary>
        /// Gets whether this property should add a panel in the UI.
        /// </summary>
        [JsonProperty("addPanel")]
        public bool AddPanel { get; protected set; }

        /// <summary>
        /// Gets whether this property references a server side control.
        /// </summary>
        [JsonProperty("serverSide")]
        public bool ServerSide { get; protected set; }

        /// <summary>
        /// Same as <code>Url</code>. Gets the URL to the view of the property.
        /// </summary>
        [JsonProperty("path")]
        public string Path {
            get { return View; }
            protected set { View = value; }
        }

        /// <summary>
        /// Gets the alias of the property.
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; protected set; }

        /// <summary>
        /// Gets the URL to the view of the property.
        /// </summary>
        [JsonProperty("view")]
        public string View { get; protected set; }

        /// <summary>
        /// Gets the caption of the property. This appears to be some legacy from Umbraco 6, since
        /// I can't see where it is used in Umbraco 7.
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; protected set; }

        #endregion

        #region Constructors

        protected DashboardProperty() { }

        /// <summary>
        /// Initializes a new dashboard property with the specified <code>alias</code>.
        /// </summary>
        /// <param name="alias">The alias of the property.</param>
        public DashboardProperty(string alias) {
            Alias = alias;
            View = DashboardHelpers.GetCachableUrl("/App_Plugins/Skybrud.Dashboard/Views/Properties/" + alias + ".html");
        }

        /// <summary>
        /// Initializes a new dashboard property with the specified <code>alias</code> and <code>view</code>.
        /// </summary>
        /// <param name="alias">The alias of the property.</param>
        /// <param name="view">THe URL to the view of the property.</param>
        public DashboardProperty(string alias, string view) {
            Alias = alias;
            View = view;
        }

        /// <summary>
        /// Initializes a new dashboard property with the specified <code>alias</code>.
        /// </summary>
        public DashboardProperty(bool showOnce, bool addPanel, bool serverSide, string path, string caption) {
            ShowOnce = showOnce;
            AddPanel = addPanel;
            ServerSide = serverSide;
            Alias = path;
            View = path;
            Caption = caption;
        }

        #endregion

    }

}