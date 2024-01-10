using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableTreeEditorInternalUtility
    {
        public static bool IsSingleFieldProperty(SerializedProperty serializedProperty)
        {
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Boolean:
                case SerializedPropertyType.Float:
                case SerializedPropertyType.String:
                case SerializedPropertyType.ObjectReference:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.Character:
                case SerializedPropertyType.AnimationCurve:
                case SerializedPropertyType.Gradient:
                case SerializedPropertyType.ManagedReference: return true;
                default: return false;
            }
        }

        public static void PropertyInsert(SerializedProperty serializedProperty, IReadOnlyList<int> indexes, Action<SerializedProperty> initializeHandler = null)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            if (indexes is List<int> list)
            {
                list.Sort();
            }

            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                int index = indexes[i];

                PropertyInsert(serializedProperty, index, initializeHandler);
            }
        }

        public static void PropertyInsert(SerializedProperty serializedProperty, int index, object value)
        {
            PropertyInsert(serializedProperty, index, null, value);
        }

        public static void PropertyInsert(SerializedProperty serializedProperty, int index, Action<SerializedProperty> initializeHandler = null, object value = null)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));

            serializedProperty.InsertArrayElementAtIndex(index);

            index = Mathf.Min(index + 1, serializedProperty.arraySize - 1);

            SerializedProperty propertyElement = serializedProperty.GetArrayElementAtIndex(index);

            if (value != null)
            {
                propertyElement.boxedValue = value;
            }

            initializeHandler?.Invoke(propertyElement);
        }

        public static void PropertyRemove(SerializedProperty serializedProperty, IReadOnlyList<int> indexes)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));
            if (indexes == null) throw new ArgumentNullException(nameof(indexes));

            if (indexes is List<int> list)
            {
                list.Sort();
            }

            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                int index = indexes[i];

                serializedProperty.DeleteArrayElementAtIndex(index);
            }
        }

        public static int GetEntryId(SerializedProperty serializedProperty, TableTreeOptions options)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));
            if (options == null) throw new ArgumentNullException(nameof(options));

            SerializedProperty propertyId = serializedProperty.FindPropertyRelative(options.PropertyIdName);
            GlobalId id = GlobalIdEditorUtility.GetGlobalIdFromProperty(propertyId);

            return (int)id.First;
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

        public static List<FieldInfo> GetSerializedFields(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var fields = new List<FieldInfo>();

            while (type != null)
            {
                foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (field.IsPublic || field.IsDefined(typeof(SerializeField)) || field.IsDefined(typeof(SerializeReference)))
                    {
                        fields.Add(field);
                    }
                }

                type = type.BaseType;
            }

            fields.Sort(TableTreeEntryFieldComparer.Default);

            return fields;
        }

        public static Type GetTableEntryType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments.Length != 1)
            {
                throw new ArgumentException("Table entry type is unknown.");
            }

            return genericArguments[0];
        }

        public static Type GetTableEntryChildrenType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type[] genericArguments = type.GetGenericArguments();

                return genericArguments[0];
            }

            throw new ArgumentException("Table entry children type is unknown.");
        }
    }
}
