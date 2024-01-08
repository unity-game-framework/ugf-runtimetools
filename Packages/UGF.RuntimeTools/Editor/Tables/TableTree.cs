using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableTree<TColumn> : ITableTree where TColumn : class, ITableTreeColumn
    {
        public IReadOnlyList<TColumn> Columns { get; }

        IReadOnlyList<ITableTreeColumn> ITableTree.Columns { get { return Columns; } }

        protected TableTree(IReadOnlyList<TColumn> columns)
        {
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
        }

        public void GetItems(ICollection<ITableTreeItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            OnGetItems(items);
        }

        protected abstract void OnGetItems(ICollection<ITableTreeItem> items);
    }
}
