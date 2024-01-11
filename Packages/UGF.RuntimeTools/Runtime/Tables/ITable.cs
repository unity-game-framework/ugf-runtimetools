using System.Collections.Generic;
using UGF.EditorTools.Runtime.Ids;

namespace UGF.RuntimeTools.Runtime.Tables
{
    public interface ITable
    {
        IReadOnlyList<ITableEntry> Entries { get; }

        T GetByName<T>(string name) where T : ITableEntry;
        ITableEntry GetByName(string name);
        bool TryGetByName<T>(string name, out T entry) where T : class, ITableEntry;
        bool TryGetByName(string name, out ITableEntry entry);
        T Get<T>(GlobalId id) where T : class, ITableEntry;
        ITableEntry Get(GlobalId id);
        bool TryGet<T>(GlobalId id, out T entry) where T : class, ITableEntry;
        bool TryGet(GlobalId id, out ITableEntry entry);
    }
}
