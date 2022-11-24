using System;

namespace UGF.RuntimeTools.Runtime.Options
{
    public readonly struct Optional<TValue>
    {
        public TValue Value { get { return HasValue ? m_value : throw new ArgumentException("Value not specified."); } }
        public bool HasValue { get; }

        public static Optional<TValue> Empty { get; } = new Optional<TValue>();

        private readonly TValue m_value;

        public Optional(TValue value)
        {
            m_value = value;

            HasValue = true;
        }

        public static implicit operator TValue(Optional<TValue> result)
        {
            return result.Value;
        }

        public static implicit operator Optional<TValue>(TValue value)
        {
            return new Optional<TValue>(value);
        }

        public static implicit operator bool(Optional<TValue> result)
        {
            return result.HasValue;
        }
    }
}
