using System;
using System.Collections.Generic;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public struct TableTreeDrawerColumn
    {
        public string PropertyName { get; }
        public string DisplayName { get; }
        public IComparer<SerializedProperty> PropertyComparer { get; set; }
        public ITableTreeDrawerColumnSearchHandler SearchHandler { get; set; }
        public float MinWidth { get; set; }

        public TableTreeDrawerColumn(string propertyName) : this(propertyName, ObjectNames.NicifyVariableName(propertyName))
        {
        }

        public TableTreeDrawerColumn(string propertyName, string displayName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyName));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException("Value cannot be null or empty.", nameof(displayName));

            PropertyName = propertyName;
            DisplayName = displayName;
            PropertyComparer = default;
            SearchHandler = default;
            MinWidth = 20F;
        }
    }
}
