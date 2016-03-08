using Skybrud.Umbraco.Dashboard.Interfaces;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.Dashboard.Model {

    /// <summary>
    /// Represents a page in Umbraco for which statistics can be accessed in the Dashboard.
    /// </summary>
    public class DashboardPage : IDashboardPage {

        #region Properties

        public IPublishedContent Content { get; private set; }

        public IDashboardSite Site { get; private set; }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Url { get; private set; }
        
        #endregion

        #region Member methods

        public IDashboardBlock[] GetBlocks() {
            return Site == null ? new IDashboardBlock[0] : Site.GetBlocksForPage(Id);
        }

        #endregion

        #region Constructors

        private DashboardPage(IPublishedContent content, IDashboardSite site) {
            Content = content;
            Site = site;
            Id = content.Id;
            Name = content.Name;
            Url = content.UrlWithDomain();
        }

        #endregion

        #region Static methods

        public static DashboardPage GetFromContent(IPublishedContent content, IDashboardSite site = null) {
            return content == null ? null : new DashboardPage(content, site);
        }

        #endregion

    }

}