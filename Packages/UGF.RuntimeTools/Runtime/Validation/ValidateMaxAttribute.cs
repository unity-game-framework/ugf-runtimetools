using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateMaxAttribute : ValidateAttribute
    {
        public object Max { get; }

        public ValidateMaxAttribute(object max)
        {
            Max = max ?? throw new ArgumentNullException(nameof(max));
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            Type type = value.GetType();
            var max = Convert.ChangeType(Max, type) as IComparable;

            return max?.CompareTo(value) >= 0 ? ValidateResult.Valid : ValidateResult.CreateInvalid($"Value must be less or equal to '{Max}'.");
        }
    }
}
