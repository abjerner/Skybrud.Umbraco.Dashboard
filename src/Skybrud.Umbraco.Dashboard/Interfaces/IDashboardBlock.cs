using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard.Interfaces {

    public interface IDashboardBlock {

        [JsonProperty("alias")]
        string Alias { get; }

        [JsonProperty("view")]
        string View { get; }

    }

}