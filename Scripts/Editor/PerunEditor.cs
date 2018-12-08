using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace PerunDrawer
{
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = false)]
    [CanEditMultipleObjects]
    public class PerunEditor : Editor
    {
        public class DragRect
        {
            [HideLabel]
            public Rect Position;
            [HideLabel]
            public Action Action;
        }
    
        public class DropRect
        {
            public Rect Position;
            public object Object;
            public List<Rect> Childs = new List<Rect>();
            public Func<bool> Validate;
        }
        
        public ListDrawer List;
        public PropertyDrawer Property;
        public GenericDrawer Generic;

        public List<Attribute> Attributes;
        private Dictionary<string, AnimBool> _foldoutStates = new Dictionary<string, AnimBool>();
        
        private List<DragRect> _dragRects = new List<DragRect>();
        private List<DropRect> _dropRects = new List<DropRect>();
        
        private void OnEnable()
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
            if (Event.current.type == EventType.Repaint)
            {
                _dragRects.Clear();
                _dropRects.Clear();
            }
            
            Attributes = serializedObject.targetObject.GetType().GetCustomAttributes(false).Cast<Attribute>().ToList();
            if(Attributes.Exists(o => o is PerunDrawerAttribute))
            {
                EditorGUI.BeginChangeCheck();
                serializedObject.Update();
                SerializedProperty iterator = serializedObject.GetIterator();
                Generic.Draw(iterator, serializedObject.targetObject.GetType(), Attributes);
                DragUpdate();
                DropUpdate();
                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
                
                if(GUI.changed)
                    Repaint();
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

        public DragRect CreateDragRect()
        {
            DragRect dragRect = new DragRect();
            _dragRects.Add(dragRect);
            return dragRect;
        }
        
        public DropRect CreateDropRect()
        {
            DropRect dropRect = new DropRect();
            _dropRects.Add(dropRect);
            return dropRect;
        }

        private void DragUpdate()
        {
            if (_dragRects.Count == 0)
                return;
            
            //_dragRects.ForEach(x => GUI.Box(x.Position, ""));
            
            Event e = Event.current;
            if (e.type != EventType.MouseDrag)
                return;
            
            DragRect dragRect = _dragRects.FirstOrDefault(x => x.Position.Contains(e.mousePosition));
            if (dragRect != null && dragRect.Action != null)
                dragRect.Action();
        }

        private Rect _dropLine = Rect.zero;
        private bool _isDrag = false;
        
        private void DropUpdate()
        {
            if (_dropRects.Count == 0)
                return;
            
            //_dropRects.ForEach(x => GUI.Box(x.Position, ""));
            //_dropRects.ForEach(x => x.Childs.ForEach(y => GUI.Box(y, "")));
            
            Event e = Event.current;
            switch (e.type) {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    _isDrag = true;
                    Rect dropLine = Rect.zero;
                    
                    for (int i = _dropRects.Count - 1; i >= 0; i--)
                    {
                        DropRect dropRect = _dropRects[i];
                        if (!dropRect.Position.Contains(e.mousePosition))
                            continue;
                        
                        if (dropRect.Validate == null || dropRect.Validate())
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                            
                            int index = -1;
                            for (int l = 0; l < dropRect.Childs.Count; l++)
                                if (dropRect.Childs[l].Contains(e.mousePosition))
                                {
                                    if (e.mousePosition.y > dropRect.Childs[l].center.y)
                                        index = l + 1 < dropRect.Childs.Count ? l + 1 : -1;
                                    else
                                        index = l;
                                    break;
                                }

                            if (index >= 0)
                            {
                                dropLine = dropRect.Childs[index];
                                dropLine.position += new Vector2(4, -1);
                            }
                            else
                            {
                                dropLine = dropRect.Position;
                                dropLine.position += new Vector2(4, dropLine.height - 4);
                            }

                            dropLine.width -= 8;
                            dropLine.height = 2;
                            
                            if (e.type == EventType.DragPerform)
                            {
                                DragAndDrop.AcceptDrag();
                                Debug.Log("Drag");
                                _isDrag = false;
                                _dropLine = Rect.zero;
                            }
                        }
                        break;
                    }

                    if (_dropLine != dropLine)
                    {
                        _dropLine = dropLine;
                        GUI.changed = true;
                    }

                    break;
                case EventType.DragExited:
                    _isDrag = false;
                    _dropLine = Rect.zero;
                    break;
            }
            
            if(_isDrag)
                GUI.Box(_dropLine, "", Style.ListDropLine);
        }
    }
}

