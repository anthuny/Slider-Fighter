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

    int power;

    public enum EffectName 
    { 
        BLEED, POISON, HEALTHUP, HEALTHDOWN, POWERUP, POWERDOWN, HEALINGUP, HEALINGDOWN, RECOVER, SPEEDUP, SPEEDDOWN, EXHAUST, HASTE, SLEEP, 
        PARRY, TAUNT, MARK, SHADOWPARTNER, DEFENSEUP, DEFENSEDOWN 
    }
    public EffectName curEffectName;

    public string effectName;
    public string effectDesc;
    [Tooltip("Percentage of x that will be used")]
    public float powerPercent;
    public float powerAmp;

    public bool isSelfCast;

    [SerializeField] private TextMeshProUGUI effectTurnCountText;
    public int turnCountRemaining;

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
        else if (effect.curEffectName == EffectData.EffectName.POISON)
            curEffectName = EffectName.POISON;
        else if (effect.curEffectName == EffectData.EffectName.HEALTHUP)
            curEffectName = EffectName.HEALTHUP;
        else if (effect.curEffectName == EffectData.EffectName.HEALTHDOWN)
            curEffectName = EffectName.HEALTHDOWN;
        else if (effect.curEffectName == EffectData.EffectName.POWERUP)
            curEffectName = EffectName.POWERUP;
        else if (effect.curEffectName == EffectData.EffectName.POWERDOWN)
            curEffectName = EffectName.POWERDOWN;
        else if (effect.curEffectName == EffectData.EffectName.HEALINGUP)
            curEffectName = EffectName.HEALINGUP;
        else if (effect.curEffectName == EffectData.EffectName.HEALINGDOWN)
            curEffectName = EffectName.HEALINGDOWN;
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
        else if (effect.curEffectName == EffectData.EffectName.DEFENSEUP)
            curEffectName = EffectName.DEFENSEUP;
        else if (effect.curEffectName == EffectData.EffectName.DEFENSEDOWN)
            curEffectName = EffectName.DEFENSEDOWN;

        effectName = effect.effectName; 
        effectDesc = effect.effectDesc;
        powerPercent = effect.powerPercent;
        powerAmp = effect.powerAmp;

        if (powerPercent != 0)
        {
            //TriggerPowerEffect();
        }

        UpdateEffectIcon(effect);
        AddTurnCountText(turnDuration);

        transform.GetComponentInParent<CanvasGroup>().alpha = 1;

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

        // Ensure there is a cap
        int max = EffectManager.instance.GetMaxEffectTurnsRemaining();
        if (turnCountRemaining >= max)
            turnCountRemaining = max;

        effectTurnCountText.text = turnCountRemaining.ToString();
    }

    public int GetTurnCountRemaining()
    {
        return turnCountRemaining;
    }

    public void RemoveEffect(UnitFunctionality unit)
    {


        EffectRemove(unit);
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
        if (curEffectName == EffectName.HEALTHUP)
        {
            tempAddedHealth = ((powerPercent / 100f) * targetUnit.GetUnitMaxHealth());

            float newCurHealth = (int)tempAddedHealth;
            targetUnit.UpdateUnitMaxHealth((int)newCurHealth);
            targetUnit.UpdateUnitCurHealth((int)newCurHealth);
            targetUnit.StartCoroutine(targetUnit.SpawnPowerUI(newCurHealth, false, false, this));
        }
        else if (curEffectName == EffectName.TAUNT)
            targetUnit.ToggleTaunt(true);
        else if (curEffectName == EffectName.POISON)
        {
            // healing recieved is halved while on
            targetUnit.ToggleUnitPoisoned(true);
            targetUnit.UpdateUnitHealingRecieved(.5f);

        }
        else if (curEffectName == EffectName.POWERUP)
        {
            //storedPowerAmp = powerAmp;
            //targetUnit.UpdatePowerIncPerLv((int)storedPowerAmp);       
            targetUnit.UpdateUnitDamageHits(2, true);
        }
        else if (curEffectName == EffectName.POWERDOWN)
            targetUnit.UpdateUnitDamageHits(2, false);
        else if (curEffectName == EffectName.HEALINGUP)
            targetUnit.UpdateUnitHealingHits(2, true);
        else if (curEffectName == EffectName.HEALINGDOWN)
            targetUnit.UpdateUnitHealingHits(2, false);
        else if (curEffectName == EffectName.PARRY)
            targetUnit.isParrying = true;
        else if (curEffectName == EffectName.SPEEDUP)
            GameManager.Instance.AddSpeedBuffUnit();
        else if (curEffectName == EffectName.MARK)
        {
            // Toggle mark effect on the target
            targetUnit.curRecieveDamageAmp += (int)powerPercent;
        }
        else if (curEffectName == EffectName.DEFENSEUP)
            GameManager.Instance.AdjustUnitDefense(true);
        else if (curEffectName == EffectName.DEFENSEDOWN)
            GameManager.Instance.AdjustUnitDefense(false);

    }

    public void EffectRemove(UnitFunctionality unit)
    {
        if (curEffectName == EffectName.HEALTHUP)
        {
            float newMaxHealth = tempAddedHealth;
            unit.UpdateUnitMaxHealth((int)newMaxHealth, false, false);
            unit.UpdateUnitCurHealth((int)tempAddedHealth, true, false);
            unit.StartCoroutine(unit.SpawnPowerUI(tempAddedHealth, false, true, this));
        }
        else if (curEffectName == EffectName.POISON)
        {
            // healing recieved is halved while on
            unit.ToggleUnitPoisoned(false);
            unit.ResetUnitHealingRecieved();

        }
        else if (curEffectName == EffectName.TAUNT)
            unit.ToggleTaunt(false);
        else if (curEffectName == EffectName.PARRY)
        {
            unit.isParrying = false;
            unit.attacked = false;
        }
        else if (curEffectName == EffectName.SPEEDUP)
        {
            GameManager.Instance.RemoveSpeedEffectUnit(unit);
        }
        else if (curEffectName == EffectName.DEFENSEUP)
        {
            GameManager.Instance.RemoveDefenseUpEffectUnit(unit);
        }
        else if (curEffectName == EffectName.DEFENSEDOWN)
        {
            GameManager.Instance.RemoveDefenseDownEffectUnit(unit);
        }
        else if (curEffectName == EffectName.MARK)
        {
            unit.curRecieveDamageAmp -= (int)powerPercent;
        }
        else if (curEffectName == EffectName.POWERUP)
        {
            unit.UpdateUnitDamageHits(2, false);
        }
        else if (curEffectName == EffectName.POWERDOWN)
        {
            unit.UpdateUnitDamageHits(2, true);
        }
        else if (curEffectName == EffectName.HEALINGUP)
        {
            unit.UpdateUnitHealingHits(2, false);
        }
        else if (curEffectName == EffectName.HEALINGDOWN)
        {
            unit.UpdateUnitHealingHits(2, true);
        }

        DestroyEffectAfterTime(unit);
    }

    void DestroyEffectAfterTime(UnitFunctionality unit)
    {
        //yield return new WaitForSeconds(.4f);

        /*
        if (unit.GetEffects()[unit.GetEffects().IndexOf(this)])
        {
            unit.GetEffects().Remove(this);
        }
        */


        //if (unit.GetEffects().Count == 0)
            //unit.ResetEffects();

        Destroy(gameObject);
    }
    public void TriggerPowerEffect(UnitFunctionality unitTarget)
    {
        unitTarget.ResetPowerUI();

        // Trigger effect alert
        // Do effect
        int unitMaxHealth = (int)unitTarget.GetUnitMaxHealth();
        float tempPower = (powerPercent / 100f) * unitMaxHealth;
        power = (int)tempPower;

        float newHealingPower = power;

        // Make bleed scale with recover buff // TODO

        if (curEffectName == EffectName.BLEED)
            unitTarget.UpdateUnitCurHealth(-power, true);
        else if (curEffectName == EffectName.RECOVER)
            unitTarget.UpdateUnitCurHealth((int)newHealingPower);
        else if (curEffectName == EffectName.POISON)
        {
            unitTarget.UpdateUnitCurHealth(-power, true);
        }

        if (curEffectName == EffectName.BLEED)
            unitTarget.StartCoroutine(unitTarget.SpawnPowerUI(power, false, true, this));
        else if (curEffectName == EffectName.RECOVER)
            unitTarget.StartCoroutine(unitTarget.SpawnPowerUI((int)newHealingPower, false, false, this));
        else if (curEffectName == EffectName.POISON)
            unitTarget.StartCoroutine(unitTarget.SpawnPowerUI(power, false, true, this));
    }
}

