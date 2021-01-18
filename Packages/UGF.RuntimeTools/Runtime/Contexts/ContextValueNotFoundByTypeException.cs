using System;

namespace UGF.RuntimeTools.Runtime.Contexts
{
    public class ContextValueNotFoundByTypeException : Exception
    {
        public ContextValueNotFoundByTypeException(Type type) : base($"Value not found in context by the specified type: '{type}'.")
        {
        }
    }
}
