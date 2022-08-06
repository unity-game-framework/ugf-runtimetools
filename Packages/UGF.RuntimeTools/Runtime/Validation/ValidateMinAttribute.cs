using System;
using System.Collections;
using UGF.RuntimeTools.Runtime.Collections;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateMinAttribute : ValidateAttribute
    {
        public object Min { get; }

        public ValidateMinAttribute(object min) : this(min, typeof(object))
        {
        }

        public ValidateMinAttribute(object min, Type targetType) : base(targetType)
        {
            Min = min ?? throw new ArgumentNullException(nameof(min));

            ValidateMembers = false;
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

            return min.CompareTo(value) <= 0
                ? ValidateResult.Valid
                : ValidateResult.CreateInvalid($"Value must be greater or equal to '{Min}'.");
        }
    }
}
