using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gear", menuName = "Gear")]
public class GearPiece : ScriptableObject
{
    public string gearName;
    public string gearType;
    public string gearRarity;
    public Sprite gearIcon;
    public int bonusHealth;
    public int bonusDamage;
    public int bonusHealing;
    public int bonusDefense;
    public int bonusSpeed;
}
