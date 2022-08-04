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

            Type type = target.GetType();

            ValidateFields(target, context, type.GetFields(), reports);
            ValidateProperties(target, context, type.GetProperties(), reports);
        }

        public static void ValidateFields(object target, IContext context, IReadOnlyList<FieldInfo> properties, ICollection<ValidateReport> reports)
        {
            ValidateMembers(target, context, properties, (x, member) => member.GetValue(x), reports);
        }

        public static void ValidateProperties(object target, IContext context, IReadOnlyList<PropertyInfo> properties, ICollection<ValidateReport> reports)
        {
            ValidateMembers(target, context, properties, (x, member) => member.GetValue(x), reports);
        }

        public static void ValidateMembers<T>(object target, IContext context, IReadOnlyList<T> members, ValidateGetMemberValueHandler<T> handler, ICollection<ValidateReport> reports) where T : MemberInfo
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (members == null) throw new ArgumentNullException(nameof(members));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (reports == null) throw new ArgumentNullException(nameof(reports));

            for (int i = 0; i < members.Count; i++)
            {
                T member = members[i];
                object value = null;

                using (new ContextValueScope(context, member))
                {
                    foreach (ValidateAttribute attribute in member.GetCustomAttributes<ValidateAttribute>())
                    {
                        value ??= handler.Invoke(target, member);

                        if (value != null)
                        {
                            ValidateResult result = attribute.Validate(value, context);

                            reports.Add(new ValidateReport(member, result));
                        }
                        else
                        {
                            reports.Add(new ValidateReport(member, ValueNotSpecifiedInvalidResult));
                        }
                    }
                }
            }
        }
    }
}
