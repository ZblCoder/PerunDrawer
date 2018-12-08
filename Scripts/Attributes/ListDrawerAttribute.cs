using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class ListDrawerAttribute : Attribute
    {
        public bool ShowAddButton = true;
        public bool ShowRemoveButton = true;
        public bool ShowDrag = true;
        public bool ShowCount = true;
        
        public enum ItemTypes
        {
            None,
            FadeGroup,
            Box,
            HorizontalGroup
        }
        
        public ItemTypes ItemType = ItemTypes.FadeGroup;
    }
}
