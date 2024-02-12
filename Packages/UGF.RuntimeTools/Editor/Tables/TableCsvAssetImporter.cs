using UGF.RuntimeTools.Runtime.Tables;
using UnityEditor.AssetImporters;

namespace UGF.RuntimeTools.Editor.Tables
{
    [ScriptedImporter(0, new[] { "table" }, new[] { "csv" })]
    public class TableCsvAssetImporter : TableCsvAssetImporter<TableAsset>
    {
    }
}
