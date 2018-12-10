using System;
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
        
        public class DragData
        {
            public Editor Editor;
            public string ListPath;
            public int Index;

            public DragData(Editor editor, string listPath, int index)
            {
                Editor = editor;
                ListPath = listPath;
                Index = index;
            }
        }
        public static DragData ListDragData = null;
/*
        public override void Draw(PropertyData data)
        {            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(data.Property.propertyPath.Replace(".Array.data[", "["));
            EditorGUILayout.LabelField((data.Value != null ? data.Value.GetType().ToString() : "null"));
            EditorGUILayout.EndHorizontal();
			
            if (data.Attributes != null)
            {
                EditorGUI.indentLevel++;
                foreach (var a in data.Attributes)
                {
                    EditorGUILayout.LabelField(a.GetType().ToString());
                }
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.indentLevel++;
            for (int i = 0; i < data.Property.arraySize; i++)
            {
                PropertyData d = new PropertyData(data, i);
                Editor.Property.Draw1(d);
            }
            EditorGUI.indentLevel--;
            
        }
        
        private void DrawItem(ListDrawerAttribute attr, SerializedProperty property, int index, Type type, List<Attribute> attrList, object parent)
        {
            switch (attr.ItemType)
            {
                case ListDrawerAttribute.ItemTypes.FadeGroup:
                    if (property.propertyType == SerializedPropertyType.Generic)
                        EditorGUILayout.GetControlRect(false, 10, GUILayout.Width(8));
                    EditorGUILayout.BeginVertical();
                    if (property.propertyType == SerializedPropertyType.Generic)
                    {
                        AnimBool animBool = Editor.GetAnimBool(property.propertyPath, property.isExpanded);
                        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, new GUIContent(index + " :"));
                        animBool.target = property.isExpanded;
                        if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                            Editor.Property.Draw(property, type, attrList);
                        EditorGUILayout.EndFadeGroup();
                    }
                    else
                        Editor.Property.Draw(property, type, attrList);
                    EditorGUILayout.EndVertical();
                    break;
                case ListDrawerAttribute.ItemTypes.Box:
                    EditorGUILayout.BeginVertical(Style.ListItemBox);
                    Editor.Property.Draw(property, type, attrList);
                    EditorGUILayout.EndVertical();
                    break;
                case ListDrawerAttribute.ItemTypes.HorizontalGroup:
                    EditorGUILayout.BeginHorizontal();
                    Editor.Property.Draw(property, type, attrList);
                    EditorGUILayout.EndHorizontal();
                    break;
                default:
                    EditorGUILayout.BeginVertical();
                    Editor.Property.Draw(property, type, attrList);
                    EditorGUILayout.EndVertical();
                    break;
            }
        }
        */
        private bool DropValidate(string listPath)
        {
            //return true;
            if (ListDragData != null)
            {
                if (ListDragData.Editor == Editor && ListDragData.ListPath == listPath)
                    return true;
                /*
                SerializedProperty dragProperty = ListDragData.Editor.serializedObject.FindProperty(ListDragData.ListPath);
                SerializedProperty dropProperty = Editor.serializedObject.FindProperty(listPath);
                if (dragProperty == null || dropProperty == null)
                    return false;
                */
                //Debug.Log(dragProperty.type + " > " + dropProperty.type);
                
                //if (dragProperty.propertyType == dropProperty.propertyType)
                  //  return true;
                
            }
            return false;
        }

        private void Drop(string listPath, int index)
        {
            if (ListDragData != null)
            {
                SerializedProperty dropProperty = Editor.serializedObject.FindProperty(listPath);
                if (ListDragData.Editor == Editor && ListDragData.ListPath == listPath)
                {
                    //Debug.Log(ListDragData.Index + " > " + index);
                    dropProperty.MoveArrayElement(ListDragData.Index, index >= 0 ? (ListDragData.Index < index ? index - 1 : index) : dropProperty.arraySize - 1);
                    return;
                }
                        
            }
        }
        
        public override void Draw(PropertyData data)
        {
            var attr = data.Attributes.FirstOrDefault() as ListDrawerAttribute;
            if(attr == null)
                attr = new ListDrawerAttribute();
            
            Rect rect = EditorGUILayout.BeginVertical(Style.ListBackground);
            PerunEditor.DropRect dropRect = null;
            if (Event.current.type == EventType.Repaint)
            {
                dropRect = Editor.CreateDropRect();
                dropRect.Position = rect;
                string path = data.Property.propertyPath;
                dropRect.Validate = () =>{
                    return DropValidate(path);// DragAndDrop.objectReferences.FirstOrDefault(e => e is type);
                };
                dropRect.Action = i => Drop(path, i);
            }
            
            AnimBool animBool = Editor.GetAnimBool(data.Property.propertyPath, data.Property.isExpanded);
            // Header

            EditorGUILayout.BeginHorizontal(Style.Toolbar);
            
            data.Property.isExpanded = EditorGUILayout.Foldout(data.Property.isExpanded, new GUIContent(data.Property.displayName));
            animBool.target = data.Property.isExpanded;

            EditorGUILayout.LabelField(data.Property != null && data.Property.arraySize > 0 ? data.Property.arraySize + " items" : "empty", Style.ToolbarLabelRight, GUILayout.Width(100));
            
            if (attr.ShowAddButton && GUILayout.Button("", Style.ToolbarAddButton, GUILayout.Width(18)))
            {
                int index = data.Property.arraySize;
                data.Property.InsertArrayElementAtIndex(index);
                data.Property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndHorizontal();
            //

            EditorGUILayout.BeginVertical(data.Property.isExpanded ? Style.ListContent : Style.ListContentEmpty);
            
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                for (int i = 0; i < data.Property.arraySize; i++)
                {
                    SerializedProperty itemProperty = data.Property.GetArrayElementAtIndex(i);
                    /*
                    if(i > 0 && itemProperty.propertyType == SerializedPropertyType.Generic)
                        EditorGUILayout.Space();
                    */
                    Rect itemRect = EditorGUILayout.BeginHorizontal(Style.ListItem);
                    if(dropRect != null && data.Property.isExpanded)
                        dropRect.Childs.Add(new Rect(itemRect.x - 5, itemRect.y - 1, itemRect.width + 6, itemRect.height + 2));

                    if (attr.ShowDrag)
                    {
                        Rect itemDragRect = EditorGUILayout.GetControlRect(false, 10, Style.ListDragElement,GUILayout.Width(13));
                        itemDragRect = new Rect(itemDragRect.x, itemDragRect.y + itemRect.height / 2 - 12, 12, 16);
                        GUI.Box(itemDragRect, GUIContent.none, Style.ListDragElement);
                        itemDragRect = new Rect(itemDragRect.x, itemDragRect.y + 6 , 12, 16);
                        GUI.Box(itemDragRect, GUIContent.none, Style.ListDragElement);
                        
                        //GUI.Box(itemDragRect, GUIContent.none, Style.ListDragElement);
                        //EditorGUI.DropdownButton(itemDragRect, GUIContent.none, FocusType.Passive, Style.ListDragElement);

                        if (Event.current.type == EventType.Repaint)
                        {
                            PerunEditor.DragRect dragRect = Editor.CreateDragRect();
                            dragRect.Position = new Rect(itemDragRect.x, itemRect.y, itemDragRect.width, itemRect.height);
                            int index = i;
                            string path = data.Property.propertyPath;
                            dragRect.Action = () =>
                            {
                                DragAndDrop.PrepareStartDrag();
                                //DragAndDrop.objectReferences = new[] {property.serializedObject.targetObject};
                                ListDragData = new DragData(Editor, path, index);
                                //DragAndDrop.paths = new[] {itemProperty.propertyPath};
                                DragAndDrop.StartDrag(itemProperty.propertyPath);
                            };
                        }
                    }

                    EditorGUILayout.BeginVertical();
                    Editor.Property.Draw(new PropertyData(data, i));
                    EditorGUILayout.EndVertical();
                    //DrawItem(attr, itemProperty, i, type, attrList, parent);
                    //
                    if (attr.ShowRemoveButton)
                    {
                        Rect deleteRect = EditorGUILayout.GetControlRect(false, 16, GUILayout.Width(16));  
                        deleteRect = new Rect(deleteRect.x, deleteRect.y + itemRect.height / 2 - 10, 16, 16);
                       
                        if (GUI.Button(deleteRect, GUIContent.none, Style.ListDeleteItem))
                            data.Property.DeleteArrayElementAtIndex(i);
                        
                        /*
                        if (GUILayout.Button("", Style.ListDeleteItem, GUILayout.Width(16)))
                        {
                        }*/
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
