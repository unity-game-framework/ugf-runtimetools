﻿using System;
using UnityEditor;

namespace UGF.RuntimeTools.Editor.Tables
{
    public readonly struct TableTreeDrawerColumn
    {
        public string PropertyName { get; }
        public string DisplayName { get; }

        public TableTreeDrawerColumn(string propertyName) : this(propertyName, ObjectNames.NicifyVariableName(propertyName))
        {
        }

        public TableTreeDrawerColumn(string propertyName, string displayName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException("Value cannot be null or empty.", nameof(propertyName));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException("Value cannot be null or empty.", nameof(displayName));

            PropertyName = propertyName;
            DisplayName = displayName;
        }
    }
}
