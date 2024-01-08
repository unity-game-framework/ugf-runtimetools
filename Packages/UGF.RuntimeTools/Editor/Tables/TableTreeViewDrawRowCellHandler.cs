using UnityEditor;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal delegate void TableTreeViewDrawRowCellHandler(Rect position, SerializedProperty serializedProperty, TableTreeColumnOptions column);
}
