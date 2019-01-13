using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class EnabledAttribute : Attribute
    {
        public string MemberName;
        public object Value;
        public bool IsNot;

        public EnabledAttribute(string memberName, bool isNot = false)
        {
            MemberName = memberName;
            Value = null;
            IsNot = isNot;
        }
        
        public EnabledAttribute(string memberName, object value, bool isNot = false)
        {
            MemberName = memberName;
            Value = value;
            IsNot = isNot;
        }
    }
}
