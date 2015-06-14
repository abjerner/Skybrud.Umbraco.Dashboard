using System.Globalization;
using System.Web;
using Skybrud.Social.Google.Analytics.Interfaces;
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
                return System.IO.File.Exists(path) ? url + "?" + System.IO.File.GetLastWriteTime(path).Ticks : url;
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
    
    }

}