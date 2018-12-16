using System;
using System.Collections.Generic;
using PerunDrawer;
using UnityEngine;
using Random = System.Random;

public class TestBehaviour : MonoBehaviour
{
    [Serializable]
    public class Item
    {
        public int IntValue;
        public string StrValue;
        public TestObjectMyDrawer MyDrawer;
        
        public Item()
        {
            IntValue = new Random().Next(100);
        }
        
        [Button]
        public void TestMethodTop()
        {
            Debug.Log("Работает Top: " + IntValue + " - " + StrValue);
        }
        
        [Button("Bottom", ButtonAttribute.AlignTypes.Bottom)]
        public void TestMethodBottom()
        {
            Debug.Log("Работает Bottom: " + IntValue + " - " + StrValue);
        }
    }
    
    public Item Data;
    
    [StructDrawer(ItemType = StructDrawerAttribute.ItemTypes.Box)]
    public List<Item> ItemListBox;
    
    public List<Item> ItemListFadeGroup;
    
    [Button("Test")]
    public void MainMethod()
    {
        Debug.Log("Работает");
    }
}









