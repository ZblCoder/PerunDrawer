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
            List,
            SelfDrawer
        }
        private static List<Type> _typesSelfDrawer;
        
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
                    if (IsSelfDrawer())
                        _type = Types.SelfDrawer;
                    else if (Property.isArray && Property.propertyType != SerializedPropertyType.String)
                        _type = Types.List;
                    else if (Property.propertyType == SerializedPropertyType.Generic)
                        _type = Types.Generic;
                    else
                        _type = Types.Property;
                }
                return _type;
            }
        }
        
        private Type _valueType;
        public Type ValueType
        {
            get
            {
                if (_valueType == null)
                {
                    if (Value != null)
                        _valueType = Value.GetType();
                    else if (Parent != null)
                        _valueType = Utilities.GetType(Parent.ValueType, Property.name);
                }
                return _valueType;
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
            set
            {
                if (_value != value)
                {
                    SetValue(value);
                    _value = value;
                }
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
        
        private object GetValue()
        {
            if(Parent == null || Parent.Value == null)
                return null;

            try
            {
                if (Parent.Type == Types.List)
                {
                    Type listType = Parent.ValueType;
                    if (Parent.ValueType.IsArray)
                    {
                        int i = 0;
                        foreach (var item in Parent.Value as IEnumerable)
                        {
                            if(i == Index)
                                return item;
                            i++;
                        }
                    }
                    else
                        return ((IList)Parent.Value)[Index];
                    /*
                    if (enumerable != null)
                    {
                        var enm = enumerable.GetEnumerator();
                        while (Index-- >= 0)
                            enm.MoveNext();
                        return enm.Current;
                    }
                    else*/
                    return null;
                }
            }
            catch (Exception e)
            {
                // todo
            }
            
            var field = Parent.ValueType.GetField(Property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if(field != null)
                return field.GetValue(Parent.Value);
			
            var property = Parent.ValueType.GetProperty(Property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if(property != null)
                return property.GetValue(Parent.Value, null);
			
            return null;
        }

        private void SetValue(object value)
        {
            if(Parent == null || Parent.Value == null)
                return;
            
            var field = Parent.ValueType.GetField(Property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(Parent.Value, value);
                return;
            }
            
            var property = Parent.ValueType.GetProperty(Property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property != null)
            {
                property.SetValue(Parent.Value, value, null);
                return;
            }
        }

        private void AddAttributes(List<Attribute> attributes)
        {
            if (attributes != null)
                _attributes.AddRange(attributes);
            /* 
                foreach (var attribute in attributes)
                    if (!_attributes.Exists(e => e.GetType() == attribute.GetType()))
                        _attributes.Add(attribute);
            */
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
            Type type = Utilities.GetElementType(ValueType);
            InsertItem(Property.arraySize, type != typeof(string) ? Activator.CreateInstance(type) : "");
        }
        
        public void InsertItem(int index, object value)
        {
            if (ValueType.IsArray)
            {
                Type type = Utilities.GetElementType(ValueType);
                Type genericListType = typeof(List<>).MakeGenericType(type);
                var list = (IList)Activator.CreateInstance(genericListType);
                if(Value != null)
                    foreach (var e in (IEnumerable)Value)
                        list.Add(e);
                list.Insert(index, value);
                var array = Array.CreateInstance(type, Property.arraySize + 1);
                list.CopyTo(array, 0);
                Utilities.SetValue(Parent.Value, Property.name, array);
            }
            else
            {
                if (Value == null)
                {
                    var list = (IList)Activator.CreateInstance(ValueType);
                    list.Add(value);
                    Utilities.SetValue(Parent.Value, Property.name, list);
                }
                else
                    ((IList) Value).Insert(index, value);
            }
        }
        
        public bool IsSelfDrawer()
        {
            if (Value == null)
                return false;
            
            if (_typesSelfDrawer == null)
            {
                _typesSelfDrawer = new List<Type>();
                FieldInfo field = typeof(CustomPropertyDrawer).GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if(field != null)
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                        foreach (var t in assembly.GetTypes())
                            foreach (var a in t.GetCustomAttributes(typeof(CustomPropertyDrawer), false))
                                _typesSelfDrawer.Add(field.GetValue(a) as Type);
                            
            }
            return _typesSelfDrawer.Exists(e => e == ValueType);
        }
    }
}