using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard.Models.Properties {
    
    /// <summary>
    /// Class describing a data based dashboard property.
    /// </summary>
    public class DashboardErrorProperty : DashboardProperty {

        #region Properties

        /// <summary>
        /// Gets or sets the title of the error message.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text of the error message.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new property with default values.
        /// </summary>
        public DashboardErrorProperty() : base("Error") {
            Title = "System error";
            Message = "An unknown server error occured.";
        }

        /// <summary>
        /// Initializes a new property with the specified <code>title</code> and <code>message</code>.
        /// </summary>
        /// <param name="title">The title of the error message.</param>
        /// <param name="message">The text of the error message.</param>
        public DashboardErrorProperty(string title, string message) : base("Error") { }

        /// <summary>
        /// Initializes a new property with the specified <code>title</code>, <code>message</code> and <code>alias</code>.
        /// </summary>
        /// <param name="title">The title of the error message.</param>
        /// <param name="message">The text of the error message.</param>
        /// <param name="alias">The alias of the property.</param>
        public DashboardErrorProperty(string title, string message, string alias) : base(alias) { }

        /// <summary>
        /// Initializes a new property with the specified <code>title</code>, <code>message</code>, <code>alias</code> and <code>view</code>.
        /// </summary>
        /// <param name="title">The title of the error message.</param>
        /// <param name="message">The text of the error message.</param>
        /// <param name="alias">The alias of the property.</param>
        /// <param name="view">THe URL to the view of the property.</param>
        public DashboardErrorProperty(string title, string message, string alias, string view) : base(alias, view) { }

        #endregion

    }

}