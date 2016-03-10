using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Social.Google.Analytics.Objects;
using Skybrud.Social.Google.Analytics.Responses.Management;
using Skybrud.Social.Json.Extensions.JObject;
using Skybrud.Umbraco.Dashboard.Config.Analytics;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics.Cached {
    
    public class AnalyticsCachedUser {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("accounts")]
        public AnalyticsCachedAccount[] Accounts { get; set; }

        public static AnalyticsCachedUser GetFromResponses(AnalyticsUserConfiguration user, AnalyticsAccountsResponse response1, AnalyticsWebPropertiesResponse response2, AnalyticsProfilesResponse response3) {

            Dictionary<string, List<AnalyticsWebProperty>> webProperties = new Dictionary<string, List<AnalyticsWebProperty>>();
            Dictionary<string, List<AnalyticsProfile>> profiles = new Dictionary<string, List<AnalyticsProfile>>();

            // Add the web properties to a dictionary for faster lookups
            foreach (AnalyticsWebProperty webProperty in response2.Body.Items) {
                List<AnalyticsWebProperty> list;
                if (!webProperties.TryGetValue(webProperty.AccountId, out list)) {
                    webProperties.Add(webProperty.AccountId, list = new List<AnalyticsWebProperty>());
                }
                list.Add(webProperty);
            }

            // Add the profiles to a dictionary for faster lookups
            foreach (AnalyticsProfile profile in response3.Body.Items) {
                List<AnalyticsProfile> list;
                if (!profiles.TryGetValue(profile.WebPropertyId, out list)) {
                    profiles.Add(profile.WebPropertyId, list = new List<AnalyticsProfile>());
                }
                list.Add(profile);
            }







            AnalyticsCachedUser cachedUser = new AnalyticsCachedUser {
                Id = user.Id
            };


            List<AnalyticsCachedAccount> cachedAccounts = new List<AnalyticsCachedAccount>();

            foreach (AnalyticsAccount account in response1.Body.Items) {

                List<AnalyticsCachedWebProperty> list11 = new List<AnalyticsCachedWebProperty>();

                List<AnalyticsWebProperty> list1;
                if (!webProperties.TryGetValue(account.Id, out list1)) {
                    list1 = new List<AnalyticsWebProperty>();
                }

                AnalyticsCachedAccount ca = new AnalyticsCachedAccount {
                    User = null,
                    Id = account.Id,
                    Name = account.Name,
                    Created = account.JsonObject.GetString("created"),
                    Updated = account.JsonObject.GetString("updated")
                };

                foreach (AnalyticsWebProperty webProperty in list1) {

                    List<AnalyticsProfile> list2;
                    if (!profiles.TryGetValue(webProperty.Id, out list2)) {
                        list2 = new List<AnalyticsProfile>();
                    }

                    AnalyticsCachedWebProperty cwp = new AnalyticsCachedWebProperty {
                        Id = webProperty.Id,
                        Name = webProperty.Name,
                        WebsiteUrl = webProperty.WebsiteUrl,
                        Created = webProperty.Created.ToString("r"),
                        Updated = webProperty.Updated.ToString("r")
                    };

                    cwp.Profiles = list2.Select(profile => new AnalyticsCachedProfile {
                        WebProperty = cwp,
                        Id = profile.Id,
                        Name = profile.Name,
                        Currency = profile.Currency,
                        Timezone = profile.Timezone,
                        WebsiteUrl = profile.WebsiteUrl,
                        Type = profile.Type,
                        Created = profile.JsonObject.GetString("created"),
                        Updated = profile.JsonObject.GetString("updated")
                    }).ToArray();

                    list11.Add(cwp);

                }

                ca.WebProperties = list11.ToArray();

                cachedAccounts.Add(ca);

            }

            cachedUser.Accounts = cachedAccounts.ToArray();

            return cachedUser;

        }

        public void Save(string path) {
            Save(path, Formatting.None);
        }

        public void Save(string path, Formatting formatting) {
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(this, formatting));
        }

        public void Save(AnalyticsUserConfiguration user) {
            Save(user, Formatting.None);
        }

        public void Save(AnalyticsUserConfiguration user, Formatting formatting) {
            System.IO.File.WriteAllText(
                DashboardContext.Current.MapPath(DashboardConstants.AnalyticsCachePath + "/Users/" + user.Id + ".json"),
                JsonConvert.SerializeObject(this, formatting)
            );
        }

        public static AnalyticsCachedUser Load(string path) {
            return Parse(JObject.Parse(System.IO.File.ReadAllText(path)));
        }

        public static AnalyticsCachedUser Load(AnalyticsUserConfiguration user) {
            if (user == null) throw new ArgumentNullException("user");
            string path = DashboardContext.Current.MapPath(DashboardConstants.AnalyticsCachePath + "/Users/" + user.Id + ".json");
            return Parse(JObject.Parse(System.IO.File.ReadAllText(path)));
        }

        public static AnalyticsCachedUser Parse(JObject obj) {
            AnalyticsCachedUser user = new AnalyticsCachedUser();
            user.Id = obj.GetString("id");
            user.Accounts = obj.GetArray("accounts", x => AnalyticsCachedAccount.Parse(user, x));
            return user;
        }

    }
}
