using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PerunDrawer
{
    public class GenericDrawer : BaseDrawer
    {
        public GenericDrawer(PerunEditor editor) : base(editor) {}
     
        private class OrderItem
        {
            public int Order = 0;
            public int Index = 0;
            public PropertyData Data;

            public string Name = "";
            public GroupAttribute GroupAttribute;
            public List<OrderItem> Childs;

            public OrderItem(string name, int index)
            {
                Index = index;
                Childs = new List<OrderItem>();
            }
            
            public OrderItem(GroupAttribute groupAttribute, int index)
            {
                var pathList = groupAttribute.Name.Split('/');
                Name = pathList[pathList.Length - 1];
                GroupAttribute = groupAttribute;
                Index = index;
                Childs = new List<OrderItem>();
            }
            
            public OrderItem(PropertyData data, int index)
            {
                Data = data;
                Index = index;
                var orderAttr = Data.Attributes.FirstOrDefault(e => e is OrderAttribute) as OrderAttribute;
                if (orderAttr != null)
                    Order = orderAttr.Order;
            }

            public OrderItem FindPath(OrderItem item, string path = "")
            {
                var parent = this;
                var pathList = path.Split('/').ToList();
                while (pathList.Count > 0)
                {
                    if(parent == null)
                        break;
                    if (!string.IsNullOrEmpty(pathList[0]))
                    {
                        var pathItem = parent.Childs.FirstOrDefault(e => e.Name.Equals(pathList[0], StringComparison.OrdinalIgnoreCase));
                        if (pathItem == null)
                        {
                            var newItem = pathList.Count > 1 ? new OrderItem(pathList[0], item.Index) : item;
                            parent.Childs.Add(newItem);
                            parent = newItem;
                        }
                        else
                            parent = pathItem;
                    }
                    pathList.RemoveAt(0);
                }
                return parent;
            }
            
            public void Add(OrderItem item, string path = "")
            {
                var parent = FindPath(item, path);
                
                if (item.Data == null)
                    return;
                if(parent != null)
                    parent.Childs.Add(item);
                else
                    Childs.Add(item);
                
            }
            
            public void Sort()
            {
                if (Childs == null)
                    return;
                Childs.Sort(_comparer);
                foreach (var item in Childs)
                    item.Sort();
            }

            public void Draw(Action<OrderItem> drawAction)
            {
                if (Childs != null)
                {
                    if (GroupAttribute != null)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField(Name, Style.BoldLabel);
                    }

                    foreach (var item in Childs)
                        item.Draw(drawAction);
                    
                    if (GroupAttribute != null)
                    {
                        EditorGUILayout.EndVertical();
                    }
                }
                else
                    drawAction.Invoke(this);
            }
            
            private static Comparer _comparer = new Comparer();
            public class Comparer : IComparer<OrderItem>
            {
                public int Compare(OrderItem a, OrderItem b)
                {
                    int compareDate = a.Order.CompareTo(b.Order);
                    return compareDate == 0 ? a.Index.CompareTo(b.Index) : compareDate;
                }
            }
        }
        
        public void DrawProperies(PropertyData data)
        {
            var buttons = Utilities.FindByAttribute<ButtonAttribute, MethodInfo>(data.Value);
            DrawMethodButtons(data, buttons, ButtonAttribute.AlignTypes.Top);
            
            Dictionary<OrderItem, string> itemList = new Dictionary<OrderItem, string>();
            OrderItem items = new OrderItem("", 0);
            Dictionary<string, OrderItem> attrList = new Dictionary<string, OrderItem>();
            
            var iterator = data.Property.Copy();
            int index = 0;
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                if (string.IsNullOrEmpty(data.Property.propertyPath) || iterator.propertyPath.IndexOf(data.Property.propertyPath, StringComparison.Ordinal) == 0)
                {
                    PropertyData itemData = new PropertyData(iterator.Copy(), data);
                    if ("m_Script" == iterator.propertyPath)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        Editor.Property.Draw(itemData);
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        string path = "";
                        foreach (var attribute in itemData.Attributes.Select(e => e as GroupAttribute))
                            if (attribute != null)
                            {
                                if(!attrList.ContainsKey(attribute.Name))
                                    attrList.Add(attribute.Name, new OrderItem(attribute, index));
                                path = path.Length < attribute.Name.Length ? attribute.Name : path;
                            }

                        itemList.Add(new OrderItem(itemData, index), path);
                        //var groupAttr = itemData.Attributes.FirstOrDefault(e => e is GroupAttribute) as GroupAttribute;
                        //items.Add(groupAttr != null ? groupAttr.Name, , new OrderItem(itemData, index));
                        //AddItem(items, new OrderItem(itemData, index));
                        index++;
                    }
                }

            foreach (var pair in attrList)
                items.Add(pair.Value, pair.Key);
  
            foreach (var pair in itemList)
                items.Add(pair.Key, pair.Value);
            
            items.Sort();
            items.Draw(e => Editor.Property.Draw(e.Data));
            
            DrawMethodButtons(data, buttons, ButtonAttribute.AlignTypes.Bottom);
        }

        private void DrawMethodButtons(PropertyData data, Dictionary<ButtonAttribute, MethodInfo> buttons, ButtonAttribute.AlignTypes alignType)
        {
            foreach (var buttonPair in buttons)
                if(buttonPair.Key.Align == alignType)
                    if (GUILayout.Button(string.IsNullOrEmpty(buttonPair.Key.Caption) ? buttonPair.Value.Name : buttonPair.Key.Caption))
                        buttonPair.Value.Invoke(data.Value, null);
        }
        
        public override void Draw(PropertyData data)
        {
            //data.Attributes.ForEach(a => EditorGUILayout.LabelField(a.GetType().FullName));
            StructDrawerAttribute attr = null;
            if (data.Parent != null && data.Parent.Type == PropertyData.Types.List)
                attr = data.Parent.Attributes.FirstOrDefault(e => e is StructDrawerAttribute) as StructDrawerAttribute;
            if(attr == null)
                attr = data.Attributes.FirstOrDefault(e => e is StructDrawerAttribute) as StructDrawerAttribute;
            if(attr == null)
                attr = new StructDrawerAttribute();
            
            switch (data.Parent != null ? attr.ItemType : StructDrawerAttribute.ItemTypes.None)
            {
                case StructDrawerAttribute.ItemTypes.FadeGroup:
                    //EditorGUILayout.GetControlRect(false, 10, GUILayout.Width(8));
                    EditorGUILayout.BeginVertical();
                    //data.Property.isExpanded = EditorGUILayout.Foldout(data.Property.isExpanded, new GUIContent(data.Property.displayName));
                    
                    if(EditorGUILayout.DropdownButton(new GUIContent(data.Property.displayName), FocusType.Passive, 
                        data.Property.isExpanded ? Style.FoldoutExpanded : Style.Foldout))
                        data.Property.isExpanded = !data.Property.isExpanded;

                    if (data.Property.isExpanded)
                    {
                        int lastIndent = EditorGUI.indentLevel;
                        EditorGUI.indentLevel = 1;
                        DrawProperies(data);
                        EditorGUI.indentLevel = lastIndent;
                    }
                    
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
