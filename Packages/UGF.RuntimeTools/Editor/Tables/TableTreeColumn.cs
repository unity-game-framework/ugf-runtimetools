using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableTreeColumn : ITableTreeColumn
    {
        public GUIContent DisplayName { get; }
        public IComparer<SerializedProperty> Comparer { get; set; } = TableTreeColumnPropertyComparer.Default;
        public ITableTreeColumnSearcher Searcher { get; set; } = TableTreeColumnSearcher.Default;

        protected TableTreeColumn(GUIContent displayName)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }
    }
}
