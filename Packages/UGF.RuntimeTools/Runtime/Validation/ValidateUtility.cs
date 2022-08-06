using System;
using System.Collections.Generic;
using System.Reflection;
using UGF.RuntimeTools.Runtime.Contexts;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public static class ValidateUtility
    {
        public static ValidateResult ValueNotSpecifiedInvalidResult { get; } = ValidateResult.CreateInvalid("Value not specified.");

        public static void Validate(object target, IContext context, bool all = true)
        {
            if (!Validate(target, context, out ValidateReport report, all))
            {
                throw report.Results.Count > 1
                    ? new ValidateReportException(report)
                    : new ValidateResultException(report.Results[0]);
            }
        }

        public static bool Validate(object target, IContext context, out ValidateReport report, bool all = true)
        {
            report = default;

            bool fields = ValidateFields(target, context, out ValidateReport fieldsReport, all);
            bool properties = ValidateProperties(target, context, out ValidateReport propertiesReport, all);

            if (!fields || !properties)
            {
                report = fieldsReport.HasResults ? fieldsReport : propertiesReport;

                if (all && fieldsReport.HasResults && propertiesReport.HasResults)
                {
                    for (int i = 0; i < propertiesReport.Results.Count; i++)
                    {
                        report.Results.Add(propertiesReport.Results[i]);
                    }
                }
            }

            return fields && properties;
        }

        public static bool ValidateFields(object target, IContext context, out ValidateReport report, bool all = true)
        {
            report = default;

            ValidateMembers(target, context, ref report, all, x => x.GetType().GetFields(), (x, y) => y.GetValue(x));

            return !report.HasResults;
        }

        public static bool ValidateProperties(object target, IContext context, out ValidateReport report, bool all = true)
        {
            report = default;

            ValidateMembers(target, context, ref report, all, x => x.GetType().GetProperties(), (x, y) => y.GetValue(x));

            return !report.HasResults;
        }

        private static void ValidateMembers<T>(object target, IContext context, ref ValidateReport report, bool all, Func<object, IReadOnlyList<T>> membersHandler, Func<object, T, object> valueHandler) where T : MemberInfo
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (membersHandler == null) throw new ArgumentNullException(nameof(membersHandler));
            if (valueHandler == null) throw new ArgumentNullException(nameof(valueHandler));

            IReadOnlyList<T> members = membersHandler.Invoke(target);

            for (int i = 0; i < members.Count; i++)
            {
                T member = members[i];
                object value = null;
                Type valueType = null;

                using (new ContextValueScope(context, member))
                {
                    foreach (ValidateAttribute attribute in member.GetCustomAttributes<ValidateAttribute>())
                    {
                        value ??= valueHandler.Invoke(target, member);

                        if (value != null)
                        {
                            valueType ??= value.GetType();

                            ValidateResult result = attribute.Validate(value, context);

                            if (!result)
                            {
                                if (!report.HasResults)
                                {
                                    report = new ValidateReport(new List<ValidateMemberResult>());
                                }

                                report.Results.Add(new ValidateMemberResult(member, attribute.GetType(), result));

                                if (!all)
                                {
                                    return;
                                }
                            }

                            if (attribute.ValidateMembers)
                            {
                                ValidateMembers(value, context, ref report, all, membersHandler, valueHandler);
                            }
                        }
                        else
                        {
                            if (!report.HasResults)
                            {
                                report = new ValidateReport(new List<ValidateMemberResult>());
                            }

                            report.Results.Add(new ValidateMemberResult(member, attribute.GetType(), ValueNotSpecifiedInvalidResult));

                            if (!all)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
