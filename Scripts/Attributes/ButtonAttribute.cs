﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerunDrawer
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ButtonAttribute : Attribute
    {
        public string Caption = "";

        public enum AlignTypes
        {
            Top,
            Bottom
        }

        public AlignTypes Align;

        public ButtonAttribute(AlignTypes align = AlignTypes.Bottom)
        {
            Align = align;
        }
        
        public ButtonAttribute(string caption, AlignTypes align = AlignTypes.Bottom)
        {
            Caption = caption;
            Align = align;
        }
    }
}
