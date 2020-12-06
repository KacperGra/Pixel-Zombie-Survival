using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopItem
{
    AmmoBox,
    Rifle,
    Shotgun
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name;
    public Sprite image;
    public int cost;
    public ShopItem index;
}
