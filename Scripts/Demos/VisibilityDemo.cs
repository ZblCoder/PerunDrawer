using PerunDrawer;
using UnityEngine;

public class VisibilityDemo : MonoBehaviour {

    public bool ShowField = false;

    [Visible("ShowField")]
    public string Field = "Luke! I am your father!";

    public enum RadioButtonEnum {
        First,
        Second,
        Third
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
}
