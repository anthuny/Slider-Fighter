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

    private void Awake()
    {
        effectIconImage = GetComponent<Image>();
    }
    public void Setup(EffectData effect)
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

        effectName = effect.effectName; 
        effectDesc = effect.effectDesc;
        powerPercent = effect.powerPercent;
        powerAmp = effect.powerAmp;

        if (powerAmp != 0)
        {
            TriggerPowerEffect();
        }

        UpdateEffectIcon(effect);
        UpdateTurnCountText(GameManager.instance.GetActiveSkill().effectTurnLength);

        maxTurnCountRemaining = GameManager.instance.GetActiveSkill().effectTurnLength;

        EffectApply();
    }

    void UpdateEffectIcon(EffectData effect)
    {
        effectIconImage.sprite = effect.effectIcon;
    }

    void UpdateTurnCountText(int turns)
    {
        turnCountRemaining = turns;
        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public void RemoveEffect()
    {
        if (GameManager.instance.GetActiveUnitFunctionality().GetEffects().Count >= 1)
            GameManager.instance.GetActiveUnitFunctionality().ResetEffects();

        EffectRemove();
        Destroy(gameObject);
    }

    public void ReduceTurnCountText()
    {
        if (turnCountRemaining == 1)
            RemoveEffect();

        turnCountRemaining--;
        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public void FillTurnCountText()
    {
        turnCountRemaining = maxTurnCountRemaining;
        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public void EffectApply()
    {
        if (curEffectName == EffectName.HEALTHUP && !initialUse)
        {
            initialUse = true;

            tempAddedHealth = (powerPercent / 100f) * GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxHealth();
            float newMaxHealth = GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxHealth() + tempAddedHealth;
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitMaxHealth((int)newMaxHealth);
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)tempAddedHealth);
            GameManager.instance.GetActiveUnitFunctionality().SpawnPowerUI(tempAddedHealth, this);
        }

        if (curEffectName == EffectName.TAUNT)
            GameManager.instance.GetActiveUnitFunctionality().ToggleTaunt(true);

        else if (curEffectName == EffectName.POWERUP)
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitPowerInc(1 * powerAmp);
        else if (curEffectName == EffectName.POWERDOWN)
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitPowerInc(1 * -powerAmp);
    }

    public void EffectRemove()
    {
        if (curEffectName == EffectName.HEALTHUP)
        {
            float newMaxHealth = GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxHealth() - tempAddedHealth;
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitMaxHealth((int)newMaxHealth);
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurHealth((int)-tempAddedHealth);
            GameManager.instance.GetActiveUnitFunctionality().SpawnPowerUI(tempAddedHealth, true, this);
        }
        else if (curEffectName == EffectName.TAUNT)
            GameManager.instance.GetActiveUnitFunctionality().ToggleTaunt(false);
    }

    public void TriggerPowerEffect()
    {
        // Trigger effect alert
        // Do effect
        int unitMaxHealth = (int)GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxHealth();
        float tempPower = (powerPercent / 100f) * unitMaxHealth;
        int power = (int)tempPower;

        if (curEffectName == EffectName.BLEED)
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurHealth(-power);
        else if (curEffectName == EffectName.RECOVER)
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurHealth(power);

        if (curEffectName == EffectName.BLEED || curEffectName == EffectName.RECOVER)
            GameManager.instance.GetActiveUnitFunctionality().SpawnPowerUI(power, this);
    }
}

