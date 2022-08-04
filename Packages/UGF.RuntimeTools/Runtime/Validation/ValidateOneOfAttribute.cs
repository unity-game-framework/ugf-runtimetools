using System;
using System.Text;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateOneOfAttribute : ValidateAttribute
    {
        public object[] Values { get; }

        public ValidateOneOfAttribute(params object[] values) : this(values, typeof(object))
        {
        }

        public ValidateOneOfAttribute(object[] values, Type targetType) : base(targetType)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            return Array.IndexOf(Values, value) != -1 ? ValidateResult.Valid : ValidateResult.CreateInvalid($"Value must be one of '{GetValuesMessage(Values)}' values.");
        }

        private string GetValuesMessage(object[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            var builder = new StringBuilder();

            for (int i = 0; i < values.Length; i++)
            {
                object value = values[i];

                builder.Append(value);

                if (i < values.Length - 1)
                {
                    builder.Append(", ");
                }
            }

            return builder.ToString();
        }
    }
}
