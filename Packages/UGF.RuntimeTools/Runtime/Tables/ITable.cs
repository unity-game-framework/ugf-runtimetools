using System.Collections.Generic;
using UGF.EditorTools.Runtime.Ids;

namespace UGF.RuntimeTools.Runtime.Tables
{
    public interface ITable
    {
        IEnumerable<ITableEntry> Entries { get; }

        public T Get<T>(GlobalId id) where T : class, ITableEntry;
        public ITableEntry Get(GlobalId id);
        public bool TryGet<T>(GlobalId id, out T entry) where T : class, ITableEntry;
        public bool TryGet(GlobalId id, out ITableEntry entry);
    }
}
