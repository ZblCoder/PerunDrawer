using System;
using System.Collections.Generic;
using PerunDrawer;
using UnityEngine;
using Random = System.Random;

public class TestBehaviour : MonoBehaviour
{
    [Flags] 
    public enum Types {   
        None = 0,
        A = 1 << 1,
        B = 1 << 2,
        C = 1 << 3,
        D = 1 << 4,
        AB = A | B,
        ALL = AB | C | D
    }
    
    [Serializable]
    public struct MyStruct
    {
        [EnumButtons]
        public Types Types1;
        
        public string strValue;
    }

    public MyStruct Struct;
    
    public Types EnumValue;

    [EnumButtons]
    public Types EnumValue1;

    [EnumButtons]
    public Types EnumValue2;
    
    
    public enum Types2 {   
        A,
        B,
        C
    }
    
    public Types2 EnumValue3;
    
    [EnumButtons, HideLabel]
    public Types2 EnumValue4;
    
    
    
    //[HideLabel]
    
    
    
    
    
    
    
/*
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









