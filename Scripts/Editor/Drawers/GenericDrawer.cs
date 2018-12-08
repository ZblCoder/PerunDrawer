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
        
        public override void Draw(SerializedProperty property, Type type, List<Attribute> attrList)
        {
            var iterator = property.Copy();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                if (string.IsNullOrEmpty(property.propertyPath) || iterator.propertyPath.IndexOf(property.propertyPath, StringComparison.Ordinal) == 0)
                {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                        Editor.Property.Draw(iterator.Copy(), type, attrList);
                    
                }
        }
        
        public void DrawBox(SerializedProperty iterator, object parent, Type type, List<Attribute> attrList)
        {
            EditorGUILayout.BeginVertical(Style.ListBackground);
            
            AnimBool animBool = Editor.GetAnimBool(iterator.propertyPath, iterator.isExpanded);
            // Header

            EditorGUILayout.BeginHorizontal(Style.Toolbar);
            
            iterator.isExpanded = EditorGUILayout.Foldout(iterator.isExpanded, new GUIContent(iterator.displayName));
            animBool.target = iterator.isExpanded;
            
            EditorGUILayout.EndHorizontal();
            //

            EditorGUILayout.BeginVertical(Style.ListContent);
                
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                Editor.Generic.Draw( iterator, type, attrList);
            
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
            
            //
            
            EditorGUILayout.EndVertical();
        }
        
    }
}
