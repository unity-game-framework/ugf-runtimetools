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
        public static void ShowWindow(SerializedProperty serializedProperty)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));

            var window = EditorWindow.GetWindow<TableTreeWindow>(serializedProperty.serializedObject.targetObject.name, false);

            window.SetSerializedProperty(serializedProperty);
            window.Show();
        }

        public static TableTreeViewState CreateState(SerializedProperty serializedProperty)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));

            return new TableTreeViewState
            {
                Header = CreateHeaderState((TableAsset)serializedProperty.serializedObject.targetObject)
            };
        }

        public static MultiColumnHeaderState CreateHeaderState(TableAsset tableAsset)
        {
            if (tableAsset == null) throw new ArgumentNullException(nameof(tableAsset));

            ITable table = tableAsset.Get();

            return CreateHeaderStateFromTableType(table.GetType());
        }

        public static MultiColumnHeaderState CreateHeaderStateFromTableType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments.Length != 1)
            {
                throw new ArgumentException($"Table header state can be created from '{typeof(Table<>)}' generic type only.");
            }

            return CreateHeaderStateFromEntryType(genericArguments[0]);
        }

        public static MultiColumnHeaderState CreateHeaderStateFromEntryType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var columns = new List<MultiColumnHeaderState.Column>();

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];

                if (field.IsDefined(typeof(SerializeField)))
                {
                    columns.Add(new MultiColumnHeaderState.Column
                    {
                        headerContent = new GUIContent(ObjectNames.NicifyVariableName(field.Name))
                    });
                }
            }

            return new MultiColumnHeaderState(columns.ToArray());
        }
    }
}
