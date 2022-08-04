using System;
using System.Reflection;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public readonly struct ValidateReport
    {
        public MemberInfo Member { get; }
        public Type AttributeType { get; }
        public ValidateResult Result { get; }

        public ValidateReport(MemberInfo member, Type attributeType, ValidateResult result)
        {
            if (!result.IsValid()) throw new ArgumentException("Value should be valid.", nameof(result));

            Member = member ?? throw new ArgumentNullException(nameof(member));
            AttributeType = attributeType ?? throw new ArgumentNullException(nameof(attributeType));
            Result = result;
        }
    }
}
