using Newtonsoft.Json.Linq;

namespace Skybrud.Umbraco.Dashboard.Config.Analytics {
    
    public class AnalyticsAccountConfiguration : AnalyticsObject {

        #region Properties



        #endregion

        #region Constructors

        private AnalyticsAccountConfiguration(JObject obj) : base(obj) {

        }

        #endregion

        #region Static methods

        public static AnalyticsAccountConfiguration Parse(JObject obj) {
            return obj == null ? null : new AnalyticsAccountConfiguration(obj);
        }

        #endregion

    }

}