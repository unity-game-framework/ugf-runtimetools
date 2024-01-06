using System;
using UGF.EditorTools.Editor.IMGUI;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeDrawer : DrawerBase
    {
        public SerializedProperty SerializedProperty { get; }

        private readonly TableTreeView m_treeView;

        private readonly GUILayoutOption[] m_layoutOptions =
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true)
        };

        public TableTreeDrawer(SerializedProperty serializedProperty, TableTreeViewState state)
        {
            SerializedProperty = serializedProperty ?? throw new ArgumentNullException(nameof(serializedProperty));

            m_treeView = new TableTreeView(serializedProperty, state);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_treeView.Reload();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public void DrawGUILayout()
        {
            Rect position;

            using (var scope = new EditorGUILayout.VerticalScope(GUIStyle.none, m_layoutOptions))
            {
                position = scope.rect;
            }

            DrawGUI(position);
        }

        public void DrawGUI(Rect position)
        {
            m_treeView.OnGUI(position);
        }
    }
}
