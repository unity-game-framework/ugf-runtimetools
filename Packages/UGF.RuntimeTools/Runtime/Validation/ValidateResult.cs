using System;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public readonly struct ValidateResult : IComparable<ValidateResult>
    {
        public bool Value { get; }
        public string Message { get; }

        public static ValidateResult Valid { get; } = CreateValid();
        public static ValidateResult Invalid { get; } = CreateInvalid();

        public ValidateResult(bool value, string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Value cannot be null or empty.", nameof(message));

            Value = value;
            Message = message;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Message);
        }

        public bool Equals(ValidateResult other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ValidateResult other && Equals(other);
        }

        public int CompareTo(ValidateResult other)
        {
            return Value.CompareTo(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static ValidateResult CreateValid(string message = "Value is valid.")
        {
            return new ValidateResult(true, message);
        }

        public static ValidateResult CreateInvalid(string message = "Value is invalid.")
        {
            return new ValidateResult(false, message);
        }

        public static implicit operator bool(ValidateResult result)
        {
            return result.Value;
        }
    }
}
