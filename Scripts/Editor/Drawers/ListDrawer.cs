using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
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
            public object Object;
            public object List;
            public object ListParent;

            public DragData(Editor editor, string listPath, int index, object obj, object list, object listParent)
            {
                Editor = editor;
                ListPath = listPath;
                Index = index;
                Object = obj;
                List = list;
                ListParent = listParent;
            }
        }
        public static DragData ListDragData = null;
        
        private bool DropValidate(object listObject, string listPath)
        {
            //return true;
            if (ListDragData != null)
            {
                if (ListDragData.Editor == Editor && ListDragData.ListPath == listPath)
                    return true;

                Type listType = Utilities.GetElementType(listObject);
                Type listType2 = Utilities.GetElementType(ListDragData.Object); 
                return listType != null && listType2 == listType;
            }
            return false;
        }

        private void Drop(object listParent, object listObject, string listPath, int index)
        {
            if (ListDragData != null)
            {
                SerializedProperty dropProperty = Editor.serializedObject.FindProperty(listPath);
                if (ListDragData.Editor == Editor && ListDragData.ListPath == listPath)
                {
                    dropProperty.MoveArrayElement(ListDragData.Index, index >= 0 ? (ListDragData.Index < index ? index - 1 : index) : dropProperty.arraySize - 1);
                    return;
                }

                SerializedProperty property = Editor.serializedObject.FindProperty(listPath);
                SerializedProperty sourceList = ListDragData.Editor.serializedObject.FindProperty(ListDragData.ListPath);
                if (property != null && sourceList != null)
                {
                    Utilities.ListInsert(listParent, property.name, listObject, index, Utilities.GetValue(ListDragData.Object, ListDragData.Index));
                    Utilities.ListRemove(ListDragData.ListParent, sourceList.name, ListDragData.List, ListDragData.Index);
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
                object objParent = data.Parent.Value;
                object obj = data.Value;
                string path = data.Property.propertyPath;
                dropRect.Validate = () =>{
                    return DropValidate(obj, path);// DragAndDrop.objectReferences.FirstOrDefault(e => e is type);
                };
                dropRect.Action = i => Drop(objParent, obj, path, i);
            }
            
            AnimBool animBool = Editor.GetAnimBool(data.Property.propertyPath, data.Property.isExpanded);
            // Header

            EditorGUILayout.BeginHorizontal(Style.Toolbar);
            
            data.Property.isExpanded = EditorGUILayout.Foldout(data.Property.isExpanded, new GUIContent(data.Property.displayName));
            animBool.target = data.Property.isExpanded;

            EditorGUILayout.LabelField(data.Property != null && data.Property.arraySize > 0 ? data.Property.arraySize + " items" : "empty", Style.ToolbarLabelRight, GUILayout.Width(100));
            
            if (attr.ShowAddButton && GUILayout.Button("", Style.ToolbarAddButton, GUILayout.Width(18)))
            {
                data.AddNewItem();
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
                            object obj = data.Value;
                            dragRect.Action = () =>
                            {
                                DragAndDrop.PrepareStartDrag();
                                ListDragData = new DragData(Editor, path, index, obj, data.Value, data.Parent.Value);
                                DragAndDrop.SetGenericData("ListDragData", ListDragData);
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
