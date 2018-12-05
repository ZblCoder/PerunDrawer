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
        /*
        private Dictionary<string, AnimBool> _foldoutStates = new Dictionary<string, AnimBool>();
        private AnimBool GetAnimBool(string path, bool value)
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
        */
        public override void OnInspectorGUI()
        {
            var attr = serializedObject.targetObject.GetType().GetCustomAttributes(false);
            if(Array.Exists(attr, o => o is PerunDrawerAttribute))
            {
                EditorGUI.BeginChangeCheck();
                serializedObject.Update();
                SerializedProperty iterator = serializedObject.GetIterator();
                GenericDrawer.Draw(iterator, serializedObject.targetObject.GetType(), attr.Cast<Attribute>().ToList());
                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
            else
                base.OnInspectorGUI();
        }
    }
}

