using UnityEngine;

namespace PerunDrawer
{
    public class Style
    {
        private static GUIStyle _styleNode;
        public static GUIStyle StyleNode
        {
            get
            {
                if (_styleNode == null)
                    _styleNode = new GUIStyle() {padding = new RectOffset(16, 16, 0, 4)};
                return _styleNode;
            }
        }

        private static GUIStyle _background;
        public static GUIStyle Background
        {
            get
            {
                if (_background == null)
                {
                    _background = new GUIStyle(GUI.skin.GetStyle("ChannelStripBg"));
                    _background.padding = new RectOffset(8, 8, 4, 8);
                    _background.overflow = new RectOffset(2, 2, 0, 4);
                    _background.stretchHeight = false;
                }

                return _background;
            }
        }

        private static GUIStyle _backgroundFocus;
        public static GUIStyle BackgroundFocus
        {
            get
            {
                if (_backgroundFocus == null)
                {
                    _backgroundFocus = new GUIStyle(Background);
                    _backgroundFocus.normal.background = _backgroundFocus.onNormal.background;
                }

                return _backgroundFocus;
            }
        }

        private static GUIStyle _caption;
        public static GUIStyle Caption
        {
            get
            {
                if (_caption == null)
                {
                    _caption = new GUIStyle(GUI.skin.GetStyle("BoldLabel"));
                    _caption.alignment = TextAnchor.MiddleCenter;
                }

                return _caption;
            }
        }
        
        private static GUIStyle _fideGroup;
        public static GUIStyle FideGroup
        {
            get
            {
                if (_fideGroup == null)
                {
                    _fideGroup = new GUIStyle();
                    _fideGroup.margin = new RectOffset(12, 0, 0, 0);
                }

                return _fideGroup;
            }
        }
    
        private static GUIStyle _output;
        public static GUIStyle Output
        {
            get
            {
                if (_output == null)
                {
                    //_point = new GUIStyle(GUI.skin.GetStyle("sv_label_1"));
                    _output = new GUIStyle(GUI.skin.GetStyle("flow shader in 0"));
                }
                return _output;
            }
        } 
    
        private static GUIStyle _input;
        public static GUIStyle Input
        {
            get
            {
                if (_input == null)
                {
                    _input = new GUIStyle(GUI.skin.GetStyle("flow shader out 0"));
                }
                return _input;
            }
        }
    
        private static GUIStyle _listBackground;
        public static GUIStyle ListBackground
        {
            get
            {
                if (_listBackground == null)
                {
                    _listBackground = new GUIStyle(GUI.skin.GetStyle("GroupBox"));
                    _listBackground.margin = new RectOffset(0, 0, 0, 3);
                    _listBackground.padding = new RectOffset(1, 1, 1, 1);
                }

                return _listBackground;
            }
        }
    
        private static GUIStyle _listContent;
        public static GUIStyle ListContent
        {
            get
            {
                if (_listContent == null)
                {
                    _listContent = new GUIStyle();
                    _listContent.padding = new RectOffset(0, 0, 0, 0);
                }
                return _listContent;
            }
        }
        
        private static GUIStyle _listContentEmpty;
        public static GUIStyle ListContentEmpty
        {
            get
            {
                if (_listContentEmpty == null)
                {
                    _listContentEmpty = new GUIStyle();
                    _listContentEmpty.padding = new RectOffset(0, 0, 1, 1);
                }
                return _listContentEmpty;
            }
        }
    
        private static GUIStyle _listItem;
        public static GUIStyle ListItem
        {
            get
            {
                if (_listItem == null)
                {
                    _listItem = new GUIStyle();
                    _listItem.margin = new RectOffset(0, 0, 0, 0);
                    _listItem.padding = new RectOffset(0, 0, 1, 2);
                    //_listItem.margin = new RectOffset(0, 0, 0, 1);
                }
                return _listItem;
            }
        }
        
        private static GUIStyle _listItem2;
        public static GUIStyle ListItem2
        {
            get
            {
                if (_listItem2 == null)
                {
                    _listItem2 = new GUIStyle(ListItem);
                    _listItem2.normal.background = GUI.skin.GetStyle("CN EntryBackEven").normal.background;
                }
                return _listItem2;
            }
        }  
        
        //(GUIStyle)"Tag MenuItem"
        
        private static GUIStyle _listItemBox;
        public static GUIStyle ListItemBox
        {
            get
            {
                if (_listItemBox == null)
                {
                    _listItemBox = new GUIStyle(GUI.skin.GetStyle("GroupBox"));
                    _listItemBox.margin = new RectOffset(2, 2, 0, 0);
                    _listItemBox.padding = new RectOffset(3, 3, 3, 3);
                }
                return _listItemBox;
            }
        }

