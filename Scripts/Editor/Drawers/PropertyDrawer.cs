using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PerunDrawer
{
	public class PropertyDrawer : BaseDrawer
	{
		public PropertyDrawer(PerunEditor editor) : base(editor) {}

		public override void Draw(SerializedProperty property, Type type, List<Attribute> attrList)
		{
			var parent = Utilities.GetParent(property);
			if (property.isArray && property.propertyType != SerializedPropertyType.String)
			{
				//if (attrList.Exists(e => e is ListDrawerAttribute))
				{
					FieldInfo info = type.GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
					if (info != null)
					{
						Type type1 = info.FieldType.IsGenericType ? info.FieldType.GetGenericArguments().First() : info.FieldType.GetElementType();
						Editor.List.Draw(property, type1, info.GetCustomAttributes(false).Cast<Attribute>().ToList());
						return;
					}
				}
			}
			else
			if (property.propertyType == SerializedPropertyType.Generic)
			{
				Editor.Generic.Draw(property, type, attrList);
				return;
			}

			attrList = Utilities.GetAttrib(parent.GetType(), property.name);
			GUIContent labelText = attrList != null && attrList.Exists(e => e is HideLabelAttribute) ? GUIContent.none : new GUIContent(property.displayName);
			
			EditorGUILayout.PropertyField(property, labelText, true);
		}
	}
}