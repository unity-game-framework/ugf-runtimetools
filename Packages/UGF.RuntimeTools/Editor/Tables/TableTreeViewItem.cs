using System;
using UGF.EditorTools.Editor.Ids;
using UGF.EditorTools.Runtime.Ids;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeViewItem : TreeViewItem
    {
        public SerializedProperty SerializedProperty { get; }

        public override int id { get { return m_id; } set { } }

        private readonly int m_id;

        public TableTreeViewItem(SerializedProperty serializedProperty)
        {
            m_id = (int)GlobalId.Generate().First;

            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));
        }

        public GlobalId GetId()
        {
            SerializedProperty propertyId = SerializedProperty.FindPropertyRelative("m_id");

            return GlobalIdEditorUtility.GetGlobalIdFromProperty(propertyId);
        }
    }
}
