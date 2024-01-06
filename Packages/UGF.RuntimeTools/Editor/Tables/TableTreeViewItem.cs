using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeViewItem : TreeViewItem
    {
        public int Index { get; }
        public SerializedProperty SerializedProperty { get; }

        public override int id
        {
            get
            {
                SerializedProperty propertyId = SerializedProperty.FindPropertyRelative("m_id");
                SerializedProperty propertyIdFirst = propertyId.FindPropertyRelative("m_first");

                return propertyIdFirst.intValue;
            }
            set { }
        }

        public override string displayName
        {
            get
            {
                SerializedProperty propertyName = SerializedProperty.FindPropertyRelative("m_name");

                return propertyName.stringValue;
            }
            set { }
        }

        public TableTreeViewItem(int index, SerializedProperty serializedProperty)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            Index = index;
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
        }
    }
}
