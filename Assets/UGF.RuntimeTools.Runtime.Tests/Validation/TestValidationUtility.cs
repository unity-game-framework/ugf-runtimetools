using UGF.RuntimeTools.Runtime.Validation;

namespace UGF.RuntimeTools.Runtime.Tests.Validation
{
    public class TestValidationUtility
    {
        private class Target
        {
            [Validate]
            public object HasValue { get; set; }
        }
    }
}
