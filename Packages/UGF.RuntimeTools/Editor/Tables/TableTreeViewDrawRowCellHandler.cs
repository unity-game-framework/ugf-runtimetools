using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal delegate void TableTreeViewDrawRowCellHandler(Rect position, TableTreeViewItem item, SerializedProperty serializedProperty, TableTreeColumnOptions column);
}
