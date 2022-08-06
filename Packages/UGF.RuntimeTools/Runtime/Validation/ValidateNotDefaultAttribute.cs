using System;
using System.Collections.Generic;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateNotDefaultAttribute : ValidateAttribute
    {
        private static readonly Dictionary<Type, object> m_defaults = new Dictionary<Type, object>();

        public ValidateNotDefaultAttribute() : this(typeof(object))
        {
        }

        public ValidateNotDefaultAttribute(Type targetType) : base(targetType)
        {
            ValidateMembers = false;
        }

        protected override ValidateResult OnValidate(object value, IContext context)
        {
            Type type = value.GetType();

            if (!type.IsValueType)
            {
                return ValidateResult.CreateInvalid($"Value must be type of '{typeof(ValueType)}'.");
            }

            return !value.Equals(GetDefaultValue(type)) ? ValidateResult.Valid : ValidateResult.CreateInvalid("Value must not be default.");
        }

        private static object GetDefaultValue(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (!m_defaults.TryGetValue(type, out object value))
            {
                value = Activator.CreateInstance(type);

                m_defaults.Add(type, value);
            }

            return value;
        }
    }
}
