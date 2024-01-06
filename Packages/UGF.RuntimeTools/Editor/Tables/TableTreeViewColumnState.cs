using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    [Serializable]
    internal class TableTreeViewColumnState : MultiColumnHeaderState.Column
    {
        [SerializeField] private string m_propertyName;

        public string PropertyName { get { return m_propertyName; } set { m_propertyName = value; } }
        public IComparer<SerializedProperty> PropertyComparer { get; set; }
        public ITableTreeDrawerColumnSearchHandler SearchHandler { get; set; }
    }
}
