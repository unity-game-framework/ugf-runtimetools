using System;
using System.Reflection;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public readonly struct ValidateMemberResult
    {
        public MemberInfo Member { get; }
        public Type AttributeType { get; }
        public ValidateResult Result { get; }

        public ValidateMemberResult(MemberInfo member, Type attributeType, ValidateResult result)
        {
            if (!result.IsValid()) throw new ArgumentException("Value should be valid.", nameof(result));

            Member = member ?? throw new ArgumentNullException(nameof(member));
            AttributeType = attributeType ?? throw new ArgumentNullException(nameof(attributeType));
            Result = result;
        }

        public bool IsValid()
        {
            return Member != null && AttributeType != null && Result.IsValid();
        }

        public string GetMessage()
        {
            if (!IsValid()) throw new ArgumentException("Value should be valid.");

            return $"{Member.Name}: {Result.Message}";
        }
    }
}
