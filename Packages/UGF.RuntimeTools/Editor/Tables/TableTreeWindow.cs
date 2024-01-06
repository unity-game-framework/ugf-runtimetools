using System;
using System.Linq;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeWindow : EditorWindow
    {
        [SerializeField] private TableTreeDrawerState m_state;

        private TableTreeDrawer m_drawer;
        private Styles m_styles;

        private class Styles
        {
            public GUIStyle Toolbar { get; } = EditorStyles.toolbar;
            public GUIStyle Statusbar { get; } = new GUIStyle("IN Footer");
        }

        private void OnEnable()
        {
            if (m_state != null && !string.IsNullOrEmpty(m_state.AssetGuid))
            {
                var asset = AssetDatabase.LoadAssetAtPath<TableAsset>(AssetDatabase.GUIDToAssetPath(m_state.AssetGuid));

                if (asset != null)
                {
                    SetTarget(asset);
                }
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
                    var asset = (TableAsset)m_drawer.Asset;
                    int count = asset.Get().Entries.Count();

                    GUILayout.FlexibleSpace();
                    GUILayout.Label($"Count: {count}");
                }
            }
        }

        public void SetTarget(TableAsset tableAsset)
        {
            if (tableAsset == null) throw new ArgumentNullException(nameof(tableAsset));

            m_drawer?.Disable();

            m_state = new TableTreeDrawerState(tableAsset);
            m_drawer = new TableTreeDrawer(tableAsset, m_state);
            m_drawer.Enable();
        }

        public void ClearTarget()
        {
            m_state = new TableTreeDrawerState();
            m_drawer?.Disable();
            m_drawer = null;
        }
    }
}
