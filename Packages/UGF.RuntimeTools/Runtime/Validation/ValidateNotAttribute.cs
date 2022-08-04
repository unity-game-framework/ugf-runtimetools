using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateNotAttribute : ValidateAttribute
    {
        public object Value { get; }

        public ValidateNotAttribute(object value) : this(value, typeof(object))
        {
        }

        public ValidateNotAttribute(object value, Type targetType) : base(targetType)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            return !Value.Equals(value) ? ValidateResult.Valid : ValidateResult.CreateInvalid($"Value must not be equal to '{Value}'.");
        }
    }
}
