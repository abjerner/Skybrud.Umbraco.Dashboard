using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.Dashboard.Interfaces {

    /// <summary>
    /// Interface describing a site.
    /// </summary>
    public interface IDashboardSite {

        #region Properties

        /// <summary>
        /// Gets a reference to the underlying instance of <see cref="IPublishedContent"/>.
        /// </summary>
        [JsonIgnore]
        IPublishedContent Content { get; }

        /// <summary>
        /// Gets the ID of the site.
        /// </summary>
        [JsonProperty("id")]
        int Id { get; }

        /// <summary>
        /// Gets the name of the site.
        /// </summary>
        [JsonProperty("name")]
        string Name { get; }

        /// <summary>
        /// Gets the primary URL of the site.
        /// </summary>
        [JsonProperty("url")]
        string Url { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an array of blocks for the site.
        /// </summary>
        /// <returns>Returns an array of <see cref="IDashboardBlock"/>.</returns>
        IDashboardBlock[] GetBlocks();

        /// <summary>
        /// Gets an array of blocks for the page with the specified <code>pageId</code>.
        /// </summary>
        /// <returns>Returns an array of <see cref="IDashboardBlock"/>.</returns>
        IDashboardBlock[] GetBlocksForPage(int pageId);

        #endregion

    }

}