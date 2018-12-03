using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour
{
    [Serializable]
    public class Item
    {
        public int IntValue;
        public string StrValue;
        public Color ColorValue;
        public AnimationCurve Curve;
    }
    
    public int Int;
    public string String;
    public Color Color;
    public List<Item> ItemList;
    public Item[] ItemArray;
    public List<int> IntList;
    public int[] IntArray;
    public Item Data;
}
