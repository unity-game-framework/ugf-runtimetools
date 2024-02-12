using System.Data;
using System.IO;
using UGF.Csv.Runtime;
using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableCsvAssetImporter<TAsset> : TableAssetImporter<TAsset> where TAsset : TableAsset
    {
        [SerializeField] private string m_tablePropertyName = "m_table";

        public string TablePropertyName { get { return m_tablePropertyName; } set { m_tablePropertyName = value; } }

        protected override void OnImport()
        {
            string csv = File.ReadAllText(assetPath);

            if (!string.IsNullOrEmpty(csv))
            {
                DataTable data = CsvUtility.FromCsv(csv);

                TableTreeEditorUtility.SetData(Table, m_tablePropertyName, data);
            }
        }

        protected override void OnExport()
        {
            DataTable data = TableTreeEditorUtility.GetData(Table, m_tablePropertyName);
            string csv = CsvUtility.ToCsv(data);

            File.WriteAllText(assetPath, csv);
        }
    }
}
