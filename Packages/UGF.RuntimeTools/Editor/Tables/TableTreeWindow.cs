using System;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeWindow : EditorWindow
    {
        [SerializeField] private string m_assetId;

        private SerializedObject m_serializedObject;
        private TableTreeDrawer m_drawer;

        private void OnEnable()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TableAsset>(AssetDatabase.GUIDToAssetPath(m_assetId));

            if (asset != null)
            {
                SetTarget(asset);
            }
        }

        private void OnDisable()
        {
            m_drawer?.Disable();
            m_drawer = null;
            m_serializedObject?.Dispose();
            m_serializedObject = null;
        }

        private void OnGUI()
        {
            if (m_serializedObject?.targetObject == null)
            {
                m_serializedObject = null;
                m_drawer = null;
            }

            m_drawer?.DrawGUILayout();
        }

        public void SetTarget(TableAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            m_serializedObject = new SerializedObject(asset);
            m_assetId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
            m_drawer?.Disable();
            m_drawer = new TableTreeDrawer(m_serializedObject, TableTreeEditorUtility.CreateOptions(asset.Get().GetType()));
            m_drawer.Enable();
        }

        public void SetTarget(TableAsset asset, TableTreeOptions options)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (options == null) throw new ArgumentNullException(nameof(options));

            m_serializedObject = new SerializedObject(asset);
            m_assetId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
            m_drawer?.Disable();
            m_drawer = new TableTreeDrawer(m_serializedObject, options);
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
