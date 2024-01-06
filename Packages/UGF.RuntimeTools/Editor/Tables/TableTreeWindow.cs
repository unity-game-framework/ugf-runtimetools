using System;
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
        }

        private void OnDisable()
        {
            ClearTarget();

            m_styles = null;
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
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Empty);
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
