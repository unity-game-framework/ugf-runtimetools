using UGF.EditorTools.Runtime.Ids;

namespace UGF.RuntimeTools.Runtime.Tables
{
    public interface ITableEntry
    {
        GlobalId Id { get; }
        string Name { get; }
    }
}
