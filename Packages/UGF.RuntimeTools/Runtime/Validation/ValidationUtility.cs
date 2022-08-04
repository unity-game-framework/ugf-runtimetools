using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public static class ValidationUtility
    {
        public static ValidateResult ValueNotSpecifiedInvalidResult { get; } = ValidateResult.CreateInvalid("Value not specified.");

        public static void ValidateAll(object target, IContext context, ICollection<ValidateReport> reports)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (reports == null) throw new ArgumentNullException(nameof(reports));

            ValidateFields(target, context, reports);
            ValidateProperties(target, context, reports);
        }

        public static void ValidateFields(object target, IContext context, ICollection<ValidateReport> reports)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (reports == null) throw new ArgumentNullException(nameof(reports));

            IReadOnlyList<FieldInfo> members = target.GetType().GetFields();

            for (int i = 0; i < members.Count; i++)
            {
                FieldInfo member = members[i];
                object value = null;
                Type valueType = null;

                using (new ContextValueScope(context, member))
                {
                    foreach (ValidateAttribute attribute in member.GetCustomAttributes<ValidateAttribute>())
                    {
                        value ??= member.GetValue(target);

                        if (value != null)
                        {
                            valueType ??= value.GetType();

                            ValidateResult result = attribute.Validate(value, context);

                            reports.Add(new ValidateReport(member, attribute.GetType(), result));

                            if (valueType.IsClass)
                            {
                                ValidateProperties(target, context, reports);
                            }
                        }
                        else
                        {
                            reports.Add(new ValidateReport(member, attribute.GetType(), ValueNotSpecifiedInvalidResult));
                        }
                    }
                }
            }
        }

        public static void ValidateProperties(object target, IContext context, ICollection<ValidateReport> reports)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (reports == null) throw new ArgumentNullException(nameof(reports));

            IReadOnlyList<PropertyInfo> members = target.GetType().GetProperties();

            for (int i = 0; i < members.Count; i++)
            {
                PropertyInfo member = members[i];
                object value = null;
                Type valueType = null;

                using (new ContextValueScope(context, member))
                {
                    foreach (ValidateAttribute attribute in member.GetCustomAttributes<ValidateAttribute>())
                    {
                        value ??= member.GetValue(target);

                        if (value != null)
                        {
                            valueType ??= value.GetType();

                            ValidateResult result = attribute.Validate(value, context);

                            reports.Add(new ValidateReport(member, attribute.GetType(), result));

                            if (valueType.IsClass)
                            {
                                ValidateProperties(target, context, reports);
                            }
                        }
                        else
                        {
                            reports.Add(new ValidateReport(member, attribute.GetType(), ValueNotSpecifiedInvalidResult));
                        }
                    }
                }
            }
        }
    }
}
