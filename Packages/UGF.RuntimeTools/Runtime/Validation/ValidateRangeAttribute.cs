﻿using System;
using System.Collections;
using UGF.RuntimeTools.Runtime.Collections;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateRangeAttribute : ValidateAttribute
    {
        public IComparable Min { get; }
        public IComparable Max { get; }

        public ValidateRangeAttribute(IComparable min, IComparable max) : this(min, max, typeof(object))
        {
        }

        public ValidateRangeAttribute(IComparable min, IComparable max, Type targetType) : base(targetType)
        {
            Min = min ?? throw new ArgumentNullException(nameof(min));
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
            var min = (IComparable)Convert.ChangeType(Min, type);
            var max = (IComparable)Convert.ChangeType(Max, type);

            return min.CompareTo(value) <= 0 && max.CompareTo(value) >= 0
                ? ValidateResult.Valid
                : ValidateResult.CreateInvalid($"Value must be in range of '{Min}' and '{Max}'.");
        }
    }
}
