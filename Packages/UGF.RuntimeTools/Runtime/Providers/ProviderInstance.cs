using System;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public static class ProviderInstance
    {
        public static IProvider<Type, IProvider> Providers { get { return m_providers; } set { m_providers = value ?? throw new ArgumentNullException(nameof(value)); } }

        private static IProvider<Type, IProvider> m_providers = new Provider<Type, IProvider>();

        public static void Set<T>(T provider) where T : IProvider
        {
            Set(typeof(T), provider);
        }

        public static void Set(Type type, IProvider provider)
        {
            m_providers.Remove(type);
            m_providers.Add(type, provider);
        }

        public static T Get<T>() where T : IProvider
        {
            return m_providers.Get<T>(typeof(T));
        }

        public static IProvider Get(Type type)
        {
            return m_providers.Get(type);
        }

        public static bool TryGet<T>(out T provider) where T : IProvider
        {
            return m_providers.TryGet(typeof(T), out provider);
        }

        public static bool TryGet(Type type, out IProvider provider)
        {
            return m_providers.TryGet(type, out provider);
        }
    }
}
