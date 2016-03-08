using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Providers;
using Examine.SearchCriteria;
using Newtonsoft.Json;
using Skybrud.Umbraco.Dashboard.Interfaces;
using Umbraco.Core.Models.Membership;
using Umbraco.Web;

namespace Skybrud.Umbraco.Dashboard.Model.LastEdited {
    
    public class LastEditedItem {

        #region Properties
        
        /// <summary>
        /// Gets the ID of the page.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets the comma separated path of the page.
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets the name of the page.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets whether the page is currently published.
        /// </summary>
        [JsonProperty("published")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets the timestamp for when the page was created.
        /// </summary>
        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets the timestamp for when the page was last updated.
        /// </summary>
        [JsonProperty("updateDate")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets a textual representation for when the page was last updated.
        /// </summary>
        [JsonProperty("updated")]
        public string Updated {
            get {
                if (UpdateDate >= DateTime.Today) return DashboardHelpers.Translate("today");
                if (UpdateDate >= DateTime.Today.AddDays(-1)) return DashboardHelpers.Translate("yesterday");
                return UpdateDate.ToString("dd.MM.yy");
            }
        }

        #endregion

        #region Static methods
        
        /// <summary>
        /// Gets the 15 last edited pages for the specified <code>site</code>.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns>Returns an array of <see cref="LastEditedItem"/>.</returns>
        public static LastEditedItem[] GetLastEditedData(IDashboardSite site) {
            return GetLastEditedData(site, -1, 15);
        }

        public static LastEditedItem[] GetLastEditedData(IDashboardSite site, IUser user) {
            return GetLastEditedData(site, user == null ? -1 : user.Id, 15);
        }

        public static LastEditedItem[] GetLastEditedData(IDashboardSite site, User user, int max) {
            return GetLastEditedData(site, user == null ? -1 : user.Id, max);
        }

        public static LastEditedItem[] GetLastEditedData(IDashboardSite site, int userId) {
            return GetLastEditedData(site, -1, 15);
        }

        public static LastEditedItem[] GetLastEditedData(IDashboardSite site, int userId, int max) {

            // List for constructing the raw query
            List<string> query = new List<string>();

            // Get a reference to the internal searcher
            BaseSearchProvider externalSearcher = ExamineManager.Instance.SearchProviderCollection["InternalSearcher"];

            // Limit the search to pages under the specified site
            query.Add("sky_path:" + site.Id);

            // Limit the search to pages last edited by the specified user
            if (userId >= 0) query.Add("writerID:" + userId);

            // Initialize the criteria for the Examine search
            ISearchCriteria criteria = externalSearcher.CreateSearchCriteria().RawQuery(String.Join(" AND ", query));

            // Make the actual search in Examine
            ISearchResults results = externalSearcher.Search(criteria);

            // Order the results (and limit the amount of results)
            IEnumerable<SearchResult> sorted = results.OrderByDescending(x => x.Fields["updateDate"]).Take(max);

            return (
                from result in sorted
                select new LastEditedItem {
                    Id = result.Id,
                    CreateDate = ParseExamineDate(result.Fields["createDate"]),
                    UpdateDate = ParseExamineDate(result.Fields["updateDate"]),
                    IsPublished = UmbracoContext.Current.ContentCache.GetById(result.Id) != null,
                    Name = result.Fields["nodeName"],
                    Path = result.Fields["path"]
                }
            ).ToArray();

        }

        private static DateTime ParseExamineDate(string str) {
            return DateTime.ParseExact(str, "yyyyMMddHHmmssfff", null);
        }

        #endregion

    }

}