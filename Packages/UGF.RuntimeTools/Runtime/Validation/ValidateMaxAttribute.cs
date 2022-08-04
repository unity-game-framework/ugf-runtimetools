using System;
using System.Collections;
using UGF.RuntimeTools.Runtime.Collections;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateMaxAttribute : ValidateAttribute
    {
        public IComparable Max { get; }

        public ValidateMaxAttribute(IComparable max) : this(max, typeof(object))
        {
        }

        public ValidateMaxAttribute(IComparable max, Type targetType) : base(targetType)
        {
            Max = max ?? throw new ArgumentNullException(nameof(max));
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            if (value is IEnumerable enumerable)
            {
                value = CollectionsUtility.GetCount(enumerable);
            }

            if (value is not IComparable)
            {
                return ValidateResult.CreateInvalid("Value must by comparable.");
            }

            Type type = value.GetType();
            var max = (IComparable)Convert.ChangeType(Max, type);

            return max.CompareTo(value) >= 0
                ? ValidateResult.Valid
                : ValidateResult.CreateInvalid($"Value must be less or equal to '{Max}'.");
        }
    }
}
