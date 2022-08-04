using System;
using System.Text.RegularExpressions;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateMatchAttribute : ValidateAttribute
    {
        public string Pattern { get; }
        public RegexOptions Options { get; }

        public ValidateMatchAttribute(string pattern, RegexOptions options = RegexOptions.None) : this(pattern, typeof(object), options)
        {
        }

        public ValidateMatchAttribute(string pattern, Type targetType, RegexOptions options = RegexOptions.None) : base(targetType)
        {
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentException("Value cannot be null or empty.", nameof(pattern));

            Pattern = pattern;
            Options = options;
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            var regex = new Regex(Pattern, Options);
            bool result = regex.IsMatch(value.ToString());

            return result ? ValidateResult.Valid : ValidateResult.CreateInvalid("Value must match specified pattern.");
        }
    }
}
