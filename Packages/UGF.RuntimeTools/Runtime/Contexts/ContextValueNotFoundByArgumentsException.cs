using System;

namespace UGF.RuntimeTools.Runtime.Contexts
{
    public class ContextValueNotFoundByArgumentsException : Exception
    {
        public ContextValueNotFoundByArgumentsException(object arguments) : base($"Value not found in context by the specified arguments: '{arguments}'.")
        {
        }
    }
}
