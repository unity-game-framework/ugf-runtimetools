using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public static class TableTreeEditorUtility
    {
        public static void ShowWindow(TableAsset tableAsset)
        {
            if (tableAsset == null) throw new ArgumentNullException(nameof(tableAsset));

            ShowWindow(tableAsset, GetEntryColumns(tableAsset));
        }

        public static void ShowWindow(TableAsset tableAsset, IReadOnlyList<TableTreeDrawerColumn> columns)
        {
            if (tableAsset == null) throw new ArgumentNullException(nameof(tableAsset));
            if (columns == null) throw new ArgumentNullException(nameof(columns));

            var window = EditorWindow.GetWindow<TableTreeWindow>(tableAsset.name, false);

            window.minSize = new Vector2(500F, 500F);
            window.SetTarget(tableAsset, columns);
            window.Show();
        }

        public static List<TableTreeDrawerColumn> GetEntryColumns(TableAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            ITable table = asset.Get();
            Type tableEntryType = TableTreeEditorInternalUtility.GetTableEntryType(table.GetType());

            return GetEntryColumns(tableEntryType);
        }

        public static List<TableTreeDrawerColumn> GetEntryColumns(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var columns = new List<TableTreeDrawerColumn>();
            List<FieldInfo> fields = TableTreeEditorInternalUtility.GetEntryFields(type);

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];

                columns.Add(new TableTreeDrawerColumn(field.Name));
            }

            return columns;
        }
    }
}
