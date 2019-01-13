using System;
using PerunDrawer;
using UnityEngine;

public class EnabledDemo : MonoBehaviour 
{
    [Serializable]
    public class Item
    {
        public string Name;
        public float Value;
    }
    
    public bool EnabledField = false;

    [Enabled("EnabledField")]
    public string Field = "Luke! I am your father!";
    
    public bool EnabledItem = false;
    
    [Enabled("EnabledItem")]
    public Item Object;
    
    public bool EnabledItemList = false;
    
    [Enabled("EnabledItemList")]
    public Item[] ItemList;
    
    public enum RadioButtonEnum {
        First,
        Second,
        Third
    }
    
    [Flags] 
    public enum Types {   
        None = 0,
        A = 1 << 1,
        B = 1 << 2,
        C = 1 << 3,
        D = 1 << 4,
        ALL = A | B | C | D
    }

    [Space]

    public RadioButtonEnum EnabledBuEnum = RadioButtonEnum.First;

    [Enabled("EnabledBuEnum", RadioButtonEnum.First)]
    public string FirstItem = "All this base is now belongs to Us!";

    [Enabled("EnabledBuEnum", RadioButtonEnum.First, IsNot = true)]
    public string NotFirstItem = "Spice must flow";

    [Enabled("EnabledBuEnum", RadioButtonEnum.Second)]
    public string SecondItem = "I'l be back...";

    [Enabled("EnabledBuEnum", RadioButtonEnum.Third)]
    public string ThirdItem = "To infinity! And beyond!";
    
    
    [Space]
    
    [EnumButtons]
    public Types EnabledType = Types.None;
    
    [Enabled("EnabledType", Types.None)]
    public string NoneItem = "None";
    
    [Enabled("EnabledType", Types.A)]
    public string AItem = "A";

    [Enabled("EnabledType", Types.B)]
    [Enabled("EnabledType", Types.A | Types.B, true)]
    public string BItem = "B";

    [Enabled("EnabledType", Types.C)]
    public string CItem = "C";

    [Enabled("EnabledType", Types.D)]
    public string DItem = "D";
    
    [Enabled("EnabledType", Types.A | Types.B)]
    public string ABItem = "AB";
    
    [Enabled("EnabledType", Types.ALL)]
    public string AllItem = "All";

}
