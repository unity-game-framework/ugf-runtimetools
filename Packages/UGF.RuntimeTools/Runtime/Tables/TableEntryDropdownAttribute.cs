using System;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tables
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TableEntryDropdownAttribute : PropertyAttribute
    {
        public Type TableType { get; }

        public TableEntryDropdownAttribute(Type tableType = null)
        {
            TableType = tableType ?? typeof(TableAsset);
        }
    }
}
