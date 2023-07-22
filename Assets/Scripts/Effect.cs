using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Effect : MonoBehaviour
{
    public enum EffectTrigger { ONGOING, TURNSTART, TURNEND, DAMAGERECIEVED }
    public EffectTrigger curEffectTrigger;

    public enum EffectType { OFFENSE, SUPPORT }
    public EffectType curEffectType;

    public enum EffectName { BLEED, HEALTHUP, HEALTHDOWN, POWERUP, POWERDOWN, RECOVER, SPEEDUP, SPEEDDOWN, EXHAUST, HASTE, SLEEP, 
    PARRY, TAUNT, MARK, SHADOWPARTNER }
    public EffectName curEffectName;

    public string effectName;
    public string effectDesc;
    [Tooltip("Percentage of x that will be used")]
    public float powerPercent;
    public float powerAmp;

    public bool isSelfCast;

    [SerializeField] private TextMeshProUGUI effectTurnCountText;
    public int turnCountRemaining;
    public int maxTurnCountRemaining;

    private Image effectIconImage;
    bool initialUse;
    private float tempAddedHealth;

    private float storedPowerAmp;

    private void Awake()
    {
        effectIconImage = GetComponent<Image>();
    }
    public void Setup(EffectData effect, UnitFunctionality targetUnit, int turnDuration = 1)
    {
        if (effect.curEffectTrigger == EffectData.EffectTrigger.TURNSTART)
            curEffectTrigger = EffectTrigger.TURNSTART;
        else if (effect.curEffectTrigger == EffectData.EffectTrigger.TURNEND)
            curEffectTrigger = EffectTrigger.TURNEND;
        else if (effect.curEffectTrigger == EffectData.EffectTrigger.DAMAGERECIEVED)
            curEffectTrigger = EffectTrigger.DAMAGERECIEVED;
        else if (effect.curEffectTrigger == EffectData.EffectTrigger.ONGOING)
            curEffectTrigger = EffectTrigger.ONGOING;

        if (effect.curEffectType == EffectData.EffectType.OFFENSE)
            curEffectType = EffectType.OFFENSE;
        else if (effect.curEffectType == EffectData.EffectType.SUPPORT)
            curEffectType = EffectType.SUPPORT;

        if (effect.curEffectName == EffectData.EffectName.BLEED)
            curEffectName = EffectName.BLEED;
        else if (effect.curEffectName == EffectData.EffectName.HEALTHUP)
            curEffectName = EffectName.HEALTHUP;
        else if (effect.curEffectName == EffectData.EffectName.HEALTHDOWN)
            curEffectName = EffectName.HEALTHDOWN;
        else if (effect.curEffectName == EffectData.EffectName.POWERUP)
            curEffectName = EffectName.POWERUP;
        else if (effect.curEffectName == EffectData.EffectName.POWERDOWN)
            curEffectName = EffectName.POWERDOWN;
        else if (effect.curEffectName == EffectData.EffectName.RECOVER)
            curEffectName = EffectName.RECOVER;
        else if (effect.curEffectName == EffectData.EffectName.SPEEDUP)
            curEffectName = EffectName.SPEEDUP;
        else if (effect.curEffectName == EffectData.EffectName.SPEEDDOWN)
            curEffectName = EffectName.SPEEDDOWN;
        else if (effect.curEffectName == EffectData.EffectName.EXHAUST)
            curEffectName = EffectName.EXHAUST;
        else if (effect.curEffectName == EffectData.EffectName.SLEEP)
            curEffectName = EffectName.SLEEP;
        else if (effect.curEffectName == EffectData.EffectName.TAUNT)
            curEffectName = EffectName.TAUNT;
        else if (effect.curEffectName == EffectData.EffectName.MARK)
            curEffectName = EffectName.MARK;
        else if (effect.curEffectName == EffectData.EffectName.SHADOWPARTNER)
            curEffectName = EffectName.SHADOWPARTNER;
        else if (effect.curEffectName == EffectData.EffectName.PARRY)
            curEffectName = EffectName.PARRY;

        effectName = effect.effectName; 
        effectDesc = effect.effectDesc;
        powerPercent = effect.powerPercent;
        powerAmp = effect.powerAmp;

        if (powerAmp != 0)
        {
            TriggerPowerEffect();
        }

        UpdateEffectIcon(effect);
        AddTurnCountText(turnDuration);

        //maxTurnCountRemaining = GameManager.Instance.GetActiveSkill().effectTurnLength;

        EffectApply(targetUnit);
    }

    void UpdateEffectIcon(EffectData effect)
    {
        effectIconImage.sprite = effect.effectIcon;
    }

    public void AddTurnCountText(int turns)
    {
        turnCountRemaining += turns;
        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public void RemoveEffect(UnitFunctionality unit)
    {
     //   if (GameManager.Instance.GetActiveUnitFunctionality().GetEffects().Count >= 1)
     //     GameManager.Instance.GetActiveUnitFunctionality().ResetEffects();

        EffectRemove(unit);
        Destroy(gameObject);
    }

    public void ReduceTurnCountText(UnitFunctionality unit)
    {
        if (turnCountRemaining == 1)
            RemoveEffect(unit);

        turnCountRemaining--;
        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public void EffectApply(UnitFunctionality targetUnit)
    {
        if (curEffectName == EffectName.HEALTHUP && !initialUse)
        {
            initialUse = true;

            tempAddedHealth = (powerPercent / 100f) * GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth();
            float newMaxHealth = GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth() + tempAddedHealth;
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitMaxHealth((int)newMaxHealth);
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)tempAddedHealth);
            GameManager.Instance.GetActiveUnitFunctionality().SpawnPowerUI(tempAddedHealth, false, false, this);
        }
        else if (curEffectName == EffectName.TAUNT)
            GameManager.Instance.GetActiveUnitFunctionality().ToggleTaunt(true);
        else if (curEffectName == EffectName.POWERUP)
        {
            storedPowerAmp = powerAmp;
            targetUnit.UpdateUnitPowerInc(storedPowerAmp);       
        }
        else if (curEffectName == EffectName.POWERDOWN)
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitPowerInc(-storedPowerAmp);
        else if (curEffectName == EffectName.PARRY)
            GameManager.Instance.GetActiveUnitFunctionality().isParrying = true;
        else if (curEffectName == EffectName.SPEEDUP)
            GameManager.Instance.SpeedAdjustTurnOrderFix();
        else if (curEffectName == EffectName.MARK)
        {
            // Toggle mark effect on the target
            targetUnit.curRecieveDamageAmp += (int)powerPercent;
        }
    }

    public void EffectRemove(UnitFunctionality unit)
    {
        if (curEffectName == EffectName.HEALTHUP)
        {
            float newMaxHealth = GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth() - tempAddedHealth;
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitMaxHealth((int)newMaxHealth);
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)-tempAddedHealth, true);
            GameManager.Instance.GetActiveUnitFunctionality().SpawnPowerUI(tempAddedHealth, false, true, this);
        }
        else if (curEffectName == EffectName.TAUNT)
            GameManager.Instance.GetActiveUnitFunctionality().ToggleTaunt(false);
        else if (curEffectName == EffectName.PARRY)
        {
            unit.isParrying = false;
            unit.attacked = false;
        }
        else if (curEffectName == EffectName.SPEEDUP)
        {
            // TODO
        }
        else if (curEffectName == EffectName.MARK)
        {
            // Toggle mark effect off the target
            unit.curRecieveDamageAmp -= (int)powerPercent;
        }
        else if (curEffectName == EffectName.POWERUP)
        {
            // Toggle mark effect off the target
            unit.UpdateUnitPowerInc(-storedPowerAmp);
        }
        else if (curEffectName == EffectName.POWERDOWN)
        {
            // Toggle mark effect off the target
            unit.UpdateUnitPowerInc(storedPowerAmp);
        }

        if (unit.GetEffects()[unit.GetEffects().IndexOf(this)])
        {
            unit.GetEffects().Remove(this);
        }
    }

    public void TriggerPowerEffect()
    {
        // Trigger effect alert
        // Do effect
        int unitMaxHealth = (int)GameManager.Instance.GetActiveUnitFunctionality().GetUnitMaxHealth();
        float tempPower = (powerPercent / 100f) * unitMaxHealth;
        int power = (int)tempPower;

        // Make bleed scale with recover buff // TODO

        if (curEffectName == EffectName.BLEED)
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth(-power, true);
        else if (curEffectName == EffectName.RECOVER)
            GameManager.Instance.GetActiveUnitFunctionality().UpdateUnitCurHealth(power);

        if (curEffectName == EffectName.BLEED || curEffectName == EffectName.RECOVER)
            GameManager.Instance.GetActiveUnitFunctionality().SpawnPowerUI(power, false, false, this);

        if (curEffectName ==  EffectName.PARRY)
        { }
    }
}

