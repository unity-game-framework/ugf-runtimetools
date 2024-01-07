using System;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public class TableTreeColumnProperty : TableTreeColumn
    {
        public string PropertyPath { get; }

        public TableTreeColumnProperty(GUIContent displayName, string propertyPath) : base(displayName)
        {
            if (string.IsNullOrEmpty(propertyPath)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyPath));

            PropertyPath = propertyPath;
        }
    }
}
