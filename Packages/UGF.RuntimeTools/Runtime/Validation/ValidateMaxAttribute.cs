using System;
using System.Collections;
using UGF.RuntimeTools.Runtime.Collections;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateMaxAttribute : ValidateAttribute
    {
        public object Max { get; }

        public ValidateMaxAttribute(object max) : this(max, typeof(object))
        {
        }

        public ValidateMaxAttribute(object max, Type targetType) : base(targetType)
        {
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

            var max = (IComparable)Convert.ChangeType(Max, type);

            return max.CompareTo(value) >= 0
                ? ValidateResult.Valid
                : ValidateResult.CreateInvalid($"Value must be less or equal to '{Max}'.");
        }
    }
}
