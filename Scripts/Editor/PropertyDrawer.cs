using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace PerunDrawer
{
	public static class PropertyDrawer
	{
		public static void Draw(SerializedProperty property, Type type, List<Attribute> attrList)
		{
			if (property.isArray && property.propertyType != SerializedPropertyType.String)
			{
				if (attrList.Exists(e => e is ListDrawerAttribute))
				{
					FieldInfo info = type.GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
					if (info != null)
					{
						Type type1 = info.FieldType.IsGenericType ? info.FieldType.GetGenericArguments().First() : info.FieldType.GetElementType();
						ListDrawer.Draw(property, type1, attrList);
						return;
					}
				}
			}
			else
			if (property.propertyType == SerializedPropertyType.Generic)
			{
				GenericDrawer.DrawBox(property, type, attrList);
				return;
			}

			EditorGUILayout.PropertyField(property, true);
		}
	}
}