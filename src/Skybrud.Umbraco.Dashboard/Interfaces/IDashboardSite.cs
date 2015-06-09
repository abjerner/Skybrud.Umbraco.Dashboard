using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Model.Analytics;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.Dashboard.Interfaces {

    /// <summary>
    /// Interface describing a site.
    /// </summary>
    public interface IDashboardSite {

        /// <summary>
        /// Gets a reference to the underlying instance of <code>IPublishedContent</code>.
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

    }

}