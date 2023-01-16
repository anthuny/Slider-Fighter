using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect")]
public class EffectData : ScriptableObject
{
    public enum EffectTrigger {ONGOING, TURNSTART, DAMAGERECIEVED}
    public EffectTrigger curEffectTrigger;

    public enum EffectDeplete { TURNSTART, TURNEND, DAMAGERECIEVED }
    public EffectDeplete curEffectDeplete;

    public string effectName;
    public string effectDesc;
    public Sprite effectIcon;
    public int effectTurnLength;
    public int effectOnTargetBonusPower;
    //public Text turnsRemainingText;
}
