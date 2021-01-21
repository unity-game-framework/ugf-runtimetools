using System;

namespace UGF.RuntimeTools.Runtime.Providers
{
    public static class ProviderInstance<TProvider> where TProvider : class, IProvider
    {
        public static TProvider Instance { get { return m_instance ?? throw new InvalidOperationException("No value specified."); } set { m_instance = value ?? throw new ArgumentNullException(nameof(value)); } }
        public static bool HasInstance { get { return m_instance != null; } }

        private static TProvider m_instance;

        public static bool Clear()
        {
            if (m_instance != null)
            {
                m_instance = null;
                return true;
            }

            return false;
        }
    }
}
