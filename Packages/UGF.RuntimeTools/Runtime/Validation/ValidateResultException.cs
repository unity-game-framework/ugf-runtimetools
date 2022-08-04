using System;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateResultException : Exception
    {
        public ValidateMemberResult Result { get; }

        public ValidateResultException(ValidateMemberResult result) : base(result.IsValid() ? result.GetMessage() : throw new ArgumentException("Value should be valid.", nameof(result)))
        {
            Result = result;
        }
    }
}
