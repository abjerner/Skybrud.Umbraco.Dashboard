using Newtonsoft.Json.Linq;
using Skybrud.Social.Json.Extensions.JObject;

namespace Skybrud.Umbraco.Dashboard.Config.Analytics {

    /// <summary>
    /// Class representing the root configuration element for the Analytics implementation of the Dashboard.
    /// </summary>
    public class AnalyticsConfiguration : AnalyticsObject {

        #region Properties

        /// <summary>
        /// Gets an array of Google OAuth 2.0 client added to the Analytics configuration.
        /// </summary>
        public AnalyticsClientConfiguration[] Clients { get; private set; }

        #endregion

        #region Constructors

        private AnalyticsConfiguration(JObject obj) : base(obj) {
            Clients = obj.GetArray("clients", AnalyticsClientConfiguration.Parse) ?? new AnalyticsClientConfiguration[0];
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Parses the specified <code>obj</code> into an instance of <see cref="AnalyticsConfiguration"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> to be parsed.</param>
        /// <returns>Returns an instance of <see cref="AnalyticsConfiguration"/>.</returns>
        public static AnalyticsConfiguration Parse(JObject obj) {
            return obj == null ? null : new AnalyticsConfiguration(obj);
        }

        #endregion

    }

}