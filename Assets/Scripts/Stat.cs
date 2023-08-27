using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stat")]
public class Stat : ScriptableObject
{
    public Sprite statIcon;
    public string statName;
    public string statDesc;
    public int statMaxAmount;
}
