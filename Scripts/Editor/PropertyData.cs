using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PerunDrawer
{
    public class PropertyData
    {
        public enum Types
        {
            None,
            Property,
            Generic,
            List
        }
        
        public SerializedProperty Property { get; private set; }
        public PropertyData Parent { get; private set; }
        public int Index { get; private set; }
        public List<PropertyData> Childs { get; private set; }
        
        private Types _type = Types.None;
        public Types Type
        {
            get
            {
                if (_type == Types.None)
                {
                    if (Property.isArray && Property.propertyType != SerializedPropertyType.String)
                        _type = Types.List;
                    else if (Property.propertyType == SerializedPropertyType.Generic)
                        _type = Types.Generic;
                    else
                        _type = Types.Property;
                }
                return _type;
            }
        }
        
        private object _value;
        public object Value
        {
            get
            {
                if (_value == null && Parent != null)
                    _value = GetValue();
                return _value;
            }
        }
        
        private List<Attribute> _attributes;
        public List<Attribute> Attributes
        {
            get
            {
                if (_attributes == null)
                    UpdateAttributes();
                return _attributes;
            }
        }

        private void Init(PropertyData parent, SerializedProperty property)
        {
            Childs = new List<PropertyData>();
            Parent = parent;
            Property = property;
        }
        
        public PropertyData(SerializedProperty property, PropertyData parent)
        {
            Init(parent, property);
        }
        
        public PropertyData(PropertyData parent, int index)
        {
            Index = index;
            Init(parent, parent.Property.GetArrayElementAtIndex(index));
        }
        
        public PropertyData(SerializedObject serializedObject)
        {
            Init(null, serializedObject.GetIterator());
            _value = serializedObject.targetObject;
        }
        
        public object GetValue()
        {
            if(Parent == null || Parent.Value == null)
                return null;

            try
            {
                if (Parent.Property.isArray && Parent.Property.propertyType != SerializedPropertyType.String)
                {
                    var enumerable = Parent.Value as IEnumerable;
                    if (enumerable != null)
                    {
                        var enm = enumerable.GetEnumerator();
                        while (Index-- >= 0)
                            enm.MoveNext();
                        return enm.Current;
                    }
                    else
                        return null;
                }
            }
            catch (Exception e)
            {
                // todo
            }
            
            var type = Parent.Value.GetType();
            var field = type.GetField(Property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if(field != null)
                return field.GetValue(Parent.Value);
			
            var property = type.GetProperty(Property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if(property != null)
                return property.GetValue(Parent.Value, null);
			
            return null;
        }

        private void AddAttributes(List<Attribute> attributes)
        {
            if (attributes != null)
                foreach (var attribute in attributes)
                    if (!_attributes.Exists(e => e.GetType() == attribute.GetType()))
                        _attributes.Add(attribute);
        }

        private void UpdateAttributes()
        {
            if (Parent != null)
            {
                _attributes = new List<Attribute>();
                if (Parent.Type == Types.List && Type == Types.Generic)
                    AddAttributes(Parent.Attributes);
                
                AddAttributes(Utilities.GetAttributes(Parent.Value, Property.name));
                AddAttributes(Utilities.GetTypeAttributes(Value));
                //if (Parent.Type == Types.List)
                //    AddAttributes(Utilities.GetElementTypeAttributes(Parent.Value));
            }
            else
                _attributes = Utilities.GetTypeAttributes(Value);

            if (_attributes == null)
                _attributes = new List<Attribute>();
        }
        
        public T GetValue<T>(string name, T defaultValue)
        {
            return Utilities.GetValue(Value, name, defaultValue);
        }

        public bool GetValue<T>(string name, out T value)
        {
            return Utilities.GetValue(Value, name, out value);
        }

        public void AddNewItem()
        {
            Type type = Utilities.GetElementType(Value);
            InsertItem(Property.arraySize, Activator.CreateInstance(type));
        }
        
        public void InsertItem(int index, object value)
        {
            Type listType = Value.GetType();
            if (listType.IsArray)
            {
                Type type = Utilities.GetElementType(Value);
                Type genericListType = typeof(List<>).MakeGenericType(type);
                var list = (IList)Activator.CreateInstance(genericListType);
                foreach (var e in (IEnumerable)Value)
                    list.Add(e);
                list.Insert(index, value);
                var array = Array.CreateInstance(type, Property.arraySize + 1);
                list.CopyTo(array, 0);
                Utilities.SetValue(Parent.Value, Property.name, array);
            }
            else
                ((IList) Value).Insert(index, value);
        }
    }
}