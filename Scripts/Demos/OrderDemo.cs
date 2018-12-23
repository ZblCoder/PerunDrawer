using PerunDrawer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDemo : MonoBehaviour {
    [Order(3)]
    public int FirstInCode = 1;

    [Order(2)]
    public int SecondInCode = 2;

    [Order(1)]
    public int ThirdInCode = 3;

    [Order(0)]
    public int FourthInCode = 4;

    [Order(-1)]
    public int FifthInCode = 5;
}
