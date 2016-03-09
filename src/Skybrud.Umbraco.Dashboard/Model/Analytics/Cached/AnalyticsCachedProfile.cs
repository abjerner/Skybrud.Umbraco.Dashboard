using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Social.Json.Extensions.JObject;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics.Cached {
    
    public class AnalyticsCachedProfile {

        [JsonIgnore]
        public AnalyticsCachedWebProperty WebProperty { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("websiteUrl")]
        public string WebsiteUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        public static AnalyticsCachedProfile Parse(AnalyticsCachedWebProperty webProperty, JObject obj) {
            AnalyticsCachedProfile profile = new AnalyticsCachedProfile();
            profile.WebProperty = webProperty;
            profile.Id = obj.GetString("id");
            profile.Currency = obj.GetString("currency");
            profile.Timezone = obj.GetString("timezone");
            profile.Name = obj.GetString("name");
            profile.Type = obj.GetString("type");
            profile.WebsiteUrl = obj.GetString("websiteUrl");
            profile.Created = obj.GetString("created");
            profile.Updated = obj.GetString("updated");
            return profile;
        }

        public bool IsMatch(string query) {
            return (Id + "  " + Name + "  " + WebsiteUrl).ToLower().Contains(query);
        }

    }

}