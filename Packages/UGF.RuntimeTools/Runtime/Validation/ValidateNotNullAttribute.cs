using System;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateNotNullAttribute : ValidateAttribute
    {
        public ValidateNotNullAttribute() : this(typeof(object))
        {
        }

        public ValidateNotNullAttribute(Type targetType) : base(targetType)
        {
            ValidateMembers = false;
        }
    }
}
