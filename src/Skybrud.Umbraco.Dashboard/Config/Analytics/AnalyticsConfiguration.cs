using System.Linq;
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

        #region Member methods

        /// <summary>
        /// Gets the user configuration matching the specified <code>userId</code>. A user may have authenticated with
        /// more than one of the clients in the configuration - if that is the case, this method will simply return the
        /// configuration matching the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns an intstance of <see cref="AnalyticsUserConfiguration"/>, or <code>null</code> if not found.</returns>
        public AnalyticsUserConfiguration GetUserById(string userId) {
            return Clients.SelectMany(client => client.Users.Where(user => user.Id == userId)).FirstOrDefault();
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