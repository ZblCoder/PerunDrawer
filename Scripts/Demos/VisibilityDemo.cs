using System;
using PerunDrawer;
using UnityEngine;

public class VisibilityDemo : MonoBehaviour 
{
    public bool ShowField = false;

    [Visible("ShowField")]
    public string Field = "Luke! I am your father!";

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

    public RadioButtonEnum ShowBuEnum = RadioButtonEnum.First;

    [Visible("ShowBuEnum", RadioButtonEnum.First)]
    public string FirstItem = "All this base is now belongs to Us!";

    [Visible("ShowBuEnum", RadioButtonEnum.First, IsNot = true)]
    public string NotFirstItem = "Spice must flow";

    [Visible("ShowBuEnum", RadioButtonEnum.Second)]
    public string SecondItem = "I'l be back...";

    [Visible("ShowBuEnum", RadioButtonEnum.Third)]
    public string ThirdItem = "To infinity! And beyond!";
    
    
    [Space]
    
    [EnumButtons]
    public Types ShowType = Types.None;
    
    [Visible("ShowType", Types.None)]
    public string NoneItem = "None";
    
    [Visible("ShowType", Types.A)]
    public string AItem = "A";

    [Visible("ShowType", Types.B)]
    public string BItem = "B";

    [Visible("ShowType", Types.C)]
    public string CItem = "C";

    [Visible("ShowType", Types.D)]
    public string DItem = "D";
    
    [Visible("ShowType", Types.A | Types.B)]
    public string ABItem = "AB";
    
    [Visible("ShowType", Types.ALL)]
    public string AllItem = "All";

}
