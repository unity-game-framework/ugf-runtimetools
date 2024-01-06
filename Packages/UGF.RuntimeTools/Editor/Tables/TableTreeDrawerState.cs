using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [Serializable]
    internal class TableTreeDrawerState
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

        public TableTreeDrawerState(TableAsset tableAsset, IReadOnlyList<TableTreeDrawerColumn> columns)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));

            m_assetGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(tableAsset));
            m_state = TableTreeEditorInternalUtility.CreateState(columns);
        }
    }
}
