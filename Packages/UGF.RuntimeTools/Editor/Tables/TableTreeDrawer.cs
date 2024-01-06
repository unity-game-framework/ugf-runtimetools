using System;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeDrawer : DrawerBase
    {
        public Object Asset { get; }
        public TableTreeDrawerState State { get; }

        private readonly GUILayoutOption[] m_layoutOptions =
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true)
        };

        private TableTreeView m_treeView;
        private SerializedObject m_serializedObject;

        public TableTreeDrawer(Object asset, TableTreeDrawerState state)
        {
            Asset = asset ? asset : throw new ArgumentNullException(nameof(asset));
            State = state ?? throw new ArgumentNullException(nameof(state));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_serializedObject = new SerializedObject(Asset);
            m_treeView = new TableTreeView(m_serializedObject, (TableTreeViewState)State.State);
            m_treeView.Reload();
            m_treeView.multiColumnHeader.ResizeToFit();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_treeView = null;
            m_serializedObject.Dispose();
            m_serializedObject = null;
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
            if (m_treeView != null)
            {
                using (new SerializedObjectUpdateScope(m_serializedObject))
                {
                    m_treeView.OnGUI(position);
                }
            }
        }
    }
}
