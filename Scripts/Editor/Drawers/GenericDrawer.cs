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
     
        OrderItem.Comparer _comparer = new OrderItem.Comparer(); 
        
        private class OrderItem
        {
            public int Order = 0;
            public int Index = 0;
            public PropertyData Data;

            public OrderItem(PropertyData data, int index)
            {
                Data = data;
                Index = index;
                var orderAttr = Data.Attributes.FirstOrDefault(e => e is OrderAttribute) as OrderAttribute;
                if (orderAttr != null)
                    Order = orderAttr.Order;
            }
            
            public class Comparer : IComparer<OrderItem>
            {
                public int Compare(OrderItem a, OrderItem b)
                {
                    int compareDate = a.Order.CompareTo(b.Order);
                    if (compareDate == 0)
                    {
                        return a.Index.CompareTo(b.Index);
                    }
                    return compareDate;
                }
            }
        }
        
        public void DrawProperies(PropertyData data)
        {
            var buttons = Utilities.FindByAttribute<ButtonAttribute, MethodInfo>(data.Value);
            DrawMethodButtons(data, buttons, ButtonAttribute.AlignTypes.Top);
            
            List<OrderItem> items = new List<OrderItem>();
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
                        items.Add(new OrderItem(itemData, index));
                        index++;
                    }
                }
            
            items.Sort(_comparer);
            
            foreach (var item in items)
                Editor.Property.Draw(item.Data);
            
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
