using System;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public class CollectionValueNotFoundByKeyException : Exception
    {
        public CollectionValueNotFoundByKeyException(object key) : base($"Value not found in collection by the specified key: '{key}'.")
        {
        }
    }
}
