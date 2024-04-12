using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect")]
public class EffectData : ScriptableObject
{
    public enum EffectTrigger {ONGOING, TURNSTART, TURNEND, ANYENDTURN }
    public EffectTrigger curEffectTrigger;

    public enum EffectType { OFFENSE, SUPPORT }
    public EffectType curEffectType;

    public enum EffectBenefitType { BUFF, DEBUFF }
    public EffectBenefitType curEffectBenefitType;

    public enum EffectName
    {
        BLEED, POISON, HEALTHUP, HEALTHDOWN, POWERUP, POWERDOWN, HEALINGUP, HEALINGDOWN, RECOVER, SPEEDUP, SPEEDDOWN, EXHAUST, HASTE, SLEEP,
        PARRY, TAUNT, MARK, SHADOWPARTNER, DEFENSEUP, DEFENSEDOWN, REAPING, MIND_CONTROL, IMMUNITY
    }
    public EffectName curEffectName;

    public string effectName;
    public string effectDesc;
    public Sprite effectIcon;
    public float powerPercent;
    public float powerAmp;
    public Color titleTextColour;
}
