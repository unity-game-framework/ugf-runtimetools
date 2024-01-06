using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeWindow : EditorWindow
    {
        [SerializeField] private string m_assetId;

        public TableTreeDrawer Drawer { get; } = new TableTreeDrawer();

        private void OnEnable()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TableAsset>(AssetDatabase.GUIDToAssetPath(m_assetId));

            if (asset != null)
            {
                Drawer.SetTarget(asset);
            }

            if (Drawer.HasSerializedObject)
            {
                Drawer.Enable();
            }
        }

        private void OnDisable()
        {
            if (Drawer.HasSerializedObject)
            {
                Drawer.Disable();
                Drawer.ClearTarget();
            }
        }

        private void OnGUI()
        {
            Drawer.DrawGUILayout();
        }

        public void SetTarget(TableAsset asset)
        {
            SetTarget(asset, TableTreeEditorUtility.GetEntryColumns(asset));
        }

        public void SetTarget(TableAsset asset, IReadOnlyList<TableTreeDrawerColumn> columns)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (columns == null) throw new ArgumentNullException(nameof(columns));

            m_assetId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));

            if (Drawer.HasSerializedObject)
            {
                Drawer.Disable();
            }

            Drawer.SetTarget(asset, columns);
            Drawer.Enable();
        }

        public void ClearTarget()
        {
            m_assetId = string.Empty;

            Drawer.Disable();
        }
    }
}
