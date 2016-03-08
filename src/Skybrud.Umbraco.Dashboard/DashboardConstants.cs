namespace Skybrud.Umbraco.Dashboard {
    
    /// <summary>
    /// Class with various constants used throughout the Dashboard.
    /// </summary>
    public static class DashboardConstants {

        /// <summary>
        /// Gets the virtual path to the temporary directory used by the Dashboard.
        /// </summary>
        public const string DashboardCachePath = "~/App_Data/TEMP/Skybrud.Dashboard/";

        /// <summary>
        /// Gets the virtual path to the temporary directory used by the part for Google Analytics.
        /// </summary>
        public const string AnalyticsCachePath = "~/App_Data/TEMP/Skybrud.Dashboard/Analytics/";

        /// <summary>
        /// Gets the virtual path to the temporary directory used by the Dashboard for storing user settings.
        /// </summary>
        public const string UserSettingsCachePath = "~/App_Data/TEMP/Skybrud.Dashboard/UserSettings/";

    }

}