using System.Collections;
using System.Collections.Generic;
using Skybrud.Umbraco.Dashboard.Interfaces;

namespace Skybrud.Umbraco.Dashboard.Plugins {

    /// <summary>
    /// Collection used for keeping track of the added dashboard plugins.
    /// </summary>
    public class DashboardPluginCollection : IEnumerable<IDashboardPlugin> {

        #region Private fields

        private readonly List<IDashboardPlugin> _list = new List<IDashboardPlugin>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the amount of plugins added to the collection.
        /// </summary>
        public int Count {
            get { return _list.Count; }
        }

        #endregion

        #region Constructors

        internal DashboardPluginCollection() { }

        #endregion

        #region Member methods

        /// <summary>
        /// Removes all plugins.
        /// </summary>
        public void Clear() {
            _list.Clear();
        }

        /// <summary>
        /// Adds the specified <code>plugin</code> to the end of the collection.
        /// </summary>
        /// <param name="plugin">The plugin top be added.</param>
        public void Add(IDashboardPlugin plugin) {
            _list.Add(plugin);
        }

        /// <summary>
        /// Adds the specified <code>plugin</code> to at <code>index</code> of the collection.
        /// </summary>
        /// <param name="plugin">The plugin top be added.</param>
        /// <param name="index">The index the plugin should be added at.</param>
        public void AddAt(IDashboardPlugin plugin, int index) {
            _list.Insert(index, plugin);
        }

        public IEnumerator<IDashboardPlugin> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

    }

}