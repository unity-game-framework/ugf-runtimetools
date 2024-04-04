using System;
using UGF.RuntimeTools.Runtime.Tables;

namespace UGF.RuntimeTools.Editor.Tables
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableTreeWindowAttribute : Attribute
    {
        public Type AssetType { get; }
        public int Priority { get; set; }

        public TableTreeWindowAttribute(Type assetType = null)
        {
            AssetType = assetType ?? typeof(TableAsset);
        }
    }
}
