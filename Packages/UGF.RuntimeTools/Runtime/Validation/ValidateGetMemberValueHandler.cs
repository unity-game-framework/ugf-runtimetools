using System.Reflection;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public delegate object ValidateGetMemberValueHandler<in T>(object target, T member) where T : MemberInfo;
}
