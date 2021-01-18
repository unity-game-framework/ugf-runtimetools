namespace UGF.RuntimeTools.Runtime.Providers
{
    public delegate void ProviderEntryHandler<in TId, in TEntry>(IProvider provider, TId id, TEntry entry);
}
