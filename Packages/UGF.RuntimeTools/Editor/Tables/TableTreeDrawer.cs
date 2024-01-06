using System;
using System.Collections.Generic;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Scopes;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeDrawer : DrawerBase
    {
        public TableAsset Asset { get; }
        public string PropertyIdName { get; set; } = "m_id";
        public bool UnlockIds { get; set; }

        private readonly GUILayoutOption[] m_layoutOptions =
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true)
        };

        private readonly TableTreeDrawerState m_state;
        private TableTreeView m_treeView;
        private SerializedObject m_serializedObject;

        public TableTreeDrawer(TableAsset asset) : this(asset, TableTreeEditorUtility.GetEntryColumns(asset))
        {
        }

        public TableTreeDrawer(TableAsset asset, IReadOnlyList<TableTreeDrawerColumn> columns)
        {
            Asset = asset ? asset : throw new ArgumentNullException(nameof(asset));

            m_state = new TableTreeDrawerState(asset, columns);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_serializedObject = new SerializedObject(Asset);
            m_treeView = new TableTreeView(m_serializedObject, (TableTreeViewState)m_state.State);
            m_treeView.Reload();
            m_treeView.multiColumnHeader.ResizeToFit();
            m_treeView.RowDraw += OnRowDrawGUI;
            m_treeView.RowCellDraw += OnRowCellDrawGUI;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            m_treeView.RowDraw -= OnRowDrawGUI;
            m_treeView.RowCellDraw -= OnRowCellDrawGUI;
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

        protected virtual void OnDrawEntryGUI(Rect position, SerializedProperty serializedProperty, int rowIndex)
        {
        }

        protected virtual void OnDrawEntryCellGUI(Rect position, SerializedProperty serializedProperty, int rowIndex, int columnIndex)
        {
            EditorGUI.PropertyField(position, serializedProperty, GUIContent.none, false);
        }

        private void OnRowDrawGUI(Rect position, int rowIndex, TableTreeViewItem rowItem)
        {
            OnDrawEntryGUI(position, rowItem.SerializedProperty, rowIndex);
        }

        private void OnRowCellDrawGUI(Rect position, int rowIndex, TableTreeViewItem rowItem, int columnIndex, TableTreeViewColumnState columnState)
        {
            SerializedProperty propertyValue = rowItem.SerializedProperty.FindPropertyRelative(columnState.PropertyName);

            using (new EditorGUI.DisabledScope(!UnlockIds && propertyValue.name == PropertyIdName))
            {
                OnDrawEntryCellGUI(position, propertyValue, rowIndex, columnIndex);
            }
        }
    }
}
