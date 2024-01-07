using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableTree : ITableTree
    {
        public IReadOnlyList<ITableTreeColumn> Columns { get; }

        protected TableTree(IReadOnlyList<ITableTreeColumn> columns)
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
