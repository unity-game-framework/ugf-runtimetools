using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeOptions
    {
        public IReadOnlyList<TableTreeColumnOptions> Columns { get; }
        public string PropertyEntriesName { get; set; } = "m_entries";
        public string PropertyIdName { get; set; } = "m_id";
        public string PropertyNameName { get; set; } = "m_name";
        public string PropertyChildrenName { get; set; } = "m_children";

        public TableTreeOptions(IReadOnlyList<TableTreeColumnOptions> columns)
        {
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
        }
    }
}
