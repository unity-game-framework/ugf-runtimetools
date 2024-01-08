using System;
using System.Collections.Generic;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeColumnOptions
    {
        public string PropertyPath { get; }
        public string DisplayName { get; }
        public IComparer<SerializedProperty> Comparer { get; set; } = TableTreeColumnPropertyComparer.Default;
        public ITableTreeColumnSearcher Searcher { get; set; } = TableTreeColumnSearcher.Default;

        public TableTreeColumnOptions(string propertyPath, string displayName)
        {
            if (string.IsNullOrEmpty(propertyPath)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyPath));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException("Value cannot be null or empty.", nameof(displayName));

            PropertyPath = propertyPath;
            DisplayName = displayName;
        }
    }
}
