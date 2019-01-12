using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace PerunDrawer
{
    public class BaseAttributesDrawer : BaseDrawer
    {
        public BaseAttributesDrawer(PerunEditor editor) : base(editor) {}
        
        public override void Draw(PropertyData data)
        {
            foreach (var attribute in data.Attributes)
                if(attribute is SpaceAttribute)
                    DrawSpace(attribute as SpaceAttribute);
                else if(attribute is HeaderAttribute)
                    DrawHeader(attribute as HeaderAttribute);
        }

        private void DrawSpace(SpaceAttribute attribute)
        {
            GUILayoutUtility.GetRect(6f, attribute.height);
        }
        
        private void DrawHeader(HeaderAttribute attribute)
        {
            GUILayoutUtility.GetRect(6f, 6f);
            EditorGUILayout.LabelField(attribute.header, Style.BoldLabel);
        }
    }
}
