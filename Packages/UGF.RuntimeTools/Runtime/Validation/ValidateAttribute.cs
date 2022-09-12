using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ValidateAttribute : Attribute
    {
        public Type TargetType { get; }
        public bool ValidateMembers { get; set; } = true;

        public string Label
        {
            get { return HasLabel ? m_label : throw new ArgumentException("Value not specified."); }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Value cannot be null or empty.", nameof(value));

                m_label = value;
            }
        }

        public bool HasLabel { get { return !string.IsNullOrEmpty(m_label); } }

        private string m_label;

        public ValidateAttribute() : this(typeof(object))
        {
        }

        public ValidateAttribute(Type targetType)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        public ValidateResult Validate(object value, IContext context)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (context == null) throw new ArgumentNullException(nameof(context));

            return TargetType.IsInstanceOfType(value) ? OnValidate(value, context) : ValidateResult.CreateInvalid($"Value must be of the specified type: '{TargetType}'.");
        }

        protected virtual ValidateResult OnValidate(object value, IContext context)
        {
            return ValidateResult.Valid;
        }
    }
}
