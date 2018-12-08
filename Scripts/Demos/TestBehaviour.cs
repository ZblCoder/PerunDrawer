using System;
using System.Collections.Generic;
using PerunDrawer;
using UnityEngine;

[PerunDrawer]
public class TestBehaviour : MonoBehaviour
{
    [Serializable]
    public struct DictionaryItem
    {
        [HideLabel]
        public string Key;
        [HideLabel]
        public GameObject Value;
    }
    
    [Serializable]
    public class Item
    {
        public int IntValue;
        public string StrValue;
        public Color ColorValue;
        
        [SerializeField]
        private AnimationCurve _curve;
    }
    
    public int Int;
    public string String;
    public Color Color;
    public Item Data;
    
    [ListDrawer(ItemType = ListDrawerAttribute.ItemTypes.Box)]
    public List<Item> ItemListBox;
    
    [ListDrawer(ItemType = ListDrawerAttribute.ItemTypes.None)]
    public List<Item> ItemListNone;
    
    [ListDrawer(ItemType = ListDrawerAttribute.ItemTypes.HorizontalGroup)]
    public List<DictionaryItem> Dictionary;
    
    public int[] IntArray;

    public void TestMethod()
    {
        Debug.Log("Работает");
    }
}
