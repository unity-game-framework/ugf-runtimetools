using System;

namespace UGF.RuntimeTools.Runtime.Collections
{
    public class CollectionValueNotFoundByArgumentException : Exception
    {
        public CollectionValueNotFoundByArgumentException(object argument) : base($"Value not found in collection by the specified argument: '{argument}'.")
        {
        }
    }
}
