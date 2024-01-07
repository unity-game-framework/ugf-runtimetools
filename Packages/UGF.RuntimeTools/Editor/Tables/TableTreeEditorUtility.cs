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
        public static void ShowWindow(TableAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            var window = EditorWindow.GetWindow<TableTreeWindow>(asset.name, false);

            window.minSize = new Vector2(500F, 500F);
            window.SetTarget(asset);
            window.Show();
        }

        public static void ShowWindow(TableAsset asset, ITableTree tableTree)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (tableTree == null) throw new ArgumentNullException(nameof(tableTree));

            var window = EditorWindow.GetWindow<TableTreeWindow>(asset.name, false);

            window.minSize = new Vector2(500F, 500F);
            window.SetTarget(asset, tableTree);
            window.Show();
        }

        public static ITableTree CreateTableTree(SerializedObject serializedObject)
        {
            if (serializedObject == null) throw new ArgumentNullException(nameof(serializedObject));

            Type type = TableTreeEditorInternalUtility.GetTableEntryType(((TableAsset)serializedObject.targetObject).Get().GetType());
            List<FieldInfo> fields = TableTreeEditorInternalUtility.GetEntryFields(type);
            var columns = new List<ITableTreeColumn>();

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];
                var displayName = new GUIContent(ObjectNames.NicifyVariableName(field.Name));

                columns.Add(new TableTreeColumnProperty(displayName, field.Name));
            }

            return new TableTreeProperty(serializedObject.FindProperty("m_table.m_entries"), columns);
        }
    }
}
