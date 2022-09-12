using System;
using System.Reflection;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public readonly struct ValidateMemberResult
    {
        public MemberInfo Member { get; }
        public Type AttributeType { get; }
        public ValidateResult Result { get; }
        public string Label { get { return HasLabel ? m_label : throw new ArgumentException("Value not specified."); } }
        public bool HasLabel { get { return !string.IsNullOrEmpty(m_label); } }

        private readonly string m_label;

        public ValidateMemberResult(MemberInfo member, Type attributeType, ValidateResult result)
        {
            if (!result.IsValid()) throw new ArgumentException("Value should be valid.", nameof(result));

            Member = member ?? throw new ArgumentNullException(nameof(member));
            AttributeType = attributeType ?? throw new ArgumentNullException(nameof(attributeType));
            Result = result;

            m_label = string.Empty;
        }

        public ValidateMemberResult(MemberInfo member, Type attributeType, ValidateResult result, string label)
        {
            if (!result.IsValid()) throw new ArgumentException("Value should be valid.", nameof(result));
            if (string.IsNullOrEmpty(label)) throw new ArgumentException("Value cannot be null or empty.", nameof(label));

            Member = member ?? throw new ArgumentNullException(nameof(member));
            AttributeType = attributeType ?? throw new ArgumentNullException(nameof(attributeType));
            Result = result;

            m_label = label;
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
