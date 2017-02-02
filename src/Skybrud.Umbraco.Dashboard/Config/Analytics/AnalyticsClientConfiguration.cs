using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;

namespace Skybrud.Umbraco.Dashboard.Config.Analytics {

    /// <summary>
    /// Class representing the configuration and OAuth information for a Google app/client.
    /// </summary>
    public class AnalyticsClientConfiguration : AnalyticsObject {

        #region Properties

        /// <summary>
        /// Gets the ID of the client. This ID is generated in Umbraco and used to uniquely identify the client within
        /// Umbraco without exposing the actual <see cref="ClientId"/>.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the OAuth 2.0 client ID.
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// Gets the OAuth 2.0 client secret.
        /// </summary>
        public string ClientSecret { get; private set; }

        /// <summary>
        /// Gets the redirect URI used for OAuth 2.0 authentication.
        /// </summary>
        public string RedirectUri { get; private set; }

        /// <summary>
        /// Gets an array of the Google users who have authenticated with this client.
        /// </summary>
        public AnalyticsUserConfiguration[] Users { get; private set; }

        /// <summary>
        /// Gets whether the any Google users have authenticated with this client.
        /// </summary>
        public bool HasUsers {
            get { return Users.Length > 0; }
        }

        #endregion

        #region Constructors

        private AnalyticsClientConfiguration(JObject obj) : base(obj) {
            Id = obj.GetString("id");
            ClientId = obj.GetString("clientId");
            ClientSecret = obj.GetString("clientSecret");
            RedirectUri = obj.GetString("redirectUri");
            Users = obj.GetArray("users", x => AnalyticsUserConfiguration.Parse(this, x));
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Parses the specified <code>obj</code> into an instance of <see cref="AnalyticsClientConfiguration"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> to be parsed.</param>
        /// <returns>Returns an instance of <see cref="AnalyticsClientConfiguration"/>.</returns>
        public static AnalyticsClientConfiguration Parse(JObject obj) {
            return obj == null ? null : new AnalyticsClientConfiguration(obj);
        }

        #endregion

    }

}