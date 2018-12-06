using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace PerunDrawer
{
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = false)]
    [CanEditMultipleObjects]
    public class PerunEditor : Editor
    {
        public ListDrawer List;
        public PropertyDrawer Property;
        public GenericDrawer Generic;

        public List<Attribute> Attributes;
        private Dictionary<string, AnimBool> _foldoutStates = new Dictionary<string, AnimBool>();
        
        private void Awake()
        {
            List = new ListDrawer(this);
            Property = new PropertyDrawer(this);
            Generic = new GenericDrawer(this);
        }

        private void OnDestroy()
        {
            
        }
        
        public override void OnInspectorGUI()
        {
            Attributes = serializedObject.targetObject.GetType().GetCustomAttributes(false).Cast<Attribute>().ToList();
            if(Attributes.Exists(o => o is PerunDrawerAttribute))
            {
                EditorGUI.BeginChangeCheck();
                serializedObject.Update();
                SerializedProperty iterator = serializedObject.GetIterator();
                Generic.Draw(iterator, serializedObject.targetObject.GetType(), Attributes);
                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
            else
                base.OnInspectorGUI();
        }
        
        public AnimBool GetAnimBool(string path, bool value)
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
    }
}

