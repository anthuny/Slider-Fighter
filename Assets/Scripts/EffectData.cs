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

    public enum EffectName
    {
        BLEED, HEALTHUP, HEALTHDOWN, POWERUP, POWERDOWN, RECOVER, SPEEDUP, SPEEDDOWN, EXHAUST, HASTE, SLEEP,
        PARRY, TAUNT, MARK, SHADOWPARTNER
    }
    public EffectName curEffectName;

    public string effectName;
    public string effectDesc;
    public Sprite effectIcon;
    public float powerPercent;
    public float powerAmp;
}
