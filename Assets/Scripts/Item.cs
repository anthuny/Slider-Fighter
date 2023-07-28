using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int basePrice;

    public bool purchased;

    public bool healthItem = false;
    public int power;
    public int procChance;
    public RuntimeAnimatorController ac;
}
