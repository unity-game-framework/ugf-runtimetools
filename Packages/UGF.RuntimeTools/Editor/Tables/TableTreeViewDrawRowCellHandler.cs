using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    internal delegate void TableTreeViewDrawRowCellHandler(Rect position, int rowIndex, TableTreeViewItem rowItem, int columnIndex, TableTreeViewColumnState columnState);
}
