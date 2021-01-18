using System;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public class ProviderEntryNotFoundByIdException : Exception
    {
        public ProviderEntryNotFoundByIdException(object id) : base($"Entry not found in provider by the specified id: '{id}'.")
        {
        }
    }
}
