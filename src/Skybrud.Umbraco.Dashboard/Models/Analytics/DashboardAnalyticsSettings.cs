using System;

namespace Skybrud.Umbraco.Dashboard.Models.Analytics {
    
    public class DashboardAnalyticsSettings {
        
        public string UserId { get; set; }
        
        public string UserEmail { get; set; }
        
        public string UserName { get; set; }

        public DateTime Authenticated { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RefreshToken { get; set; }

        public string ProfileId { get; set; }

        public bool HasProfileData {
            get { return !String.IsNullOrWhiteSpace(ProfileId); }
        }

    }

}