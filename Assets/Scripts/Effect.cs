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

    float power;

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
    public int turnCountRemaining = 0;

    private Image effectIconImage;
    bool initialUse;
    private float tempAddedHealth;

    private float storedPowerAmp;
    public Color titleTextColour;

    public float addedStat;
    public int effectPowerStacks = 0;
    [SerializeField] private GameObject effectTierGO;

    private void Awake()
    {
        effectIconImage = GetComponent<Image>();
    }

    public void UpdateEffectTierImages(int count = 1)
    {
        Transform parent = gameObject.transform.GetChild(0).transform;

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(effectTierGO, parent.position, Quaternion.identity);
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.Euler(0, 0, 90);
            //go.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion(0, 0, 180, 0));
            go.name = "Effect Tier Image " + count;
        }
    }

    public void ClearEffectTierImages()
    {
        Transform parent = gameObject.transform.GetChild(0).transform;

        // Clear all potential previous effect tier images
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
    public void Setup(EffectData effect, UnitFunctionality targetUnit, int turnDuration = 1)
    {
        ClearEffectTierImages();

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
        titleTextColour = effect.titleTextColour;
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
        if (turns == 0)
        {
            turnCountRemaining = 0;
        }
        else
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
        //EffectApply(unit);

        if (turnCountRemaining == 1)
            RemoveEffect(unit);

        turnCountRemaining--;
        effectTurnCountText.text = turnCountRemaining.ToString();

        //StartCoroutine(ReduceEffectWait(.25f));
    }

    IEnumerator ReduceEffectWait(float time)
    {
        turnCountRemaining--;
        effectTurnCountText.text = turnCountRemaining.ToString();

        yield return new WaitForSeconds(time);
    }

    public void EffectApply(UnitFunctionality targetUnit)
    {
        if (curEffectName == EffectName.HEALTHUP)
        {
            tempAddedHealth = (powerPercent / 100f) * targetUnit.GetStartingMaxHealth();
            addedStat += tempAddedHealth;
            float newCurHealth = (int)tempAddedHealth;
            targetUnit.UpdateUnitMaxHealth((int)newCurHealth);
            targetUnit.UpdateUnitCurHealth((int)newCurHealth, false, false, true, false);
            targetUnit.StartCoroutine(targetUnit.SpawnPowerUI(newCurHealth, false, false, this));
        }
        else if (curEffectName == EffectName.TAUNT)
            targetUnit.ToggleTaunt(true);
        else if (curEffectName == EffectName.POISON)
        {
            // healing recieved is halved while on
            targetUnit.ToggleUnitPoisoned(true);

            targetUnit.UpdateUnitHealingRecieved(-0.05f);

            //addedStat += .2f;
        }
        else if (curEffectName == EffectName.POWERUP)
        {
            //storedPowerAmp = powerAmp;
            //targetUnit.UpdatePowerIncPerLv((int)storedPowerAmp);   

            addedStat += 1;
            targetUnit.UpdateUnitPowerHits(1, true);
        }
        else if (curEffectName == EffectName.POWERDOWN)
        {
            addedStat += 1;
            targetUnit.UpdateUnitPowerHits(1, false);
        }
        else if (curEffectName == EffectName.HEALINGUP)
        {
            addedStat += 1;
            targetUnit.UpdateUnitHealingHits(1, true);
        }

        else if (curEffectName == EffectName.HEALINGDOWN)
        {
            addedStat += 1;
            targetUnit.UpdateUnitHealingHits(1, false);
        }
        else if (curEffectName == EffectName.PARRY)
            targetUnit.isParrying = true;
        else if (curEffectName == EffectName.SPEEDUP)
        {
            //addedStat += 2;
            GameManager.Instance.AdjustSpeedBuffUnit();
        }
        else if (curEffectName == EffectName.SPEEDDOWN)
        {
            GameManager.Instance.AdjustSpeedBuffUnit(false);
        }
        else if (curEffectName == EffectName.MARK)
        {
            // Toggle mark effect on the target
            targetUnit.curRecieveDamageAmp += (int)powerPercent;
        }
        else if (curEffectName == EffectName.DEFENSEUP)
        {
            GameManager.Instance.AdjustUnitDefense(true);
        }
        else if (curEffectName == EffectName.DEFENSEDOWN)
        {
            GameManager.Instance.AdjustUnitDefense(false);
        }
        else if (curEffectName == EffectName.BLEED)
        {

        }
        else if (curEffectName == EffectName.RECOVER)
        {

        }

        effectPowerStacks++;
    }

    public void EffectRemove(UnitFunctionality unit, bool doEffects = true)
    {
        //Debug.Log("removing effect");
        if (doEffects)
        {
            if (curEffectName == EffectName.HEALTHUP)
            {
                float healthAdded = addedStat;
                unit.UpdateUnitMaxHealth((int)healthAdded, false, false);
                //unit.UpdateUnitCurHealth((int)healthAdded, true, false, true, false, true);
                unit.StartCoroutine(unit.SpawnPowerUI(healthAdded, false, true, this));

                addedStat = 0;
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
                GameManager.Instance.RemoveSpeedUpEffectUnit(unit);
            }
            else if (curEffectName == EffectName.SPEEDDOWN)
            {
                GameManager.Instance.RemoveSpeedDownEffectUnit(unit);
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
                unit.UpdateUnitPowerHits((int)addedStat, false);
            }
            else if (curEffectName == EffectName.POWERDOWN)
            {
                unit.UpdateUnitPowerHits((int)addedStat, true);
            }
            else if (curEffectName == EffectName.HEALINGUP)
            {
                unit.UpdateUnitHealingHits((int)addedStat, false);
            }
            else if (curEffectName == EffectName.HEALINGDOWN)
            {
                unit.UpdateUnitHealingHits((int)addedStat, true);
            }
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
        effectPowerStacks = 0;

        if (this != null)
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
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



        // Ensure only healing is cut
        if (curEffectType == EffectType.SUPPORT)
        {
            newHealingPower *= unitTarget.curHealingRecieved;
        }

        newHealingPower *= effectPowerStacks;

        // Debug.Log("power = " + power);

        if (curEffectName == EffectName.BLEED)
        {
            power *= effectPowerStacks;
            unitTarget.UpdateUnitCurHealth((int)-power, true, false, true, true, true);
        }
        else if (curEffectName == EffectName.POISON)
            unitTarget.UpdateUnitCurHealth((int)-power, true, false, true, true, true);
        else if (curEffectName == EffectName.RECOVER)
            unitTarget.UpdateUnitCurHealth((int)newHealingPower);


        if (curEffectName == EffectName.BLEED)
            unitTarget.StartCoroutine(unitTarget.SpawnPowerUI(power, false, true, this, false));
        else if (curEffectName == EffectName.POISON)
            unitTarget.StartCoroutine(unitTarget.SpawnPowerUI(power, false, true, this, false, true));
        else if (curEffectName == EffectName.RECOVER)
            unitTarget.StartCoroutine(unitTarget.SpawnPowerUI((int)newHealingPower, false, false, this));

    }
}

