using System;
using System.Reflection;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public readonly struct ValidateReport
    {
        public MemberInfo Member { get; }
        public ValidateResult Result { get; }

        public ValidateReport(MemberInfo member, ValidateResult result)
        {
            if (!result.IsValid()) throw new ArgumentException("Value should be valid.", nameof(result));

            Member = member ?? throw new ArgumentNullException(nameof(member));
            Result = result;
        }
    }
}
