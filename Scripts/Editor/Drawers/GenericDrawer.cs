using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace PerunDrawer
{
    public class GenericDrawer : BaseDrawer
    {
        public GenericDrawer(PerunEditor editor) : base(editor) {}

        public void DrawProperies(PropertyData data)
        {
            var iterator = data.Property.Copy();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                if (string.IsNullOrEmpty(data.Property.propertyPath) || iterator.propertyPath.IndexOf(data.Property.propertyPath, StringComparison.Ordinal) == 0)
                {
                    PropertyData itemData = new PropertyData(iterator, data);
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                        Editor.Property.Draw(itemData);
                }
        }
        
        public override void Draw(PropertyData data)
        {
            var attr = data.Attributes.FirstOrDefault() as StructDrawerAttribute;
            if(attr == null)
                attr = new StructDrawerAttribute();
            
            switch (data.Parent != null ? attr.ItemType : StructDrawerAttribute.ItemTypes.None)
            {
                case StructDrawerAttribute.ItemTypes.FadeGroup:
                    EditorGUI.indentLevel++;
                    //EditorGUILayout.GetControlRect(false, 10, GUILayout.Width(8));
                    EditorGUILayout.BeginVertical();
                    AnimBool animBool = Editor.GetAnimBool(data.Property.propertyPath, data.Property.isExpanded);
                    data.Property.isExpanded = EditorGUILayout.Foldout(data.Property.isExpanded, new GUIContent(data.Property.displayName));
                    animBool.target = data.Property.isExpanded;
                    if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                        DrawProperies(data);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndFadeGroup();
                    EditorGUI.indentLevel--;
                    break;
                case StructDrawerAttribute.ItemTypes.Box:
                    EditorGUILayout.BeginVertical(Style.ListItemBox);
                    DrawProperies(data);
                    EditorGUILayout.EndVertical();
                    break;
                case StructDrawerAttribute.ItemTypes.HorizontalGroup:
                    EditorGUILayout.BeginHorizontal();
                    DrawProperies(data);
                    EditorGUILayout.EndHorizontal();
                    break;
                default:
                    EditorGUILayout.BeginVertical();
                    DrawProperies(data);
                    EditorGUILayout.EndVertical();
                    break;
            }
        }
    }
}
