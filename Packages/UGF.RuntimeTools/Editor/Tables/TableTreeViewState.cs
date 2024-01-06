using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [Serializable]
    public class TableTreeViewState : TreeViewState
    {
        [SerializeField] private MultiColumnHeaderState m_header;

        public MultiColumnHeaderState Header { get { return m_header; } set { m_header = value; } }
    }
}
