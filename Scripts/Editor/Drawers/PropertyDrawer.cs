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
			switch (data.Type)
			{
				case PropertyData.Types.List:
					Editor.List.Draw(data);
					return;
				case PropertyData.Types.Generic:
					Editor.Generic.Draw(data);
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
	}
}