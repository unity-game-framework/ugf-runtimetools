using System;
using UGF.EditorTools.Editor.IMGUI;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeDrawer : DrawerBase, IDisposable
    {
        public SerializedObject SerializedObject { get; }

        private readonly TableTreeView m_treeView;

        private readonly GUILayoutOption[] m_layoutOptions =
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true)
        };

        public TableTreeDrawer(Object tableAsset, TableTreeDrawerState state) : this(new SerializedObject(tableAsset), state)
        {
        }

        public TableTreeDrawer(SerializedObject serializedObject, TableTreeDrawerState state)
        {
            SerializedObject = serializedObject ?? throw new ArgumentNullException(nameof(serializedObject));

            SerializedProperty propertyTable = serializedObject.FindProperty("m_table");

            m_treeView = new TableTreeView(propertyTable, (TableTreeViewState)state.State);
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

        public void Dispose()
        {
            SerializedObject.Dispose();
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
