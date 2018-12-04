using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace PerunDrawer
{
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = false)]
    [CanEditMultipleObjects]
    public class BaseDrawer : Editor 
    {
        private Dictionary<string, AnimBool> _foldoutStates = new Dictionary<string, AnimBool>();

        private void DrawDefault(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property, true);
        }
        
        private void DrawProperty(SerializedProperty property, Type type, List<Attribute> attrList)
        {
            if(attrList == null)
                attrList = Utilities.GetAttrib(type, property.name);
            
            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                if (attrList.Exists(e => e is ListDrawerAttribute))
                {
                    FieldInfo info = type.GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (info != null)
                    {
                        Type type1 = info.FieldType.IsGenericType ? info.FieldType.GetGenericArguments().First() : info.FieldType.GetElementType();
                        DrawList(property, type1, attrList);
                        return;
                    }
                }
            }
            else
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                DrawGeneric(property, type);
                return;
            }

            DrawDefault(property);
        }

        private void DrawList(SerializedProperty property, Type type, List<Attribute> attrList)
        {
            var attr = attrList.FirstOrDefault() as ListDrawerAttribute;
            if(attr == null)
                attr = new ListDrawerAttribute();
            
            EditorGUILayout.BeginVertical(Style.ListBackground);
            
            AnimBool animBool = GetAnimBool(property.propertyPath, property.isExpanded);
            // Header

            EditorGUILayout.BeginHorizontal(Style.Toolbar);
            
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, new GUIContent(property.displayName + " [" + property.arraySize + "]"));
            animBool.target = property.isExpanded;

            if (attr.ShowAddButton && GUILayout.Button("", Style.ToolbarAddButton, GUILayout.Width(32)))
            {
                int index = property.arraySize;
                property.InsertArrayElementAtIndex(index);
            }

            EditorGUILayout.EndHorizontal();
            //

            EditorGUILayout.BeginVertical(Style.ListContent);
            
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))

                for (int i = 0; i < property.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal(Style.ListItem);
                    
                    EditorGUILayout.BeginVertical();
                    DrawProperty(property.GetArrayElementAtIndex(i), type, attrList);
                    EditorGUILayout.EndVertical();
                    //
            
                    if (attr.ShowRemoveButton && GUILayout.Button("", Style.ListDeleteItem, GUILayout.Width(16)))
                    {
                        
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
            EditorGUILayout.EndFadeGroup();
            
            EditorGUILayout.EndVertical();
            //
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawGeneric(SerializedProperty iterator, Type type)
        {
            string parentPath = iterator.propertyPath;
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                if (string.IsNullOrEmpty(parentPath) || iterator.propertyPath.IndexOf(parentPath, StringComparison.Ordinal) == 0)
                {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                        DrawProperty(iterator.Copy(), type, null);
                }
        }
        
        private void DrawGenericBox(SerializedProperty iterator, Type type)
        {
            EditorGUILayout.BeginVertical(Style.ListBackground);
            
            AnimBool animBool = GetAnimBool(iterator.propertyPath, iterator.isExpanded);
            // Header

            EditorGUILayout.BeginHorizontal(Style.Toolbar);
            
            iterator.isExpanded = EditorGUILayout.Foldout(iterator.isExpanded, new GUIContent(iterator.displayName));
            animBool.target = iterator.isExpanded;
            
            EditorGUILayout.EndHorizontal();
            //

            EditorGUILayout.BeginVertical(Style.ListContent);
                
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                DrawGeneric(iterator, type);
            
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
            
            //
            
            EditorGUILayout.EndVertical();
        }

        private AnimBool GetAnimBool(string path, bool value)
        {
            AnimBool result;
            if (!_foldoutStates.TryGetValue(path, out result))
            {
                result = new AnimBool(value);
                _foldoutStates.Add(path, result);
                result.valueChanged.RemoveAllListeners();
                result.valueChanged.AddListener(Repaint);
            }
            return result;
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            SerializedProperty iterator = serializedObject.GetIterator();
            DrawGeneric(iterator, serializedObject.targetObject.GetType());
            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
            //base.OnInspectorGUI();
        }
    }
}

