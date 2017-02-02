namespace Skybrud.Umbraco.Dashboard.Constants {

    /// <summary>
    /// ENum class with representing various errors returned by the dashboard.
    /// </summary>
    public enum DashboardError {

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
        AnalyticsNoUsers

    }

}