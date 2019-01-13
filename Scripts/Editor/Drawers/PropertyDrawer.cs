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

			bool isDisabled = !IsEnabled(data);

			if (isDisabled)
				Editor.IsDisabled = true;
			
			switch (data.Type)
			{
				case PropertyData.Types.SelfDrawer:
					EditorGUI.BeginDisabledGroup(Editor.IsDisabled);
					EditorGUILayout.PropertyField(data.Property, new GUIContent(data.Property.displayName));
					EditorGUI.EndDisabledGroup();
					break;
				case PropertyData.Types.List:
					Editor.BaseAttributes.Draw(data);
					Editor.List.Draw(data);
					break;
				case PropertyData.Types.Generic:
					Editor.BaseAttributes.Draw(data);
					Editor.Generic.Draw(data);
					break;
				default:
					EditorGUI.BeginDisabledGroup(Editor.IsDisabled);
					if (data.Property.propertyType != SerializedPropertyType.Enum)
					{
						GUIContent labelText = data.Attributes.Exists(e => e is HideLabelAttribute) ? GUIContent.none : new GUIContent(data.Property.displayName);
						EditorGUILayout.PropertyField(data.Property, labelText, true);
					}
					else
						Editor.Enum.Draw(data);
					EditorGUI.EndDisabledGroup();
					break;
			}
			
			if (isDisabled)
				Editor.IsDisabled = false;
			
			/*
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(data.Property.propertyPath.Replace(".Array.data[", "["));
			EditorGUILayout.LabelField((data.Value != null ? data.ValueType.ToString() : "null"));
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
			if (Editor.IsDisabled)
				return true;
			
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
		
		private bool IsEnabled(PropertyData data)
		{
			bool enabled = true;
			foreach (var attr in data.Attributes)
			{
				EnabledAttribute enabledAttr = attr as EnabledAttribute;
				if (enabledAttr != null)
				{
					if (enabledAttr.Value == null)
					{
						bool visibleValue;
						if (Utilities.GetValue(data.Parent.Value, enabledAttr.MemberName, out visibleValue))
							enabled = enabled && (enabledAttr.IsNot ? !visibleValue : visibleValue);
						else
							EditorGUILayout.HelpBox("EnabledAttribute: MemberName \"" + enabledAttr.MemberName + "\" not found!", MessageType.Error);
					}
					else
					{
						object enabledValue;
						if (Utilities.GetValue(data.Parent.Value, enabledAttr.MemberName, out enabledValue, false)
						    && enabledValue.GetType() == enabledAttr.Value.GetType())
						{
							if (enabledValue.GetType().IsEnum && enabledAttr.Value.GetType().IsEnum
							    && enabledValue.GetType().GetCustomAttributes(false).ToList().Exists(e => e is FlagsAttribute)
							    && (int) enabledValue != 0 && (int) enabledAttr.Value != 0)
							{
								enabled = enabled && (enabledAttr.IsNot
									          ? ((int) enabledValue & (int) enabledAttr.Value) != (int) enabledAttr.Value
									          : ((int) enabledValue & (int) enabledAttr.Value) == (int) enabledAttr.Value);
								continue;
							}

							enabled = enabled && (enabledAttr.IsNot ? !Equals(enabledValue, enabledAttr.Value) : Equals(enabledValue, enabledAttr.Value));
							continue;
						}
						EditorGUILayout.HelpBox("EnabledAttribute: MemberName \"" + enabledAttr.MemberName + "\" not found!", MessageType.Error);
					}
				}
			}
			return enabled;
		}
	}
}