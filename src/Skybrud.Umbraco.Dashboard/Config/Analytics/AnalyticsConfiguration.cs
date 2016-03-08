using Newtonsoft.Json.Linq;
using Skybrud.Social.Json.Extensions.JObject;

namespace Skybrud.Umbraco.Dashboard.Config.Analytics {
    
    public class AnalyticsConfiguration : AnalyticsObject {

        #region Properties

        public AnalyticsClientConfiguration[] Clients { get; private set; }

        #endregion

        #region Constructors

        private AnalyticsConfiguration(JObject obj) : base(obj) {
            Clients = obj.GetArray("clients", AnalyticsClientConfiguration.Parse) ?? new AnalyticsClientConfiguration[0];
        }

        #endregion

        #region Static methods

        public static AnalyticsConfiguration Parse(JObject obj) {
            return obj == null ? null : new AnalyticsConfiguration(obj);
        }

        #endregion

    }

    public class AnalyticsClientConfiguration : AnalyticsObject {

        #region Properties



        #endregion

        #region Constructors

        private AnalyticsClientConfiguration(JObject obj) : base(obj) {

        }

        #endregion

        #region Static methods

        public static AnalyticsClientConfiguration Parse(JObject obj) {
            return obj == null ? null : new AnalyticsClientConfiguration(obj);
        }

        #endregion

    }

}