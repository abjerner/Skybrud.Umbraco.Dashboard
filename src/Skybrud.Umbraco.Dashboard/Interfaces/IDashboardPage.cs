using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.Dashboard.Interfaces {
    
    /// <summary>
    /// Interface representing a page shown in the dashboard.
    /// </summary>
    public interface IDashboardPage {

        /// <summary>
        /// Gets a reference to the underlying <see cref="IPublishedContent"/>.
        /// </summary>
        [JsonIgnore]
        IPublishedContent Content { get; }

        /// <summary>
        /// Gets the reference to the site of the page.
        /// </summary>
        [JsonIgnore]
        IDashboardSite Site { get; }

        /// <summary>
        /// Gets the ID of the page.
        /// </summary>
        [JsonProperty("id")]
        int Id { get; }

        /// <summary>
        /// Gets the name of the page.
        /// </summary>
        [JsonProperty("name")]
        string Name { get; }

        /// <summary>
        /// Gets the URL of the page.
        /// </summary>
        [JsonProperty("url")]
        string Url { get; }

        #region Methods

        /// <summary>
        /// Gets an array of blocks for the page.
        /// </summary>
        /// <returns>Returns an array of <see cref="IDashboardBlock"/>.</returns>
        IDashboardBlock[] GetBlocks();

        #endregion

    }

}