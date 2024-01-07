using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [Serializable]
    internal class TableTreeViewState : TreeViewState
    {
        [SerializeField] private int m_searchColumnIndex;
        [SerializeField] private MultiColumnHeaderState m_header;

        public int SearchColumnIndex { get { return m_searchColumnIndex; } set { m_searchColumnIndex = value; } }
        public MultiColumnHeaderState Header { get { return m_header; } set { m_header = value; } }
    }
}
