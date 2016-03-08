using System.IO;
using Newtonsoft.Json.Linq;

namespace Skybrud.Umbraco.Dashboard.Config {
    
    /// <summary>
    /// Class representign the root configuration element for the Dashboard.
    /// </summary>
    public class DashboardConfiguration {

        #region Properties

        /// <summary>
        /// Gets a reference to the <see cref="JObject"/> the configuration was parsed from.
        /// </summary>
        public JObject JObject { get; private set; }

        #endregion

        #region Constructors

        private DashboardConfiguration(JObject obj) {
            JObject = obj;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Loads the configuration from the specified absolute <code>path</code>.
        /// </summary>
        /// <param name="path">The absolute path to the configuration file.</param>
        /// <returns>Returns an instance of <see cref="DashboardConfiguration"/>.</returns>
        public static DashboardConfiguration Load(string path) {
            
            // Load the contents of the file
            string contents = File.ReadAllText(path);

            // Parse the contents into an instance of JObject
            JObject obj = JObject.Parse(contents);

            // Initialize a new configuration instance
            return new DashboardConfiguration(obj);

        }

        #endregion

    }

}