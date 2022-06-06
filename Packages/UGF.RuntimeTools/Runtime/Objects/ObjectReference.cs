using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Objects
{
    public readonly struct ObjectReference<TValue> where TValue : class
    {
        public TValue Value { get { return m_value ?? throw new ArgumentException("Value not specified."); } }
        public bool HasValue { get { return m_value != null; } }

        public static ObjectReference<TValue> Empty { get; } = default;

        private readonly TValue m_value;

        public ObjectReference(TValue value)
        {
            m_value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool Equals(ObjectReference<TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(m_value, other.m_value);
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectReference<TValue> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(m_value);
        }

        public static bool operator ==(ObjectReference<TValue> left, ObjectReference<TValue> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ObjectReference<TValue> left, ObjectReference<TValue> right)
        {
            return !left.Equals(right);
        }

        public static implicit operator ObjectReference<TValue>(TValue value)
        {
            return new ObjectReference<TValue>(value);
        }

        public static implicit operator bool(ObjectReference<TValue> reference)
        {
            return reference.HasValue;
        }

        public static implicit operator TValue(ObjectReference<TValue> reference)
        {
            return reference.Value;
        }
    }
}
