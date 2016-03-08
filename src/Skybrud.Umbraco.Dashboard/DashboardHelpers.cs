using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Social.Google.Analytics.Interfaces;
using Skybrud.Umbraco.Dashboard.Extensions.JObject;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Umbraco.Core.Models.Membership;
using Umbraco.Web;

namespace Skybrud.Umbraco.Dashboard {

    /// <summary>
    /// Various helper methods, properties and similar for the dashboard.
    /// </summary>
    public class DashboardHelpers {

        /// <summary>
        /// Gets a string for the ISO 8601 date format.
        /// </summary>
        public const string IsoDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
        
        /// <summary>
        /// If <code>url</code> matches a local file, the timestamp of that file is appended as part of the query string. This is extremely useful for cache busting.
        /// </summary>
        /// <param name="url">The URL.</param>
        public static string GetCachableUrl(string url) {
            if (url != null && url.StartsWith("/") && !url.StartsWith("//")) {
                string path = HttpContext.Current.Server.MapPath(url);
                return File.Exists(path) ? url + "?" + File.GetLastWriteTime(path).Ticks : url;
            }
            return url;
        }

        /// <summary>
        /// Translates the phrase matching the name of the specified <code>field</code>.
        /// </summary>
        /// <param name="field">The field.</param>
        public static string Translate(IAnalyticsField field) {
            return DashboardTranslations.Instance.Translate(DashboardContext.Current.Culture, field.Name);
        }

        /// <summary>
        /// Translates the phrase matching the name of the specified <code>field</code>.
        /// </summary>
        /// <param name="culture">The culture to be used for the translation.</param>
        /// <param name="field">The field.</param>
        public static string Translate(CultureInfo culture, IAnalyticsField field) {
            return DashboardTranslations.Instance.Translate(culture, field.Name);
        }

        /// <summary>
        /// Translates the phrase with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the phrase.</param>
        /// <param name="args">The arguments to be inserted into the phrase.</param>
        public static string Translate(string key, params string[] args) {
            return DashboardTranslations.Instance.Translate(DashboardContext.Current.Culture, key, args);
        }

        /// <summary>
        /// Translates the phrase with the specified <code>key</code>.
        /// </summary>
        /// <param name="culture">The culture to be used for the translation.</param>
        /// <param name="key">The key of the phrase.</param>
        /// <param name="args">The arguments to be inserted into the phrase.</param>
        public static string Translate(CultureInfo culture, string key, params string[] args) {
            return DashboardTranslations.Instance.Translate(culture, key, args);
        }

        /// <summary>
        /// Ensures that the Dashboard uses the culture of the current user.
        /// </summary>
        public static void EnsureCurrentUserCulture() {
            DashboardContext.Current.Culture = GetCurrentUserCulture();
        }

        /// <summary>
        /// Gets the culture of the current user, or default if not logged in.
        /// </summary>
        /// <returns>The culture of the current user.</returns>
        public static CultureInfo GetCurrentUserCulture() {
            IUser user = UmbracoContext.Current.Security.CurrentUser;
            if (user != null) {
                switch (user.Language) {
                    case "da":
                    case "da-DK":
                    case "da_DK":
                        return new CultureInfo("da-DK");
                }
            }
            return new CultureInfo("en-GB");
        }

        /// <summary>
        /// Static class for working with user settings in relation to the Dashboard.
        /// </summary>
        public static class UserSettings {

            /// <summary>
            /// Sets the default <code>site</code> of the specified backoffice <code>user</code>.
            /// </summary>
            /// <param name="user">The user.</param>
            /// <param name="site">The site.</param>
            public static void SetDefaultSite(IUser user, IDashboardSite site) {
                if (user == null || site == null) return;
                SetValue(user, "site", site.Id);
            }

            /// <summary>
            /// Gets the ID of the default site of the specified backoffice <code>user</code>.
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public static int GetDefaultSiteId(IUser user) {
                return GetInt32(user, "site");
            }

            private static JObject GetUserSettings(IUser user) {

                // Declare the path to the settings file of the specified user
                string path = HttpContext.Current.Server.MapPath(DashboardConstants.UserSettingsCachePath + user.Id + ".json");

                // Return an empty JObject if the file isn't found
                if (!File.Exists(path)) return new JObject();

                // Read the raw contents of the file
                string contents = File.ReadAllText(path);

                // Parse the contents into an instance of JObject
                return JObject.Parse(contents).GetObject("settings") ?? new JObject();

            }

            /// <summary>
            /// Gets the integer value of the setting matching the specified <code>user</code> and <code>propertyName</code>.
            /// </summary>
            /// <param name="user">The user.</param>
            /// <param name="propertyName">The name of the property.</param>
            /// <returns>Returns the integer (32-bit) value of the settings property.</returns>
            public static int GetInt32(IUser user, string propertyName) {
                return GetUserSettings(user).GetInt32(propertyName);
            }

            /// <summary>
            /// Gets the string value of the setting matching the specified <code>user</code> and <code>propertyName</code>.
            /// </summary>
            /// <param name="user">The user.</param>
            /// <param name="propertyName">The name of the property.</param>
            /// <returns>Returns the integer (32-bit) value of the settings property.</returns>
            public static string GetString(User user, string propertyName) {
                return GetUserSettings(user).GetString(propertyName);
            }

            public static void SetValue(IUser user, string propertyName, object value) {

                string path2 = HttpContext.Current.Server.MapPath(DashboardConstants.UserSettingsCachePath);
                string path3 = HttpContext.Current.Server.MapPath(DashboardConstants.UserSettingsCachePath + user.Id + ".json");

                if (!Directory.Exists(path2)) Directory.CreateDirectory(path2);

                // Load or initialize the root JObject
                JObject obj = File.Exists(path3) ? JObject.Parse(File.ReadAllText(path3, Encoding.UTF8)) : new JObject {
                    {"id", user.Id},
                    {"username", user.Username},
                    {"settings", new JObject()}
                };

                // Get the settings object
                JObject settings = obj.GetObject("settings");

                // Get the original JSON value for comparison
                string original = obj.ToString(Formatting.Indented);

                // Set the property value
                settings[propertyName] = value == null ? null : JToken.FromObject(value);

                // Serialize the new JSON value
                string serialized = obj.ToString(Formatting.Indented);

                // Update the file if the value has changed
                if (original != serialized) {
                    File.WriteAllText(path3, serialized, Encoding.UTF8);
                }

            }

        }
    
    }

}