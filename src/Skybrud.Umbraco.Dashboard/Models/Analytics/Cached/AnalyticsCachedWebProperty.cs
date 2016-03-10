using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Social.Json.Extensions.JObject;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Cached {
    
    public class AnalyticsCachedWebProperty {

        [JsonIgnore]
        public AnalyticsCachedAccount Account { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("websiteUrl")]
        public string WebsiteUrl { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("profiles")]
        public AnalyticsCachedProfile[] Profiles { get; set; }

        public static AnalyticsCachedWebProperty Parse(AnalyticsCachedAccount account, JObject obj) {
            AnalyticsCachedWebProperty webProperty = new AnalyticsCachedWebProperty();
            webProperty.Account = account;
            webProperty.Id = obj.GetString("id");
            webProperty.Name = obj.GetString("name");
            webProperty.WebsiteUrl = obj.GetString("websiteUrl");
            webProperty.Created = obj.GetString("created");
            webProperty.Updated = obj.GetString("updated");
            webProperty.Profiles = obj.GetArray("profiles", x => AnalyticsCachedProfile.Parse(webProperty, x));
            return webProperty;
        }

        public bool IsMatch(string query) {
            return (Id + "  " + Name + "  " + WebsiteUrl).ToLower().Contains(query);
        }

    }

}