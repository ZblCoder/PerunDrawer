using System;
using System.Collections.Generic;
using UnityEditor;

namespace PerunDrawer
{
    public abstract class BaseDrawer
    {
        private PerunEditor _editor;
        public PerunEditor Editor { get { return _editor; } }

        public BaseDrawer(PerunEditor editor)
        {
            _editor = editor;
        }
        
        public virtual void Draw(PropertyData data)
        {
            
        }
    }
}
