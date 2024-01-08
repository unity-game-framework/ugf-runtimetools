using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal static class TableTreeDrawerEditorUtility
    {
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

        public static void PropertyInsert(SerializedProperty serializedProperty, int index, Action<SerializedProperty> initializeHandler, object value = null)
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
    }
}
