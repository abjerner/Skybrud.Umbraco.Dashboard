using Skybrud.Umbraco.Dashboard.Models.Analytics.Selection;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Dashboard.PropertyEditors {
    
    /// <summary>
    /// Property value converter for the various property editors / data types defined by the Dashboard.
    /// </summary>
    public class DashboardPropertyValueConverter : IPropertyValueConverter {

        public bool IsConverter(PublishedPropertyType propertyType) {
            return propertyType.PropertyEditorAlias == "Skybrud.Dashboard.AnalyticsProfile";
        }

        public object ConvertDataToSource(PublishedPropertyType propertyType, object data, bool preview) {
            return DashboardAnalyticsSiteSelection.Deserialize(data as string);
        }

        public object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview) {
            return source;
        }

        public object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview) {
            return null;
        }

    }

}