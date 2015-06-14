using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard.Interfaces {

    /// <summary>
    /// Interface describing a basic dashboard block.
    /// </summary>
    public interface IDashboardBlock {

        /// <summary>
        /// Gets the alias of the block.
        /// </summary>
        [JsonProperty("alias")]
        string Alias { get; }

        /// <summary>
        /// Gets the URL to the view of the block.
        /// </summary>
        [JsonProperty("view")]
        string View { get; }

    }

}