        private static GUIStyle _listDragElement;
        public static GUIStyle ListDragElement
        {
            get
            {
                if (_listDragElement == null)
                {
                    _listDragElement = new GUIStyle(GUI.skin.GetStyle("RL DragHandle"));
                    _listDragElement.margin = new RectOffset(4, 0, 7, 0);
                }
                return _listDragElement;
            }
        }
        
        private static GUIStyle _listDragItem;
        public static GUIStyle ListDragItem
        {
            get
            {
                if (_listDragItem == null)
                {
                    _listDragItem = new GUIStyle(GUI.skin.GetStyle("SelectionRect"));
                    _listDragItem.margin = ListItem.margin;
                    _listDragItem.padding = ListItem.padding;
                }
                return _listDragItem;
            }
        }
        
        private static GUIStyle _listDropLine;
        public static GUIStyle ListDropLine
        {
            get
            {
                if (_listDropLine == null)
                {
                    _listDropLine = new GUIStyle(GUI.skin.GetStyle("InsertionMarker"));
                }
                return _listDropLine;
            }
        }
        
        private static GUIStyle _listDeleteItem;
        public static GUIStyle ListDeleteItem
        {
            get
            {
                if (_listDeleteItem == null)
                {
                    _listDeleteItem = new GUIStyle(GUI.skin.GetStyle("OL Minus"));
                    _listDeleteItem.margin = new RectOffset(0, 4, 2, 0);
                    _listDeleteItem.alignment = TextAnchor.MiddleCenter;
                }
                return _listDeleteItem;
            }
        }
    
        private static GUIStyle _toolbar;
        public static GUIStyle Toolbar
        {
            get
            {
                if (_toolbar == null)
                {
                    _toolbar = new GUIStyle(GUI.skin.GetStyle("Toolbar"));
                    _toolbar.padding = new RectOffset(0, 0, 0, 0);
                    _toolbar.margin = new RectOffset();
                }
                return _toolbar;
            }
        }
    
        private static GUIStyle _foldout;
        public static GUIStyle Foldout
        {
            get
            {
                if (_foldout == null)
                {
                    _foldout = new GUIStyle(GUI.skin.GetStyle("Foldout"));
                    _foldout.fixedWidth = 0;
                    _foldout.padding = new RectOffset(14, 0, 0, 0);
                }
                return _foldout;
            }
        }
        
        private static GUIStyle _foldoutExpanded;
        public static GUIStyle FoldoutExpanded
        {
            get
            {
                if (_foldoutExpanded == null)
                {
                    _foldoutExpanded = new GUIStyle(Foldout);
                    _foldoutExpanded.normal.background = _foldoutExpanded.onNormal.background;
                }
                return _foldoutExpanded;
            }
        }
        
        private static GUIStyle _toolbarLabel;
        public static GUIStyle ToolbarLabel
        {
            get
            {
                if (_toolbarLabel == null)
                {
                    _toolbarLabel = new GUIStyle(GUI.skin.GetStyle("Toolbar"));
                    _toolbarLabel.normal.background = null;
                    _toolbarLabel.padding = new RectOffset(0, 0, 0, 0);
                    _toolbarLabel.alignment = TextAnchor.MiddleLeft;
                }
                return _toolbarLabel;
            }
        }
        
        private static GUIStyle _toolbarLabelRight;
        public static GUIStyle ToolbarLabelRight
        {
            get
            {
                if (_toolbarLabelRight == null)
                {
                    _toolbarLabelRight = new GUIStyle(GUI.skin.GetStyle("Toolbar"));
                    _toolbarLabelRight.normal.background = null;
                    _toolbarLabelRight.padding = new RectOffset(0, 5, 2, 0);
                    _toolbarLabelRight.alignment = TextAnchor.UpperRight;
                    
                }
                return _toolbarLabelRight;
            }
        }
    
        private static GUIStyle _toolbarButton;
        public static GUIStyle ToolbarButton
        {
            get
            {
                if (_toolbarButton == null)
                {
                    _toolbarButton = new GUIStyle(GUI.skin.GetStyle("toolbarbutton"));
                    _toolbarButton.padding = new RectOffset(0, 0, 0, 0);
                    _toolbarButton.alignment = TextAnchor.MiddleCenter;
                }
                return _toolbarButton;
            }
        }
    
        private static GUIStyle _toolbarAddButton;
        public static GUIStyle ToolbarAddButton
        {
            get
            {
                if (_toolbarAddButton == null)
                {
                    _toolbarAddButton = new GUIStyle(GUI.skin.GetStyle("OL Plus"));
                    _toolbarAddButton.imagePosition = ImagePosition.ImageOnly;
                    _toolbarAddButton.margin = new RectOffset(0, 2, 1, 0);
                    /*
                GUIStyle plus = GUI.skin.GetStyle("OL Plus");
                _toolbarAddButton.normal.background = plus.normal.background;
                _toolbarAddButton.active.background = plus.active.background;*/

                }
                return _toolbarAddButton;
            }
        }
    
    }
}
