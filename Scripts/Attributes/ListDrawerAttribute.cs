using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class ListDrawerAttribute : Attribute
    {
        public bool ShowAddButton = true;
        public bool ShowRemoveButton = true;
    }
}
