using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Newtonsoft.Json;
using Umbraco.Core.Logging;

namespace Skybrud.Umbraco.Dashboard {

    /// <summary>
    /// Class for handling translations in the Dashboard.
    /// </summary>
    public class DashboardTranslations {

        /// <summary>
        /// Gets an array of all languages in the translations file.
        /// </summary>
        public DashboardLanguage[] Languages { get; private set; }

        /// <summary>
        /// Gets a reference to the translations singleton.
        /// </summary>
        public static DashboardTranslations Instance {

            get {

                string cacheKey = typeof(DashboardTranslations).FullName;

                // Declare the path to the file
                string path = HttpContext.Current.Server.MapPath("~/App_Plugins/Skybrud.Dashboard/Translations.json");

                // Attempt to get the instance from the cache
                DashboardTranslations cfg = HttpContext.Current.Cache[cacheKey] as DashboardTranslations;

                if (cfg == null) {

                    // Load the JSON file
                    cfg = Load(path);

                    // Add the instance to the cache (with a dependency on the config file)
                    HttpContext.Current.Cache.Insert(cacheKey, cfg, new CacheDependency(path));

                }

                return cfg;

            }

        }

        /// <summary>
        /// Translates the phrase with the specified <code>key</code>.
        /// </summary>
        /// <param name="culture">The culture to be used for the translation.</param>
        /// <param name="key">The key of the phrase.</param>
        /// <param name="args">The arguments to be inserted into the phrase.</param>
        public string Translate(CultureInfo culture, string key, params string[] args) {
            var language = Languages.FirstOrDefault(x => x.Alias == culture.Name);
            string value = language == null ? null : language.Translate(key, args);
            return value ?? "$(" + key + ")";
        }

        /// <summary>
        /// Initializes a new translations context from the specified <code>path</code>.
        /// </summary>
        /// <param name="path">The path to the translations file.</param>
        public static DashboardTranslations Load(string path) {
            try {
                return new DashboardTranslations {
                    Languages = JsonConvert.DeserializeObject<DashboardLanguage[]>(File.ReadAllText(path))
                };
            } catch (Exception ex) {
                LogHelper.Error<DashboardTranslations>("Unable to load translations", ex);
                return new DashboardTranslations {
                    Languages = new DashboardLanguage[0]
                };
            }
        }

    }

}