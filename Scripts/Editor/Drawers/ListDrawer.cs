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
        
        public override void Draw(SerializedProperty property, Type type, List<Attribute> attrList)
        {
            var parent = Utilities.GetParent(property);
            var attr = attrList.FirstOrDefault() as ListDrawerAttribute;
            if(attr == null)
                attr = new ListDrawerAttribute();
            
            Rect rect = EditorGUILayout.BeginVertical(Style.ListBackground);
            PerunEditor.DropRect dropRect = null;
            if (Event.current.type == EventType.Repaint)
            {
                dropRect = Editor.CreateDropRect();
                dropRect.Position = rect;
                dropRect.Object = Utilities.GetValue(parent, property.name);
                dropRect.Validate = () =>
                {
                    return true;// DragAndDrop.objectReferences.FirstOrDefault(e => e is type);
                };
            }
            
            AnimBool animBool = Editor.GetAnimBool(property.propertyPath, property.isExpanded);
            // Header

            EditorGUILayout.BeginHorizontal(Style.Toolbar);
            
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, new GUIContent(property.displayName));
            animBool.target = property.isExpanded;

            EditorGUILayout.LabelField(property != null && property.arraySize > 0 ? property.arraySize + " items" : "empty", Style.ToolbarLabelRight, GUILayout.Width(100));
            
            if (attr.ShowAddButton && GUILayout.Button("", Style.ToolbarAddButton, GUILayout.Width(18)))
            {
                int index = property.arraySize;
                property.InsertArrayElementAtIndex(index);
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndHorizontal();
            //

            EditorGUILayout.BeginVertical(property.isExpanded ? Style.ListContent : Style.ListContentEmpty);
            
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty itemProperty = property.GetArrayElementAtIndex(i);
                    /*
                    if(i > 0 && itemProperty.propertyType == SerializedPropertyType.Generic)
                        EditorGUILayout.Space();
                    */
                    Rect itemRect = EditorGUILayout.BeginHorizontal(Style.ListItem);
                    if(dropRect != null && property.isExpanded)
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
                            dragRect.Position = new Rect(itemDragRect.x, itemRect.y, itemDragRect.width,
                                itemRect.height);
                            dragRect.Action = () =>
                            {
                                DragAndDrop.PrepareStartDrag();
                                DragAndDrop.objectReferences = new[] {property.serializedObject.targetObject};
                                //DragAndDrop.paths = new[] {itemProperty.propertyPath};
                                DragAndDrop.StartDrag(itemProperty.propertyPath);
                            };
                        }
                    }

                    DrawItem(attr, itemProperty, i, type, attrList, parent);
                    //
                    if (attr.ShowRemoveButton)
                    {
                        Rect deleteRect = EditorGUILayout.GetControlRect(false, 16, GUILayout.Width(16));  
                        deleteRect = new Rect(deleteRect.x, deleteRect.y + itemRect.height / 2 - 10, 16, 16);
                       
                        if (GUI.Button(deleteRect, GUIContent.none, Style.ListDeleteItem))
                            property.DeleteArrayElementAtIndex(i);
                        
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
