using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateRangeAttribute : ValidateAttribute
    {
        public object Min { get; }
        public object Max { get; }

        public ValidateRangeAttribute(object min, object max) : this(min, max, typeof(object))
        {
        }

        public ValidateRangeAttribute(object min, object max, Type targetType) : base(targetType)
        {
            Min = min ?? throw new ArgumentNullException(nameof(min));
            Max = max ?? throw new ArgumentNullException(nameof(max));
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            Type type = value.GetType();
            var min = Convert.ChangeType(Min, type) as IComparable;
            var max = Convert.ChangeType(Max, type) as IComparable;

            if (min?.CompareTo(value) <= 0 && max?.CompareTo(value) >= 0)
            {
                return ValidateResult.Valid;
            }

            return ValidateResult.CreateInvalid($"Value must be in range of '{Min}' and '{Max}'.");
        }
    }
}
