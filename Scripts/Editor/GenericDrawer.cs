using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PerunDrawer
{
    public static class GenericDrawer
    {
        public static void Draw(SerializedProperty iterator, Type type, List<Attribute> attrList)
        {
            string parentPath = iterator.propertyPath;
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                if (string.IsNullOrEmpty(parentPath) || iterator.propertyPath.IndexOf(parentPath, StringComparison.Ordinal) == 0)
                {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                        PropertyDrawer.Draw(iterator.Copy(), type, attrList);
                }
        }
        
        public static void DrawBox(SerializedProperty iterator, Type type, List<Attribute> attrList)
        {
            EditorGUILayout.BeginVertical(Style.ListBackground);
            
            //AnimBool animBool = GetAnimBool(iterator.propertyPath, iterator.isExpanded);
            // Header

            EditorGUILayout.BeginHorizontal(Style.Toolbar);
            
            iterator.isExpanded = EditorGUILayout.Foldout(iterator.isExpanded, new GUIContent(iterator.displayName));
            //animBool.target = iterator.isExpanded;
            
            EditorGUILayout.EndHorizontal();
            //

            EditorGUILayout.BeginVertical(Style.ListContent);
                
            //if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            if (EditorGUILayout.BeginFadeGroup(iterator.isExpanded ? 1 : 0))
                GenericDrawer.Draw(iterator, type, attrList);
            
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
            
            //
            
            EditorGUILayout.EndVertical();
        }
        
    }
}
