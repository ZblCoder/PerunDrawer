using System;

namespace PerunDrawer
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public sealed class StructDrawerAttribute : Attribute
	{
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

