using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Skybrud.Umbraco.Dashboard.Config.Analytics {
    
    /// <summary>
    /// Class representing a basic object from the Google Analytics API derived from an instance of <see cref="JObject"/>.
    /// </summary>
    public class AnalyticsObject {

        #region Properties

        /// <summary>
        /// Gets a reference to the internal <see cref="JObject"/> the object was created from.
        /// </summary>
        [JsonIgnore]
        public JObject JObject { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance from the specified <code>obj</code>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> representing the object.</param>
        protected AnalyticsObject(JObject obj) {
            JObject = obj;
        }

        #endregion

    }

}