using Newtonsoft.Json.Linq;

namespace Skybrud.Umbraco.Dashboard.Config.Analytics {
    
    public class AnalyticsClientConfiguration : AnalyticsObject {

        #region Properties

        public string Id { get; private set; }

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public AnalyticsAccountConfiguration[] Users { get; private set; }

        public bool HasUsers {
            get { return Users.Length > 0; }
        }

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