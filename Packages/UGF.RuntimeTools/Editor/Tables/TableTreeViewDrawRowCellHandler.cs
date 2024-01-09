using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public delegate void TableTreeViewDrawRowCellHandler(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column);
}
