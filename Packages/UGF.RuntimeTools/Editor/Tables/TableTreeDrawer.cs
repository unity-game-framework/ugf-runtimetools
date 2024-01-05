using System;
using UGF.EditorTools.Editor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal class TableTreeDrawer : DrawerBase
    {
        private TableTreeView m_treeView;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_treeView = null;
        }

        public void SetSerializedProperty(SerializedProperty serializedProperty)
        {
            m_treeView = new TableTreeView(serializedProperty);
        }

        public void ClearSerializedProperty()
        {
            m_treeView = null;
        }

        public void DrawGUI(Rect position)
        {
            m_treeView?.OnGUI(position);
        }
    }
}
