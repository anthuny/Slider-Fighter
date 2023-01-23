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

    public string effectName;
    public string effectDesc;
    public float healthPerc;
    public float powerAmp;

    [SerializeField] private TextMeshProUGUI effectTurnCountText;
    public int turnCountRemaining;
    public int maxTurnCountRemaining;

    private Image effectIconImage;

    private void Awake()
    {
        effectIconImage = GetComponent<Image>();
    }
    public void Setup(EffectData effect)
    {
        if (effect.curEffectTrigger == EffectData.EffectTrigger.TURNSTART)
            curEffectTrigger = EffectTrigger.TURNSTART;
        if (effect.curEffectTrigger == EffectData.EffectTrigger.TURNEND)
            curEffectTrigger = EffectTrigger.TURNEND;
        if (effect.curEffectTrigger == EffectData.EffectTrigger.DAMAGERECIEVED)
            curEffectTrigger = EffectTrigger.DAMAGERECIEVED;
        if (effect.curEffectTrigger == EffectData.EffectTrigger.ONGOING)
            curEffectTrigger = EffectTrigger.ONGOING;

        if (effect.curEffectType == EffectData.EffectType.OFFENSE)
            curEffectType = EffectType.OFFENSE;
        if (effect.curEffectType == EffectData.EffectType.SUPPORT)
            curEffectType = EffectType.SUPPORT;

        effectName = effect.effectName; 
        effectDesc = effect.effectDesc;
        healthPerc = effect.healthPerc;
        powerAmp = effect.powerAmp;

        UpdateEffectIcon(effect);
        UpdateTurnCountText(GameManager.instance.GetActiveSkill().effectTurnLength);

        maxTurnCountRemaining = GameManager.instance.GetActiveSkill().effectTurnLength;
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

    public void ReduceTurnCountText()
    {
        if (turnCountRemaining == 1)
        {
            Destroy(gameObject);
        }

        turnCountRemaining--;
        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public void FillTurnCountText()
    {
        turnCountRemaining = maxTurnCountRemaining;
        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public void TriggerPowerEffect()
    {
        // Trigger effect alert

        // Do effect
        int unitMaxHealth = (int)GameManager.instance.GetActiveUnitFunctionality().GetUnitMaxHealth();
        float tempPower = (unitMaxHealth / healthPerc);
        int power = (int)tempPower;

        if (curEffectType == EffectType.OFFENSE)
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurHealth(-power);
        else
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitCurHealth(power);

        GameManager.instance.GetActiveUnitFunctionality().SpawnPowerUI(power, this);
    }

    public void TriggerPowerAdjustEffect()
    {
        //GameManager.instance.GetActiveUnitFunctionality().TriggerTextAlert(effectName, 1, true, "Inflict");

        if (curEffectType == EffectType.OFFENSE)
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitPowerInc(1 * powerAmp);
        else
            GameManager.instance.GetActiveUnitFunctionality().UpdateUnitPowerInc(1 * -powerAmp);

    }
}

