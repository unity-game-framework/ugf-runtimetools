using System;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [Serializable]
    public class TableTreeDrawerState
    {
        [SerializeField] private TableTreeViewState m_state;

        public TreeViewState State { get { return m_state; } }

        public TableTreeDrawerState()
        {
            m_state = new TableTreeViewState();
        }

        public TableTreeDrawerState(TableAsset tableAsset)
        {
            m_state = TableTreeEditorInternalUtility.CreateState(tableAsset);
        }
    }
}
