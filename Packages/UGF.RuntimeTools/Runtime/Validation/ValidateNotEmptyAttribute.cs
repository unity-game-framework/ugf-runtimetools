using System.Collections;
using UGF.RuntimeTools.Runtime.Collections;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateNotEmptyAttribute : ValidateAttribute
    {
        public ValidateNotEmptyAttribute() : base(typeof(IEnumerable))
        {
            ValidateMembers = false;
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            int count = CollectionsUtility.GetCount((IEnumerable)value);

            return count > 0 ? ValidateResult.Valid : ValidateResult.CreateInvalid("Value must be not empty.");
        }
    }
}
