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
            var buttons = Utilities.FindByAttribute<ButtonAttribute, MethodInfo>(data.Value);

            foreach (var buttonPair in buttons)
                if(buttonPair.Key.Align == ButtonAttribute.AlignTypes.Top)
                    if (GUILayout.Button(string.IsNullOrEmpty(buttonPair.Key.Caption) ? buttonPair.Value.Name : buttonPair.Key.Caption))
                        buttonPair.Value.Invoke(data.Value, null);
            
            var iterator = data.Property.Copy();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                if (string.IsNullOrEmpty(data.Property.propertyPath) || iterator.propertyPath.IndexOf(data.Property.propertyPath, StringComparison.Ordinal) == 0)
                {
                    PropertyData itemData = new PropertyData(iterator, data);
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                        Editor.Property.Draw(itemData);
                }
            
            foreach (var buttonPair in buttons)
                if(buttonPair.Key.Align == ButtonAttribute.AlignTypes.Bottom)
                    if (GUILayout.Button(string.IsNullOrEmpty(buttonPair.Key.Caption) ? buttonPair.Value.Name : buttonPair.Key.Caption))
                        buttonPair.Value.Invoke(data.Value, null);
        }
        
        public override void Draw(PropertyData data)
        {
            //data.Attributes.ForEach(a => EditorGUILayout.LabelField(a.GetType().FullName));
            
            var attr = data.Attributes.FirstOrDefault() as StructDrawerAttribute;
            if(attr == null)
                attr = new StructDrawerAttribute();
            
            switch (data.Parent != null ? attr.ItemType : StructDrawerAttribute.ItemTypes.None)
            {
                case StructDrawerAttribute.ItemTypes.FadeGroup:
                    //EditorGUILayout.GetControlRect(false, 10, GUILayout.Width(8));
                    EditorGUILayout.BeginVertical();
                    AnimBool animBool = Editor.GetAnimBool(data.Property.propertyPath, data.Property.isExpanded);
                    //data.Property.isExpanded = EditorGUILayout.Foldout(data.Property.isExpanded, new GUIContent(data.Property.displayName));
                    
                    if(EditorGUILayout.DropdownButton(new GUIContent(data.Property.displayName), FocusType.Passive, 
                        data.Property.isExpanded ? Style.FoldoutExpanded : Style.Foldout))
                        data.Property.isExpanded = !data.Property.isExpanded;
                    
                    animBool.target = data.Property.isExpanded;
                    if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                    {
                        EditorGUI.indentLevel++;
                        DrawProperies(data);
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.EndFadeGroup();
                    EditorGUILayout.EndVertical();
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
