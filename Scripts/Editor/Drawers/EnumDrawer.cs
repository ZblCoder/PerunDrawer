using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PerunDrawer
{
	public class EnumDrawer : BaseDrawer
	{
		public EnumDrawer(PerunEditor editor) : base(editor) {}

		public override void Draw(PropertyData data)
		{
			//data.Attributes.ForEach(a => EditorGUILayout.LabelField(a.GetType().FullName));
			bool isFlags = data.Attributes.Exists(e => e is FlagsAttribute);
			
			if (data.Attributes.Exists(e => e is EnumButtonsAttribute))
			{
				EditorGUILayout.BeginHorizontal();
			
				if(!data.Attributes.Exists(e => e is HideLabelAttribute))
					EditorGUILayout.LabelField(new GUIContent(data.Property.displayName), GUILayout.Width(EditorGUIUtility.labelWidth - 5));
				
				Array list = Enum.GetValues(data.Value.GetType());
				if (list.Length > 0)
				{
					int buttonsIntValue = data.Property.intValue;
					int enumLength = list.Length;
					
					GUIStyle style = EditorStyles.miniButton;
					for (int i = 0; i < enumLength; i++)
					{
						if (enumLength > 1)
						{
							if (i == 0)
								style = EditorStyles.miniButtonLeft;
							else if (i == enumLength - 1)
								style = EditorStyles.miniButtonRight;
							else
								style = EditorStyles.miniButtonMid;
						}

						int value = (int) list.GetValue(i);
						bool lastValue = isFlags && value != 0 ? (data.Property.intValue & value) == value : data.Property.intValue == value;

						bool newValue = GUILayout.Toggle(lastValue, data.Property.enumDisplayNames[i], style);
						if (newValue != lastValue)
						{
							if (isFlags && value != 0)
							{
								if (newValue)
									buttonsIntValue |= value;
								else
									buttonsIntValue ^= value;
							}
							else
								buttonsIntValue = value;
							
							data.Property.intValue = buttonsIntValue;
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			else 
			{
				GUIContent labelText = data.Attributes.Exists(e => e is HideLabelAttribute) ? GUIContent.none : new GUIContent(data.Property.displayName);
				if (isFlags)
					data.Value = EditorGUILayout.EnumFlagsField(labelText, (Enum) data.Value);
				else
					EditorGUILayout.PropertyField(data.Property, labelText, true);
			}
		}
	}
}