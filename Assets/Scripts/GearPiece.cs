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

    public void UpdateGearPiece(string newName, string gearType, string gearRarity, Sprite gearIcon, int bonusHealth, int bonusDamage,
        int bonusHealing, int bonusDefense, int bonusSpeed)
    {
        this.gearName = newName;
        this.gearType = gearType;
        this.gearRarity = gearRarity;
        this.gearIcon = gearIcon;
        this.bonusHealth = bonusHealth;
        this.bonusDamage = bonusDamage;
        this.bonusHealing = bonusHealing;
        this.bonusDefense = bonusDefense;
        this.bonusSpeed = bonusSpeed;
    }
}
