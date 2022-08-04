using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ValidateAttribute : Attribute
    {
        public Type TargetType { get; }

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
