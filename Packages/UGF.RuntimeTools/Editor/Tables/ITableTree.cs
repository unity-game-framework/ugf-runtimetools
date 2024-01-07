using System.Collections.Generic;

namespace UGF.RuntimeTools.Editor.Tables
{
    public interface ITableTree
    {
        IReadOnlyList<ITableTreeColumn> Columns { get; }

        void GetItems(ICollection<ITableTreeItem> items);
    }
}
