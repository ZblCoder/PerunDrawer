
using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class HideLabelAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class LabelTextAttribute : Attribute
    {
        public string Text;
        public LabelTextAttribute(string text)
        {
            Text = text;
        }
    }
}
