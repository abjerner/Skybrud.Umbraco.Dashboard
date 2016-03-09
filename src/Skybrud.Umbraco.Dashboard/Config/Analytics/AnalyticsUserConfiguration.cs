using System;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Dashboard.Extensions.JObject;

namespace Skybrud.Umbraco.Dashboard.Config.Analytics {
    
    /// <summary>
    /// Class representing the configuration and authentication information for a Google user.
    /// </summary>
    public class AnalyticsUserConfiguration : AnalyticsObject {

        #region Properties

        /// <summary>
        /// Gets a reference to the parent client configuration.
        /// </summary>
        public AnalyticsClientConfiguration Client { get; private set; }

        /// <summary>
        /// Gets the Google user ID of the authenticated user.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the email address of the authenticated user.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the name of the authenticated user.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets whether a name has been specified by the authenticated Google user.
        /// </summary>
        public bool HasName {
            get { return !String.IsNullOrWhiteSpace(Name); }
        }

        /// <summary>
        /// Gets the refresh token of the authenticated user. The refresh token can along with the client ID and client
        /// secret of <see cref="Client"/> be used to obtain new access tokens, which again can be used to make calls
        /// to the Google Analytics API.
        /// </summary>
        public string RefreshToken { get; private set; }

        #endregion

        #region Constructors

        private AnalyticsUserConfiguration(AnalyticsClientConfiguration client, JObject obj) : base(obj) {
            Client = client;
            Id = obj.GetString("id");
            Email = obj.GetString("email");
            Name = obj.GetString("name");
            RefreshToken = obj.GetString("refreshToken");
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Parses the specified <code>obj</code> into an instance of <see cref="AnalyticsUserConfiguration"/>.
        /// </summary>
        /// <param name="client">The parent client configuration.</param>
        /// <param name="obj">The instance of <see cref="JObject"/> to be parsed.</param>
        /// <returns>Returns an instance of <see cref="AnalyticsUserConfiguration"/>.</returns>
        public static AnalyticsUserConfiguration Parse(AnalyticsClientConfiguration client, JObject obj) {
            return obj == null ? null : new AnalyticsUserConfiguration(client, obj);
        }

        #endregion

    }

}