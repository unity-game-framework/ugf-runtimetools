using System;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateResultException : Exception
    {
        public ValidateMemberResult Result { get; }
        public override string Message { get { return Result.GetMessage(); } }

        public ValidateResultException(ValidateMemberResult result)
        {
            if (!result.IsValid()) throw new ArgumentException("Value should be valid.", nameof(result));

            Result = result;
        }
    }
}
