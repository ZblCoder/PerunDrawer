using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PerunDrawer;
using UnityEngine;
using Random = System.Random;

public class TestBehaviour : MonoBehaviour
{

    [Serializable]
    public class TestItem
    {
        public int IntValue;
        public string StrValue;
    }
    
    [Serializable]
    public class TestItemGroup
    {
        public int IntValue;
        public string StrValue;
        public TestItem[] Gruops;
        
        public List<TestItem> Gruops1;
    }

    public TestItemGroup[] arrayValue;
    
    /*
    [Serializable]
    public class Item
    {
        [Space]
        public int IntValue;
        public string StrValue;
        public TestObjectMyDrawer MyDrawer;
        
        public Item()
        {
            IntValue = new Random().Next(100);
        }
    }
    
    public int IntValue;
    [Space]
    public int IntValue2;
    [Space]
    public TestObjectMyDrawer ItemValuea;
    
    [Space]
    [Header("Header")]
    [Space]
    public int IntValue3;

    [Space] 
    [Header("Header")]
    [Space] 
    public TestObjectMyDrawer ItemValue;


    public bool Visible = true;

    [Visible("Visible")]
    public string VisibleText = "text";
    
    [Visible("Visible", true)]
    public TestObjectMyDrawer VisibleObj;
    
    [Visible("VisibleNotFount")]
    public TestObjectMyDrawer Visible3Obj;
    
    [Space] 
    [Space] 
    
    public int Visible2Int;
    
    [Visible("GetVisible")]
    public TestObjectMyDrawer Visible2Obj;

    private bool GetVisible()
    {
        return Visible2Int % 2 == 0;
    }
    */
    /*
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
    */
}









