using System;
using System.Collections;
using UGF.RuntimeTools.Runtime.Collections;
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

            if (value is IEnumerable enumerable)
            {
                type = typeof(int);
                value = CollectionsUtility.GetCount(enumerable);
            }

            if (value is Enum)
            {
                type = type.GetEnumUnderlyingType();
                value = Convert.ChangeType(value, type);
            }

            if (value is not IComparable)
            {
                return ValidateResult.CreateInvalid("Value must by comparable.");
            }

            var min = (IComparable)Convert.ChangeType(Min, type);
            var max = (IComparable)Convert.ChangeType(Max, type);

            return min.CompareTo(value) <= 0 && max.CompareTo(value) >= 0
                ? ValidateResult.Valid
                : ValidateResult.CreateInvalid($"Value must be in range of '{Min}' and '{Max}'.");
        }
    }
}
