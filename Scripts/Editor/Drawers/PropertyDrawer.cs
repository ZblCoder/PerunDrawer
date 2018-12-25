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
		
		public override void Draw(PropertyData data)
		{
			if(!IsVisible(data))
				return;
			
			switch (data.Type)
			{
				case PropertyData.Types.SelfDrawer:
					EditorGUILayout.PropertyField(data.Property, new GUIContent(data.Property.displayName));
					return;
				case PropertyData.Types.List:
					Editor.BaseAttributes.Draw(data);
					Editor.List.Draw(data);
					return;
				case PropertyData.Types.Generic:
					Editor.BaseAttributes.Draw(data);
					Editor.Generic.Draw(data);
					return;
			}

			if (data.Property.propertyType == SerializedPropertyType.Enum)
			{
				Editor.Enum.Draw(data);
				return;
			}
			
			GUIContent labelText = data.Attributes.Exists(e => e is HideLabelAttribute) ? GUIContent.none : new GUIContent(data.Property.displayName);
			
			EditorGUILayout.PropertyField(data.Property, labelText, true);
			
			/*
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(data.Property.propertyPath.Replace(".Array.data[", "["));
			EditorGUILayout.LabelField((data.Value != null ? data.Value.GetType().ToString() : "null"));
			EditorGUILayout.EndHorizontal();
			
			if (data.Attributes != null)
			{
				EditorGUI.indentLevel++;
				foreach (var a in data.Attributes)
				{
					EditorGUILayout.LabelField("A: " + a.GetType().ToString());
				}
				EditorGUI.indentLevel--;
			}
			*/
		}

		private bool Equals(object objA, object objB)
		{
			return objA == objB || (objA != null && objB != null && objA.Equals(objB));
		}
		
		private bool IsVisible(PropertyData data)
		{
			bool visible = true;
			foreach (var attr in data.Attributes)
				if(attr is VisibleAttribute)
				{
					VisibleAttribute visibleAttr = attr as VisibleAttribute;
					if (visibleAttr.Value == null)
					{
						bool visibleValue;
						if (Utilities.GetValue(data.Parent.Value, visibleAttr.MemberName, out visibleValue))
							visible = visible && (visibleAttr.IsNot ? !visibleValue : visibleValue);
						else
							EditorGUILayout.HelpBox("VisibleAttribute: MemberName \"" + visibleAttr.MemberName + "\" not found!", MessageType.Error);
					}
					else
					{
						object visibleValue;
						if (Utilities.GetValue(data.Parent.Value, visibleAttr.MemberName, out visibleValue, false)
							&& visibleValue.GetType() == visibleAttr.Value.GetType())
						{
							if (visibleValue.GetType().IsEnum && visibleAttr.Value.GetType().IsEnum
								&& visibleValue.GetType().GetCustomAttributes(false).ToList().Exists(e => e is FlagsAttribute)
							    && (int) visibleValue != 0 && (int) visibleAttr.Value != 0)
							{
								visible = visible && (visibleAttr.IsNot
											  ? ((int) visibleValue & (int) visibleAttr.Value) != (int) visibleAttr.Value
											  : ((int) visibleValue & (int) visibleAttr.Value) == (int) visibleAttr.Value);
								continue;
							}
							visible = visible && (visibleAttr.IsNot ? !Equals(visibleValue, visibleAttr.Value) : Equals(visibleValue, visibleAttr.Value));
							continue;
						}
						EditorGUILayout.HelpBox("VisibleAttribute: MemberName \"" + visibleAttr.MemberName + "\" not found!", MessageType.Error);
					}
				}
			
			return visible;
		}
	}
}