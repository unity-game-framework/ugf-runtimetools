using System;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public static partial class ValidateUtility
    {
        [Obsolete("Validate method with 'all' argument has been deprecated.")]
        public static void Validate(object target, IContext context, bool all)
        {
            Validate(target, context);
        }

        [Obsolete("Validate method with 'all' argument has been deprecated.")]
        public static bool Validate(object target, IContext context, out ValidateReport report, bool all)
        {
            return Validate(target, context, out report);
        }

        [Obsolete("ValidateFields method with 'all' argument has been deprecated.")]
        public static bool ValidateFields(object target, IContext context, out ValidateReport report, bool all)
        {
            return ValidateFields(target, context, out report);
        }

        [Obsolete("ValidateProperties method with 'all' argument has been deprecated.")]
        public static bool ValidateProperties(object target, IContext context, out ValidateReport report, bool all)
        {
            return ValidateProperties(target, context, out report);
        }
    }
}
