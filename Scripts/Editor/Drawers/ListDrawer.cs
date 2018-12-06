﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace PerunDrawer
{
    public class ListDrawer : BaseDrawer
    {
        public ListDrawer(PerunEditor editor) : base(editor) {}
        
        public override void Draw(SerializedProperty property, Type type, List<Attribute> attrList)
        {
            var attr = attrList.FirstOrDefault() as ListDrawerAttribute;
            if(attr == null)
                attr = new ListDrawerAttribute();
            
            EditorGUILayout.BeginVertical(Style.ListBackground);
            
            AnimBool animBool = Editor.GetAnimBool(property.propertyPath, property.isExpanded);
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
                    Editor.Property.Draw(property.GetArrayElementAtIndex(i), type, attrList);
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
    }
}