using PerunDrawer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ListDemo : MonoBehaviour {
    [Serializable]
    public sealed class Weapon {
        public string Name;
        public int Damage;

        public Weapon(string name, int damage) {
            Name = name;
            Damage = damage;
        }
    }

    public List<Weapon> Weapons = new List<Weapon>();

    [Button]
    public void Clear() {
        Weapons.Clear();
    }

    [Button("Add sword", Align = ButtonAttribute.AlignTypes.Bottom)]
    public void AddSword() {
        Weapons.Add(new Weapon("Sword", 10));
    }

    [Button("Add Pike", Align = ButtonAttribute.AlignTypes.Bottom)]
    public void AddPike() {
        Weapons.Add(new Weapon("Pike", 15));
    }
}
