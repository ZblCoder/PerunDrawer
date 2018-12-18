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
			
			GUIContent labelText = data.Attributes.Exists(e => e is HideLabelAttribute) ? GUIContent.none : new GUIContent(data.Property.displayName);
			bool isFlags = data.Attributes.Exists(e => e is FlagsAttribute);
			
			if (data.Attributes.Exists(e => e is EnumButtonsAttribute))
			{
				Array list = Enum.GetValues(data.Value.GetType());
				if (list.Length > 0)
				{
					int buttonsIntValue = data.Property.intValue;
					int enumLength = list.Length;
					
					EditorGUILayout.BeginHorizontal();
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
						bool lastValue = isFlags ? (data.Property.intValue & value) == value : data.Property.intValue == value;

						bool newValue = GUILayout.Toggle(lastValue, data.Property.enumDisplayNames[i], style);
						if (newValue != lastValue)
						{
							if (isFlags)
							{
								if (newValue)
									buttonsIntValue |= value;
								else
									buttonsIntValue ^= value;
							}
							else
								buttonsIntValue = value;
						}
					}
					EditorGUILayout.EndHorizontal();
					data.Property.intValue = buttonsIntValue;
				}
			}
			else if (isFlags)
				data.Value = EditorGUILayout.EnumFlagsField(labelText, (Enum) data.Value);
			else
				EditorGUILayout.PropertyField(data.Property, labelText, true);

		}
	}
}