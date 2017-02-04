namespace Skybrud.Umbraco.Dashboard.Constants {

    /// <summary>
    /// Eeum class with representing various errors returned by the dashboard.
    /// </summary>
    public enum DashboardError {

        /// <summary>
        /// Indicates an unknown server error.
        /// </summary>
        UnknownServerError,

        /// <summary>
        /// Indicates that a specified site was not found.
        /// </summary>
        SiteNotFound,

        /// <summary>
        /// Indicates that the <code>analytics</code> property is missing in <code>~/config/Skybrud.Dashboard.config</code>.
        /// </summary>
        AnalyticsNotConfigured,

        /// <summary>
        /// Indicates that no clients have been configured for the Analytics configuration in <code>~/config/Skybrud.Dashboard.config</code>.
        /// </summary>
        AnalyticsNoClients,

        /// <summary>
        /// Indicates that no users have been configured for the Analytics configuration in <code>~/config/Skybrud.Dashboard.config</code>.
        /// </summary>
        AnalyticsNoUsers,

        /// <summary>
        /// Indicates that the specified site doesn't have a valid configuration for Google Analytics.
        /// </summary>
        AnalyticsSiteNotConfigured,

        /// <summary>
        /// Indicates a generic authentication error returned by the Google Analytics API.
        /// </summary>
        AnalyticsAuthError,

        /// <summary>
        /// Indicates that the used access token isn't valid or doesn't have the required permissions to fetch Google Analytics statistics.
        /// </summary>
        AnalyticsAuthInvalidGrant
    
    }

}