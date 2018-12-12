using System;
using System.Collections.Generic;
using PerunDrawer;
using UnityEngine;
using Random = System.Random;

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

        public Item()
        {
            IntValue = new Random().Next(100);
        }
    }
    
    public int Int;
    public string String;
    public Color Color;
    public Item Data;
    
    [StructDrawer(ItemType = StructDrawerAttribute.ItemTypes.Box)]
    public List<Item> ItemListBox;
    
    [StructDrawer(ItemType = StructDrawerAttribute.ItemTypes.None)]
    public List<Item> ItemListNone;
    
    [StructDrawer(ItemType = StructDrawerAttribute.ItemTypes.HorizontalGroup)]
    public List<DictionaryItem> Dictionary;
    
    public int[] IntArray;

    public void TestMethod()
    {
        Debug.Log("Работает");
    }
}
