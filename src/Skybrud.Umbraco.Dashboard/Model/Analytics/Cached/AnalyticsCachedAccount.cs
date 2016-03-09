using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Social.Json.Extensions.JObject;

namespace Skybrud.Umbraco.Dashboard.Model.Analytics.Cached {
    
    public class AnalyticsCachedAccount {

        [JsonIgnore]
        public AnalyticsCachedUser User { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("properties")]
        public AnalyticsCachedWebProperty[] WebProperties { get; set; }

        public static AnalyticsCachedAccount Parse(AnalyticsCachedUser user, JObject obj) {
            AnalyticsCachedAccount account = new AnalyticsCachedAccount();
            account.User = user;
            account.Id = obj.GetString("id");
            account.Name = obj.GetString("name");
            account.Created = obj.GetString("created");
            account.Updated = obj.GetString("updated");
            account.WebProperties = obj.GetArray("properties", x => AnalyticsCachedWebProperty.Parse(account, x));
            return account;
        }

        public bool IsMatch(string query) {
            return (Id + "  " + Name).ToLower().Contains(query);
        }

    }

}