using System.Web;

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
    
    }

}