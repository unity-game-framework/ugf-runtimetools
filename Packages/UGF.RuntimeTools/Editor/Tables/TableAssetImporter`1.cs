﻿using UGF.RuntimeTools.Runtime.Tables;
using UnityEngine;

namespace UGF.RuntimeTools.Editor.Tables
{
    public abstract class TableAssetImporter<TTable> : TableAssetImporter where TTable : TableAsset
    {
        [SerializeField] private TTable m_table;

        public TTable Table { get { return m_table; } set { m_table = value; } }

        protected override bool OnIsValid()
        {
            return m_table != null;
        }
    }
}
