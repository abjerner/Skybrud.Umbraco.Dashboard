using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Dashboard {
    
    /// <summary>
    /// Class describing a language for the dashboard.
    /// </summary>
    public class DashboardLanguage {

        /// <summary>
        /// Gets or sets the alias of the language.
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a dictionary with all the phrases of the language.
        /// </summary>
        [JsonProperty("phrases")]
        public Dictionary<string, string> Phrases { get; set; }

        /// <summary>
        /// Translates the phrase with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the phrase.</param>
        /// <param name="args">The arguments to be inserted into the phrase.</param>
        public string Translate(string key, string[] args) {
            string value;
            if (Phrases.TryGetValue(key, out value)) {
                return args == null || args.Length == 0 ? value : String.Format(value, args);
            }
            return null;
        }

    }

}