using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateMinAttribute : ValidateAttribute
    {
        public object Min { get; }

        public ValidateMinAttribute(object min)
        {
            Min = min ?? throw new ArgumentNullException(nameof(min));
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            Type type = value.GetType();
            var min = Convert.ChangeType(Min, type) as IComparable;

            return min?.CompareTo(value) <= 0 ? ValidateResult.Valid : ValidateResult.CreateInvalid($"Value must be greater or equal to '{Min}'.");
        }
    }
}
