using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public static class TableTreeEditorUtility
    {
        public static TableTreeWindow ShowWindow(TableAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            TableTreeOptions options = CreateOptions(asset.Get().GetType());

            return ShowWindow(asset, options);
        }

        public static TableTreeWindow ShowWindow(TableAsset asset, TableTreeOptions options)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (!TryGetWindow(asset, out TableTreeWindow window))
            {
                window = EditorWindow.CreateWindow<TableTreeWindow>(asset.name, typeof(TableTreeWindow));
            }

            window.minSize = new Vector2(500F, 500F);
            window.SetTarget(asset, options);
            window.Show();
            window.Focus();

            return window;
        }

        public static bool TryGetWindow(TableAsset asset, out TableTreeWindow window)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            TableTreeWindow[] windows = Resources.FindObjectsOfTypeAll<TableTreeWindow>();

            for (int i = 0; i < windows.Length; i++)
            {
                window = windows[i];

                if (window.HasSerializedObject && window.SerializedObject.targetObject == asset)
                {
                    return true;
                }
            }

            window = default;
            return false;
        }

        public static TableTreeViewState CreateState(TableTreeOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var columns = new MultiColumnHeaderState.Column[options.Columns.Count];

            for (int i = 0; i < options.Columns.Count; i++)
            {
                TableTreeColumnOptions column = options.Columns[i];

                columns[i] = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(column.DisplayName)
                };
            }

            return new TableTreeViewState
            {
                Header = new MultiColumnHeaderState(columns)
            };
        }

        public static TableTreeOptions CreateOptions(Type tableType)
        {
            if (tableType == null) throw new ArgumentNullException(nameof(tableType));

            Type entryType = TableTreeEditorInternalUtility.GetTableEntryType(tableType);

            if (!TryGetEntryChildrenPropertyName(entryType, out string childrenPropertyName))
            {
                childrenPropertyName = "m_children";
            }

            List<TableTreeColumnOptions> columns = CreateColumnOptions(entryType);

            return new TableTreeOptions(columns)
            {
                PropertyChildrenName = childrenPropertyName
            };
        }

        public static List<TableTreeColumnOptions> CreateColumnOptions(Type entryType)
        {
            if (entryType == null) throw new ArgumentNullException(nameof(entryType));

            var columns = new List<TableTreeColumnOptions>();

            if (!TryGetEntryChildrenPropertyName(entryType, out string childrenPropertyName))
            {
                childrenPropertyName = "m_children";
            }

            List<FieldInfo> fields = TableTreeEditorInternalUtility.GetSerializedFields(entryType);

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];

                string displayName = ObjectNames.NicifyVariableName(field.Name);

                columns.Add(new TableTreeColumnOptions(field.Name, displayName, TableTreeEntryType.Entry));

                if (field.Name == childrenPropertyName)
                {
                    Type type = TableTreeEditorInternalUtility.GetTableEntryChildrenType(field.FieldType);

                    CreateColumnOptionsFromFields(columns, TableTreeEditorInternalUtility.GetSerializedFields(type), TableTreeEntryType.Child);
                }
            }

            return columns;
        }

        public static void CreateColumnOptionsFromFields(ICollection<TableTreeColumnOptions> columns, IReadOnlyList<FieldInfo> fields, TableTreeEntryType entryType)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            for (int i = 0; i < fields.Count; i++)
            {
                FieldInfo field = fields[i];

                string displayName = ObjectNames.NicifyVariableName(field.Name);

                columns.Add(new TableTreeColumnOptions(field.Name, displayName, entryType));
            }
        }

        public static bool TryGetEntryChildrenPropertyName(Type entryType, out string propertyName)
        {
            if (entryType == null) throw new ArgumentNullException(nameof(entryType));

            var attribute = entryType.GetCustomAttribute<TableEntryChildrenAttribute>();

            if (attribute != null)
            {
                propertyName = attribute.PropertyName;
                return true;
            }

            propertyName = default;
            return false;
        }
    }
}
