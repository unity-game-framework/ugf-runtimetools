using System;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeDrawerColumnSearchHandler : ITableTreeDrawerColumnSearchHandler
    {
        public static TableTreeDrawerColumnSearchHandler Default { get; } = new TableTreeDrawerColumnSearchHandler();

        public bool Check(SerializedProperty serializedProperty, string search)
        {
            if (serializedProperty == null) throw new ArgumentNullException(nameof(serializedProperty));
            if (search == null) throw new ArgumentNullException(nameof(search));

            return OnCheck(serializedProperty, search);
        }

        protected virtual bool OnCheck(SerializedProperty serializedProperty, string search)
        {
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Boolean:
                case SerializedPropertyType.Float:
                case SerializedPropertyType.String:
                case SerializedPropertyType.Color:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Rect:
                case SerializedPropertyType.Character:
                case SerializedPropertyType.Bounds:
                case SerializedPropertyType.Gradient:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.RectInt:
                case SerializedPropertyType.BoundsInt:
                case SerializedPropertyType.Hash128:
                {
                    object value = serializedProperty.boxedValue;
                    string text = value != null ? value.ToString() : string.Empty;

                    return text.Contains(search, StringComparison.OrdinalIgnoreCase);
                }
                case SerializedPropertyType.ManagedReference:
                {
                    string text = serializedProperty.managedReferenceId.ToString();

                    return text.Contains(search, StringComparison.OrdinalIgnoreCase);
                }
                case SerializedPropertyType.Enum:
                {
                    string[] names = serializedProperty.enumDisplayNames;
                    int index = serializedProperty.enumValueIndex;

                    return names[index].Contains(search, StringComparison.OrdinalIgnoreCase);
                }
                case SerializedPropertyType.Generic:
                {
                    if (!serializedProperty.isArray)
                    {
                        if (serializedProperty.type == nameof(GlobalId))
                        {
                            string id = GlobalIdEditorUtility.GetGuidFromProperty(serializedProperty);

                            return id.Contains(search, StringComparison.OrdinalIgnoreCase);
                        }
                    }

                    return false;
                }
                default:
                {
                    return false;
                }
            }
        }
    }
}
