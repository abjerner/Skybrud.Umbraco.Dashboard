using System;
using System.Collections.Generic;
using Skybrud.Umbraco.Dashboard.Model;

namespace Skybrud.Umbraco.Dashboard.Extensions {
    
    /// <summary>
    /// Various extension methods for <see cref="List&lt;DashboardTab&gt;"/>
    /// </summary>
    public static class DashboardTabListExtensions {
        
        /// <summary>
        /// Removes all tabs matching the specified <code>predicate</code>.
        /// </summary>
        /// <param name="list">The list of tabs.</param>
        /// <param name="predicate">The predicate used for matching the tabs.</param>
        public static void Remove(this List<DashboardTab> list, Func<DashboardTab, bool> predicate) {
            foreach (DashboardTab item in list.ToArray()) {
                if (predicate(item)) {
                    list.Remove(item);
                }
            }
        }

    }

}