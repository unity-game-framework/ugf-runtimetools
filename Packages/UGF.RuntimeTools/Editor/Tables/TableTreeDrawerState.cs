using System;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [Serializable]
    public class TableTreeDrawerState
    {
        [SerializeField] private string m_assetGuid;
        [SerializeField] private TableTreeViewState m_state;

        public string AssetGuid { get { return m_assetGuid; } set { m_assetGuid = value; } }
        public TreeViewState State { get { return m_state; } }

        public TableTreeDrawerState()
        {
            m_assetGuid = string.Empty;
            m_state = new TableTreeViewState();
        }

        public TableTreeDrawerState(TableAsset tableAsset)
        {
            m_assetGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(tableAsset));
            m_state = TableTreeEditorInternalUtility.CreateState(tableAsset);
        }
    }
}
