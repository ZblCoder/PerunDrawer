using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PerunDrawerAttribute : Attribute
    {
        public bool IsFull = true;
    }
}
