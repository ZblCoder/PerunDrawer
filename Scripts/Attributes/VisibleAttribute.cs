using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class VisibleAttribute : Attribute
    {
        public string MemberName;
        public object Value;
        public bool IsNot;

        public VisibleAttribute(string memberName, bool isNot = false)
        {
            MemberName = memberName;
            Value = null;
            IsNot = isNot;
        }
        
        public VisibleAttribute(string memberName, object value, bool isNot = false)
        {
            MemberName = memberName;
            Value = value;
            IsNot = isNot;
        }
    }
}
