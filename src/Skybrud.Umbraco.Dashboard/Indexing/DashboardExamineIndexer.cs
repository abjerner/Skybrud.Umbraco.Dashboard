using Examine;
using Examine.Providers;

namespace Skybrud.Umbraco.Dashboard.Indexing {

    /// <summary>
    /// Examine indexer specific to the Dashboard.
    /// </summary>
    public class DashboardExamineIndexer {

        private DashboardExamineIndexer() {
            BaseIndexProvider indexer = ExamineManager.Instance.IndexProviderCollection["InternalIndexer"];
            indexer.GatheringNodeData += internalIndexer_GatheringNodeData;
        }

        private void internalIndexer_GatheringNodeData(object sender, IndexingNodeDataEventArgs e) {

            // Make the path searchable
            string path;
            e.Fields.TryGetValue("path", out path);
            e.Fields.Add("sky_path", (path + "").Replace(',', ' '));

        }

        /// <summary>
        /// Initializes the indexer.
        /// </summary>
        /// <returns>Returns the initialized indexer.</returns>
        public static DashboardExamineIndexer Init() {
            return new DashboardExamineIndexer();
        }

    }

}
