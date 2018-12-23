using PerunDrawer;
using System;
using UnityEngine;

public sealed class EnumButtonsDemo : MonoBehaviour {
    public enum EnumType {
        None = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4
    }

    [Flags]
    public enum EnumFlagsType {
        None    = 0,
        A   = 1 << 1,
        B   = 1 << 2,
        C   = 1 << 3,
        D   = 1 << 4,
        AB  = A | B,
        ALL = AB | C | D
    }

    public EnumType DefaultEnumVisual;

    [EnumButtons]
    public EnumType ButtonsEnumVisual;

    [EnumButtons, HideLabel]
    public EnumType ButtonsEnumNoLabel;

    public EnumFlagsType DefaultFlagsVisual;

    [EnumButtons]
    public EnumFlagsType ButtonsFlagsVisual;

    [EnumButtons, HideLabel]
    public EnumFlagsType ButtonsFlagsNoLabel;
}