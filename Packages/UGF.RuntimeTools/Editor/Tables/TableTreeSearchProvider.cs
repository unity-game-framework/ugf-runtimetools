using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor.Search;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableTreeSearchProvider
    {
        [SearchItemProvider]
        public static SearchProvider GetProvider()
        {
            return new SearchProvider(nameof(TableTreeSearchProvider), "Table Entries")
            {
                filterId = "table:",
                fetchItems = OnFetchItems
            };
        }

        private static IEnumerable<SearchItem> OnFetchItems(SearchContext context, List<SearchItem> items, SearchProvider provider)
        {
            if (context.empty)
            {
                yield break;
            }

            IReadOnlyList<TableAsset> tables = TableEditorUtility.FindTableAssetAll(typeof(TableAsset));

            for (int i = 0; i < tables.Count; i++)
            {
                TableAsset table = tables[i];
                IReadOnlyList<ITableEntry> entries = table.Get().Entries;

                for (int e = 0; e < entries.Count; e++)
                {
                    ITableEntry entry = entries[e];

                    if (entry.Name.Contains(context.searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return provider.CreateItem(context, entry.Id.ToString(), entry.Name, null, null, null);
                    }
                }
            }
        }
    }
}
