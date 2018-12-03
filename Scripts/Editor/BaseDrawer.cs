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

        private void DrawProperty(SerializedProperty property, Type type, List<Attribute> attrList)
        {
            if(attrList == null)
                attrList = Utilities.GetAttrib(type, property.name);
            
            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                FieldInfo info = type.GetField(property.name);
                Type type1 = info.FieldType.IsGenericType ? info.FieldType.GetGenericArguments().First() : info.FieldType.GetElementType();
                
                DrawList(property, type1, attrList);
                return;
            }
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                FieldInfo info = type.GetField(property.name);
                DrawGenericBox(property, info.FieldType);
                return;
            }
            
            EditorGUILayout.PropertyField(property, true, new GUILayoutOption[0]);
        }

        private void DrawList(SerializedProperty property, Type type, List<Attribute> attrList)
        {
            EditorGUILayout.LabelField(property.displayName + " [" + property.arraySize + "]");
            
            for (int i = 0; i < property.arraySize; i++)
               DrawProperty(property.GetArrayElementAtIndex(i), type, attrList);

            if (GUILayout.Button("+"))
            {
                property.InsertArrayElementAtIndex(property.arraySize);
            }
        }
        
        private void DrawGeneric(SerializedProperty iterator, Type type)
        {
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    DrawProperty(iterator.Copy(), type, null);
        }
        
        private void DrawGenericBox(SerializedProperty iterator, Type type)
        {
            EditorGUILayout.BeginVertical("Box");
            
            AnimBool animBool;
            if (!_foldoutStates.TryGetValue(iterator.propertyPath, out animBool))
            {
                animBool = new AnimBool(true);
                _foldoutStates.Add(iterator.propertyPath, animBool);
                animBool.valueChanged.RemoveAllListeners();
                animBool.valueChanged.AddListener(Repaint);
            }
                
            iterator.isExpanded = EditorGUILayout.Foldout(iterator.isExpanded, new GUIContent(iterator.displayName));
            animBool.target = iterator.isExpanded;
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                DrawGeneric(iterator, type);
            
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
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

