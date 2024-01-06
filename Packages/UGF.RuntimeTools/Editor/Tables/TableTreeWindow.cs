using System;
using System.Collections.Generic;
using System.Linq;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeWindow : EditorWindow
    {
        [SerializeField] private string m_assetId;

        private TableTreeDrawer m_drawer;
        private Styles m_styles;

        private class Styles
        {
            public GUIStyle Toolbar { get; } = EditorStyles.toolbar;
            public GUIStyle Statusbar { get; } = new GUIStyle("IN Footer");
        }

        private void OnEnable()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TableAsset>(AssetDatabase.GUIDToAssetPath(m_assetId));

            if (asset != null)
            {
                SetTarget(asset);
            }

            m_drawer?.Enable();
        }

        private void OnDisable()
        {
            m_drawer?.Disable();
        }

        private void OnGUI()
        {
            m_styles ??= new Styles();

            using (new EditorGUILayout.HorizontalScope(m_styles.Toolbar))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Empty);
            }

            if (m_drawer != null)
            {
                m_drawer.DrawGUILayout();
            }
            else
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
                {
                    EditorGUILayout.HelpBox("No table data to display.", MessageType.Info);
                }
            }

            using (new EditorGUILayout.HorizontalScope(m_styles.Statusbar))
            {
                string path = m_drawer != null
                    ? AssetDatabase.GetAssetPath(m_drawer.Asset)
                    : "None";

                GUILayout.Label($"Path: {path}");

                if (m_drawer != null)
                {
                    int count = m_drawer.Asset.Get().Entries.Count();

                    GUILayout.FlexibleSpace();
                    GUILayout.Label($"Count: {count}");
                }
            }
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
            m_drawer?.Disable();
            m_drawer = new TableTreeDrawer(asset, columns);
            m_drawer.Enable();
        }

        public void ClearTarget()
        {
            m_assetId = string.Empty;
            m_drawer?.Disable();
            m_drawer = null;
        }
    }
}
