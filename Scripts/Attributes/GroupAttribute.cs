using System;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class GroupAttribute : Attribute
    {
        public enum ItemTypes
        {
            None,
            FadeGroup,
            Box,
            HorizontalGroup
        }
		
        public string Name;
        public ItemTypes ItemType = ItemTypes.FadeGroup;
        
        public GroupAttribute(string name, ItemTypes type = ItemTypes.FadeGroup)
        {
            Name = name;
            ItemType = type;
        }
    }
}

