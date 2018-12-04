using System;
using System.Collections;
using System.Collections.Generic;
using PerunDrawer;
using UnityEngine;

public class List : MonoBehaviour
{
    [Serializable]
    public class Item
    {
        public int IntValue;
        public string StrValue;
        public Color ColorValue;
        [SerializeField]
        private AnimationCurve _curve;
        
        public List<Item> ItemList;
    }
    
    public int Int;
    public string String;
    public Color Color;
    
    [ListDrawer]
    public List<Item> ItemList;
    
    public Item[] ItemArray;
    public List<int> IntList;
    public int[] IntArray;
    public Item Data;
    
}
