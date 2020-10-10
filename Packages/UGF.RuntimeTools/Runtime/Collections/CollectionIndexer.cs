namespace UGF.RuntimeTools.Runtime.Collections
{
    public delegate TValue CollectionIndexer<out TValue, in TCollection>(TCollection collection, int index);
}
