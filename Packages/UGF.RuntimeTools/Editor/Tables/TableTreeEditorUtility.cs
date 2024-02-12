using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UGF.EditorTools.Editor.Serialized;
using UGF.EditorTools.Runtime.Ids;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public static class TableTreeEditorUtility
    {
        public static TableTreeWindow ShowWindow(TableAsset asset, GlobalId focusItemId = default)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            TableTreeOptions options = CreateOptions(asset.Get().GetType());

            return ShowWindow(asset, options, focusItemId);
        }

        public static TableTreeWindow ShowWindow(TableAsset asset, TableTreeOptions options, GlobalId focusItemId = default)
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

            if (focusItemId.IsValid())
            {
                window.Drawer.TreeView.TryFocusAtItem(focusItemId);
            }

            window.Focus();
            window.Drawer.TreeView.SetFocusAndEnsureSelectedItem();

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

        public static void SetData(TableAsset asset, string tablePropertyName, DataTable data)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (string.IsNullOrEmpty(tablePropertyName)) throw new ArgumentException("Value cannot be null or empty.", nameof(tablePropertyName));

            using var serializedObject = new SerializedObject(asset);

            SerializedProperty propertyTable = serializedObject.FindProperty(tablePropertyName);
            TableTreeOptions options = CreateOptions(asset.Get().GetType());

            SetData(propertyTable, data, options);
        }

        public static void SetData(SerializedProperty serializedProperty, DataTable data, TableTreeOptions options)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (options == null) throw new ArgumentNullException(nameof(options));

            SerializedProperty propertyEntries = serializedProperty.FindPropertyRelative(options.PropertyEntriesName);

            propertyEntries.ClearArray();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];

                SerializedProperty propertyEntry = TableTreeEditorInternalUtility.PropertyInsert(propertyEntries, propertyEntries.arraySize);

                foreach (SerializedProperty entryProperty in SerializedPropertyEditorUtility.GetChildren(propertyEntry))
                {
                    if (entryProperty.name != options.PropertyChildrenName)
                    {
                        entryProperty.boxedValue = row[entryProperty.name];
                    }
                    else
                    {
                        object value = row[entryProperty.name];
                        int count = Convert.ToInt32(value);

                        for (int c = 0; c < count; c++)
                        {
                            row = data.Rows[++i];

                            SerializedProperty propertyChild = TableTreeEditorInternalUtility.PropertyInsert(entryProperty, entryProperty.arraySize);

                            foreach (SerializedProperty childProperty in SerializedPropertyEditorUtility.GetChildren(propertyChild))
                            {
                                childProperty.boxedValue = row[childProperty.name];
                            }
                        }
                    }
                }
            }
        }

        public static DataTable GetData(TableAsset asset, string tablePropertyName)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (string.IsNullOrEmpty(tablePropertyName)) throw new ArgumentException("Value cannot be null or empty.", nameof(tablePropertyName));

            using var serializedObject = new SerializedObject(asset);

            SerializedProperty propertyTable = serializedObject.FindProperty(tablePropertyName);
            TableTreeOptions options = CreateOptions(asset.Get().GetType());

            return GetData(propertyTable, options);
        }

        public static DataTable GetData(SerializedProperty serializedProperty, TableTreeOptions options)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));
            if (options == null) throw new ArgumentNullException(nameof(options));

            var data = new DataTable();

            for (int i = 0; i < options.Columns.Count; i++)
            {
                TableTreeColumnOptions column = options.Columns[i];

                data.Columns.Add(column.PropertyName);
            }

            SerializedProperty propertyEntries = serializedProperty.FindPropertyRelative(options.PropertyEntriesName);

            for (int i = 0; i < propertyEntries.arraySize; i++)
            {
                SerializedProperty propertyEntry = propertyEntries.GetArrayElementAtIndex(i);
                DataRow row = data.NewRow();

                int childrenCount = 0;

                foreach (SerializedProperty entryProperty in SerializedPropertyEditorUtility.GetChildren(propertyEntry))
                {
                    if (entryProperty.name != options.PropertyChildrenName)
                    {
                        row[entryProperty.name] = entryProperty.boxedValue;
                    }
                    else
                    {
                        row[entryProperty.name] = entryProperty.arraySize;

                        childrenCount = entryProperty.arraySize;

                        for (int c = 0; c < entryProperty.arraySize; c++)
                        {
                            SerializedProperty propertyChild = entryProperty.GetArrayElementAtIndex(c);
                            DataRow childRow = data.NewRow();

                            foreach (SerializedProperty childProperty in SerializedPropertyEditorUtility.GetChildren(propertyChild))
                            {
                                childRow[childProperty.name] = childProperty.boxedValue;
                            }

                            data.Rows.Add(childRow);
                        }
                    }
                }

                if (childrenCount > 0)
                {
                    data.Rows.InsertAt(row, data.Rows.Count - childrenCount);
                }
                else
                {
                    data.Rows.Add(row);
                }
            }

            return data;
        }
    }
}
