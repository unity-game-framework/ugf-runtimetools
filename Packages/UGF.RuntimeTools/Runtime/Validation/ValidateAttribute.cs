using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class ValidateAttribute : Attribute
    {
        public ValidateResult Validate(object value, IContext context)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (context == null) throw new ArgumentNullException(nameof(context));

            return OnValidate(value, context);
        }

        protected abstract ValidateResult OnValidate(object value, IContext context);
    }
}
