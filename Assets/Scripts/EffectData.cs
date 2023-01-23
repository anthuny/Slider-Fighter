using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect")]
public class EffectData : ScriptableObject
{
    public enum EffectTrigger {ONGOING, TURNSTART, TURNEND, DAMAGERECIEVED }
    public EffectTrigger curEffectTrigger;

    public enum EffectType { OFFENSE, SUPPORT }
    public EffectType curEffectType;

    public string effectName;
    public string effectDesc;
    public Sprite effectIcon;
    public float healthPerc;
    public float powerAmp;
    //public Text turnsRemainingText;
}
