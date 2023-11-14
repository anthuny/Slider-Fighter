using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFunctionality : MonoBehaviour
{
    RectTransform rt;
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;

    public enum LastOpenedMastery { STANDARD, ADVANCED };
    public LastOpenedMastery lastOpenedStatPage;

    [SerializeField] private UIElement selectionCircle;
    [SerializeField] private UIElement unitVisuals;
    private Sprite unitIcon;
    [SerializeField] private Transform unitVisualsParent;
    [SerializeField] private Transform powerUIParent;
    [SerializeField] private UIElement statUI;
    [SerializeField] private Image unitHealthBar;
    [SerializeField] private Image unitAttackBar;
    public UIElement healthBarUIElement;
    public UIElement attackBarUIElement;

    public Image unitImage;
    public UIElement curUnitTurnArrow;
    public int curSpeed;
    public int curPower;
    public float curPowerInc = 0;
    public int curDefense;
    [SerializeField] private int curHealth;
    private int maxHealth;
    private int curlevel;
    private float curExp;
    private float maxExp;
    [HideInInspector]
    public int unitStartTurnEnergyGain;
    public EnergyCost energyCostImage;
    [SerializeField] private Transform projectileParent;
    //s[SerializeField] private Sprite projectileSprite;
    [SerializeField] private UIElement unitExpBar;
    [SerializeField] private Text unitLevelText;
    [SerializeField] private Image unitExpBarImage;
    [SerializeField] private Text unitExpGainText;
    [SerializeField] private float fillAmountInterval;
    [SerializeField] private UIElement unitBg;
    [SerializeField] private UIElement unitUIElement;
    [SerializeField] private Transform effectsParent;
    public UIElement effects;
    [SerializeField] private List<Effect> activeEffects = new List<Effect>();
    public int curRecieveDamageAmp = 100;
    [SerializeField] private Color unitColour;

    [SerializeField] private List<Item> equiptItems = new List<Item>();

    [SerializeField] private List<Stat> curStatPage = new List<Stat>();

    [SerializeField] private List<Stat> standardMasteries = new List<Stat>();
    [SerializeField] private List<Stat> advancedMasteries = new List<Stat>();

    public UnitData unitData;

    [HideInInspector]
    public GameObject prevPowerUI;

    public bool isSelected;
    private int unitValue;
    private Animator animator;

    // Team Stat Page
    public int statsBase1Added;
    public int statsBase2Added;
    public int statsBase3Added;
    public int statsBase4Added;
    public int statsBase5Added;

    public int statsAdv1Added;
    public int statsAdv2Added;
    public int statsAdv3Added;
    public int statsAdv4Added;

    private int spentMasteryTotalPoints = 0;
    public int statsSpentBasePoints = 0;
    public int statsSpentAdvPoints = 0;

    [SerializeField] private int skill0CurCooldown = 0;
    [SerializeField] private int skill1CurCooldown = 0;
    [SerializeField] private int skill2CurCooldown = 0;
    [SerializeField] private int skill3CurCooldown = 0;

    public bool idleBattle;
    public bool isDead;
    public bool isTaunting;
    public bool isParrying;
    public bool attacked;
    public bool isVisible;


    private int curAttackCharge;
    private int attackChargeTurnStart;
    private int maxAttackCharge = 100;

    private float oldCurSpeed = 0;
    private float oldCurDefense = 0;
    public bool isSpeedUp;
    public bool isDefenseUp;
    public bool isDefenseDown;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ToggleUnitExpVisual(false);
        ToggleUnitBG(false);
        ResetEffects();

        UpdateUnitPowerInc(1);

        UpdateIsVisible(false);
    }

    public void UpdateLastOpenedMastery(TeamSetup.ActiveStatType masteryType)
    {
        /*
        if (masteryType == TeamSetup.ActiveStatType.STANDARD)
            lastOpenedStatPage = LastOpenedMastery.STANDARD;
        else if (masteryType == TeamSetup.ActiveStatType.ADVANCED)
            lastOpenedStatPage = LastOpenedMastery.ADVANCED;
         */

        lastOpenedStatPage = LastOpenedMastery.STANDARD;
    }

    public LastOpenedMastery GetLastOpenedMastery()
    {
        return lastOpenedStatPage;
    }

    public void UpdateFacingDirection(bool right = true)
    {
        if (right)
            gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y);
        else
            gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y);
    }

    public void UpdateIsVisible(bool toggle)
    {
        isVisible = toggle;

        if (curUnitType == UnitType.PLAYER)
        {
            if (isVisible)
                unitUIElement.UpdateAlpha(1);
            else
                unitUIElement.UpdateAlpha(0);
        }
    }

    public bool GetIsVisible()
    {
        return isVisible;
    }
    public void AddOwnedItems(Item item)
    {
        equiptItems.Add(item);
    }

    public List<Item> GetEquipItems()
    {
        return equiptItems;
    }

    public void UpdateCurrentMasteries(List<Stat> masteries)
    {
        this.curStatPage = masteries;
    }

    public void UpdateStandardMasteries(List<Stat> masteries)
    {
        this.standardMasteries = masteries;
    }
    public void UpdateAdvancedMasteries(List<Stat> masteries)
    {
        this.advancedMasteries = masteries;
    }

    public Stat GetCurrentStat(int count)
    {
        return curStatPage[count];
    }
    public Stat GetStandardMastery(int count)
    {
        return standardMasteries[count];
    }
    public Stat GetAdvancedMastery(int count)
    {
        return advancedMasteries[count];
    }

    public List<Stat> GetAllCurrentMasteries()
    {
        return curStatPage;
    }
    public List<Stat> GetAllStandardMasteries()
    {
        return standardMasteries;
    }
    public List<Stat> GetAllAdvancedMasteries()
    {
        return advancedMasteries;
    }

    public void ClearMasteries()
    {
        curStatPage.Clear();
        advancedMasteries.Clear();
    }

    public int GetEquipItemCount(string itemName)
    {
        int amountOfItems = 0;

        for (int i = 0; i < GetEquipItems().Count; i++)
        {
            if (GetEquipItems()[i].itemName == itemName)
            {
                amountOfItems++;
            }
        }

        return amountOfItems;
    }

    public void SetPositionAndParent(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void TriggerTextAlert(string name, float alpha, bool effect, string gradient = null, bool levelUp = false)
    {
        statUI.UpdateContentText(name);
        statUI.UpdateAlpha(alpha);

        // Set correct text colour gradient
        if (effect)
        {
            if (gradient == "Inflict")
                statUI.UpdateContentTextColour(EffectManager.instance.gradientEffectAlert);
            else if (gradient == "Trigger")
                statUI.UpdateContentTextColour(EffectManager.instance.gradientEffectTrigger);
        }
        else
            statUI.UpdateContentTextColour(GameManager.Instance.gradientSkillAlert);

        if (levelUp)
            statUI.UpdateContentTextColour(GameManager.Instance.gradientLevelUpAlert);
    }

    public void ToggleIdleBattle(bool toggle)
    {
        idleBattle = toggle;
    }

    public bool GetIdleBattle()
    {
        return idleBattle;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public int GetSpentMasteryPoints()
    {
        return spentMasteryTotalPoints;
    }

    public void UpdateSpentMasteryPoints(int add)
    {
        spentMasteryTotalPoints += add;
    }

    public void ResetSpentStatPoints()
    {
        spentMasteryTotalPoints = 0;
        statsSpentBasePoints = 0;
        statsSpentAdvPoints = 0;
    }

    public void ToggleHideEffects(bool toggle)
    {
        if (!toggle)
            effects.UpdateAlpha(1);
        else
            effects.UpdateAlpha(0);
    }

    public IEnumerator StartUnitTurn()
    {
        yield return new WaitForSeconds(GameManager.Instance.enemyEffectWaitTime);

        // Do unit's turn automatically if its on idle battle
        if (GetIdleBattle() && GameManager.Instance.activeRoomAllies.Count >= 1 && !isDead)
        {
            yield return new WaitForSeconds(GameManager.Instance.enemySkillThinkTime);

            // If unit has energy to choose a skill, choose one
            GameManager.Instance.UpdateActiveSkill(ChooseRandomSkill());

            // Trigger current unit's turn energy count to deplete for skill use
            //GameManager.Instance.UpdateActiveUnitEnergyBar(true, false, GameManager.Instance.activeSkill.skillEnergyCost, true);
            //GameManager.Instance.UpdateActiveUnitHealthBar(false);

            // Select units
            GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);

            TriggerTextAlert(GameManager.Instance.GetActiveSkill().skillName, 1, false);

            
            if (GameManager.Instance.GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED && GameManager.Instance.GetActiveSkill().skillPower != 0)
            {
                animator.SetTrigger("SkillFlg");
                //yield return new WaitForSeconds(GameManager.Instance.enemyRangedSkillWaitTime);
            }
            else if (GameManager.Instance.GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED && GameManager.Instance.GetActiveSkill().skillPower == 0)
            {
                animator.SetTrigger("SkillFlg");
            }
            else
            {
                animator.SetTrigger("AttackFlg");
                //yield return new WaitForSeconds(GameManager.Instance.enemyMeleeSkillWaitTime);
            }

            // Adjust power based on skill effect amp on target then send it 
            StartCoroutine(GameManager.Instance.WeaponAttackCommand(GameManager.Instance.activeSkill.skillPower, GameManager.Instance.activeSkill.skillAttackCount));

            /*
            // End turn
            GameManager.Instance.ToggleEndTurnButton(false);
            GameManager.Instance.UpdateTurnOrder();
            yield break;
            */
        }
    }

    public void DecreaseEffectTurnsLeft(bool turnStart, bool parry = false)
    {
        // If no effects remain on the unit, stop
        if (activeEffects.Count >= 1)
        {
            if (activeEffects[0] == null)
                return;
        }

        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (parry)
            {
                if (activeEffects[i].curEffectName == Effect.EffectName.PARRY)
                {
                    activeEffects[i].TriggerPowerEffect();
                    TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                    activeEffects[i].ReduceTurnCountText(this);
                    //return;
                }
            }

            if (turnStart)
            {
                if (activeEffects[i].curEffectTrigger == Effect.EffectTrigger.TURNSTART)
                {
                    activeEffects[i].TriggerPowerEffect();
                    TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                    activeEffects[i].ReduceTurnCountText(this);
                }
            }
            else
            {
                if (activeEffects[i].curEffectTrigger == Effect.EffectTrigger.TURNEND)
                {
                    activeEffects[i].TriggerPowerEffect();
                    TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                    activeEffects[i].ReduceTurnCountText(this);
                }
            }
        }
    }


    public IEnumerator UnitEndTurn()
    {
        yield return new WaitForSeconds(GameManager.Instance.enemyAttackWaitTime);

        // End turn
        GameManager.Instance.UpdateTurnOrder();
    }

    public int GetSkill0CurCooldown()
    {
        return skill0CurCooldown;
    }
    public int GetSkill1CurCooldown()
    {
        return skill1CurCooldown;
    }
    public int GetSkill2CurCooldown()
    {
        return skill2CurCooldown;
    }
    public int GetSkill3CurCooldown()
    {
        return skill3CurCooldown;
    }

    public void ResetSkill0Cooldown()
    {
        skill0CurCooldown = 0;
        //GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(skill0CurCooldown.ToString());
    }
    public void ResetSkill1Cooldown()
    {
        skill1CurCooldown = 0;
        GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(skill1CurCooldown.ToString());
    }
    public void ResetSkill2Cooldown()
    {
        skill2CurCooldown = 0;
        GameManager.Instance.skill2IconCooldownUIText.UpdateUIText(skill2CurCooldown.ToString());
    }
    public void ResetSkill3Cooldown()
    {
        skill3CurCooldown = 0;
        GameManager.Instance.skill3IconCooldownUIText.UpdateUIText(skill3CurCooldown.ToString());
    }

    public void SetSkill0CooldownMax()
    {
        skill0CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill0().skillCooldown;
        //GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(skill0CurCooldown.ToString());
    }
    public void SetSkill1CooldownMax()
    {
        skill1CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill1().skillCooldown + 1;
        GameManager.Instance.skill1IconCooldownUIText.UpdateUIText((skill1CurCooldown).ToString());
    }
    public void SetSkill2CooldownMax()
    {
        skill2CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill2().skillCooldown + 1;
        GameManager.Instance.skill2IconCooldownUIText.UpdateUIText((skill2CurCooldown).ToString());
    }
    public void SetSkill3CooldownMax()
    {
        skill3CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill3().skillCooldown + 1;
        GameManager.Instance.skill3IconCooldownUIText.UpdateUIText((skill3CurCooldown).ToString());
    }

    public void DecreaseSkill0Cooldown()
    {
        skill0CurCooldown--;

        if (skill0CurCooldown <= 0)
            skill0CurCooldown = 0;

        //GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(skill1CurCooldown.ToString());
    }
    public void DecreaseSkill1Cooldown()
    {
        skill1CurCooldown--;

        if (skill1CurCooldown <= 0)
            skill1CurCooldown = 0;

        GameManager.Instance.skill1IconCooldownUIText.UpdateUIText(skill1CurCooldown.ToString());
    }
    public void DecreaseSkill2Cooldown()
    {
        skill2CurCooldown--;

        if (skill2CurCooldown <= 0)
            skill2CurCooldown = 0;

        GameManager.Instance.skill2IconCooldownUIText.UpdateUIText(skill2CurCooldown.ToString());
    }
    public void DecreaseSkill3Cooldown()
    {
        skill3CurCooldown--;

        if (skill3CurCooldown <= 0)
            skill3CurCooldown = 0;

        GameManager.Instance.skill3IconCooldownUIText.UpdateUIText(skill3CurCooldown.ToString());
    }

    SkillData ChooseRandomSkill()
    {
        int unitEnemyIntelligence = 10;
        for (int i = 0; i < unitEnemyIntelligence; i++)
        {
            int rand = Random.Range(1, 5);
            //Debug.Log(rand);
            if (rand == 1)  // Skill 1
            {
                if (skill1CurCooldown == 0)
                    return GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill1();
                else
                    continue;
            }
            else if (rand == 2)  // Skill 2
            {
                if (skill2CurCooldown == 0)
                    return GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill2();
                else
                    continue;
            }
            else if (rand == 3)  // Skill 3
            {
                if (skill3CurCooldown == 0)
                    return GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill3();
                else
                    continue;
            }

            else if (rand == 4)
            {
                return GameManager.Instance.GetActiveUnitFunctionality().unitData.GetSkill0();
            }
        }

        return GameManager.Instance.GetActiveUnitFunctionality().unitData.skill0;
    }

    public List<Effect> GetEffects()
    {
        return activeEffects;
    }

    public void AddUnitEffect(EffectData addedEffect, UnitFunctionality targetUnit, int turnDuration = 1, int effectHitAcc = -1)
    {
        // If player miss, do not apply effect
        if (effectHitAcc == 0 || targetUnit.isParrying)
            return;

        // If unit is already effected with this effect, add to the effect
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (addedEffect.effectName == activeEffects[i].effectName)
            {
                // If skill came from an enemy, with no hit accuracy
                if (effectHitAcc == -1)
                {
                    // Determining whether the effect hits, If it fails, stop
                    if (GameManager.Instance.GetActiveSkill().effectHitChance != 0)
                    {
                        int rand = Random.Range(0, 100);
                        if (rand > GameManager.Instance.GetActiveSkill().effectHitChance)
                            return;
                    }

                    // Cause Effect. Do not trigger text alert if its casting a skill on self. (BECAUSE: Skill announce overtakes this).
                    if (GameManager.Instance.GetActiveUnitFunctionality() != this)
                        TriggerTextAlert(GameManager.Instance.GetActiveSkill().effect.effectName, 1, true, "Inflict");
                    activeEffects[i].AddTurnCountText(turnDuration);
                    return;
                }
                    
                for (int x = 0; x < effectHitAcc; x++)
                {
                    // Determining whether the effect hits, If it fails, stop
                    if (GameManager.Instance.GetActiveSkill().effectHitChance != 0)
                    {
                        int rand = Random.Range(0, 100);
                        if (rand > GameManager.Instance.GetActiveSkill().effectHitChance)
                            return;
                    }

                    activeEffects[i].AddTurnCountText(turnDuration);
                }
                return;
            }
        }

        // Determining whether the effect hits, If it fails, stop
        if (GameManager.Instance.GetActiveSkill().effectHitChance != 0)
        {
            int rand = Random.Range(0, 100);
            if (rand < GameManager.Instance.GetActiveSkill().effectHitChance)
                return;
        }

        // Cause Effect. Do not trigger text alert if its casting a skill on self. (BECAUSE: Skill announce overtakes this).
        if (GameManager.Instance.GetActiveUnitFunctionality() != this)
            TriggerTextAlert(GameManager.Instance.GetActiveSkill().effect.effectName, 1, true, "Inflict");

        // Spawn new effect on target unit
        GameObject go = Instantiate(EffectManager.instance.effectPrefab, effectsParent.transform);
        go.transform.SetParent(effectsParent);
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        go.transform.localScale = new Vector3(1, 1, 1);

        Effect effect = go.GetComponent<Effect>();
        activeEffects.Add(effect);
        effect.Setup(addedEffect, targetUnit, turnDuration);

        int index = activeEffects.IndexOf(effect);

        // If effect was used through a weapon hit attack, add counts to that effect
        if (effectHitAcc != -1)
        {
            for (int x = 0; x < effectHitAcc - 1; x++)
            {
                activeEffects[index].AddTurnCountText(1);
            }
        }
    }

    public void ResetEffects()
    {
        activeEffects.Clear();
        for (int i = 0; i < effectsParent.childCount; i++)
        {
            Destroy(effectsParent.GetChild(i).gameObject);
        }
    }

    /*
    public void UpdateUnitProjectileSprite(Sprite sprite)
    {
        projectileSprite = sprite;
    }
    */
    public void UpdateUnitValue(int val)
    {
        unitValue = val;
    }

    public int GetUnitValue()
    {
        return unitValue;
    }

    public void UpdateUnitColour(Color color)
    {
        unitColour = color;
    }

    public Color GetUnitColour()
    {
        return unitColour;
    }

    public void UpdateUnitVisual(Sprite sprite)
    {
        unitImage.sprite = sprite;
    }

    public Sprite GetUnitSprite()
    {
        return unitImage.sprite;
    }

    public Sprite GetUnitIcon()
    {
        return unitIcon;
    }

    public void UpdateUnitIcon(Sprite sprite)
    {
        unitIcon = sprite;
    }

    public float GetUnitPowerInc()
    {
        return curPowerInc;
    }

    public void UpdateUnitPowerInc(float newPowerInc)
    {
        curPowerInc += newPowerInc;
    }

    public void ToggleTaunt(bool toggle)
    {
        isTaunting = toggle;
    }

    int healCount = 0;
    int damageCount = 0;

    public void ResetPowerUI()
    {
        damageCount = 0;
        healCount = 0;
    }
    public void SpawnPowerUI(float power = 10f, bool isParrying = false, bool offense = false, Effect effect = null, int powerUICount = 1, bool isLuckyHit = false, bool isHeal = false)
    {
        if (offense)
            damageCount++;
        else
            healCount++;

        if (!isLuckyHit)
        {
            // If this is NOT the first power text UI
            if (prevPowerUI != null)
            {
                // If power UI count has been reached from heal / damage, reset back to original Y position.
                if (healCount >= GameManager.Instance.maxPowerUICount+1 || damageCount >= GameManager.Instance.maxPowerUICount+1)
                {
                    prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, powerUIParent.position, Quaternion.identity);
                    healCount = 0;
                    damageCount = 0;
                }
                else
                    prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.Instance.powerUIHeightLvInc), Quaternion.identity); 
            }
            // If this IS the first power text UI
            else
                prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, powerUIParent.position, Quaternion.identity);
        }
        else
            prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.Instance.powerUIHeightLvInc), Quaternion.identity);

        prevPowerUI.transform.SetParent(powerUIParent);
        prevPowerUI.transform.localScale = Vector2.one;

        PowerText powerText = prevPowerUI.GetComponent<PowerText>();

        if (isParrying)
        {
            powerText.UpdatePowerTextFontSize(GameManager.Instance.powerSkillParryFontSize);
            powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillParry);
            powerText.UpdatePowerText(GameManager.Instance.parryPowerText);
            return;
        }

        // If power is 0, display that it missed
        if (power <= 0)
        {
            powerText.UpdatePowerTextFontSize(GameManager.Instance.powerMissFontSize);
            powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillMiss);
            powerText.UpdatePowerText(GameManager.Instance.missPowerText);
            //powerText.UpdatePowerText(power.ToString());   // Update Power Text
            return;
        }

        // Make Animate
        powerText.GetComponent<UIElement>().UpdateAlpha(1);

        float Randx = Random.Range(-GameManager.Instance.powerHorizontalRandomness, GameManager.Instance.powerHorizontalRandomness);
        prevPowerUI.transform.localPosition = new Vector2(Randx, prevPowerUI.transform.localPosition.y);

        //powerText.UpdateSortingOrder(powerUICount);

        // Otherwise, display the power
        powerText.UpdatePowerTextFontSize(GameManager.Instance.powerHitFontSize);

        // Update Text Colour
        if (effect == null)
        {
            if (!isHeal)
            {
                // Change power text colour to offense colour if the type of attack is offense
                if (GameManager.Instance.activeSkill.curSkillType == SkillData.SkillType.OFFENSE)
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillAttack);
                // Change power text colour to support colour if the type of attack is support
                else if (GameManager.Instance.activeSkill.curSkillType == SkillData.SkillType.SUPPORT)
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);
            }
            else
            {
                // Change power text colour to support colour if the type of attack is support
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);
            }

        }
        else
        {
            // Change power text colour to offense colour if the type of attack is offense
            if (effect.curEffectType == Effect.EffectType.OFFENSE)
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillAttack);
            // Change power text colour to support colour if the type of attack is support
            else if (effect.curEffectType == Effect.EffectType.SUPPORT)
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);

            if (effect.curEffectName == Effect.EffectName.HEALTHUP && offense)
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillAttack);
        }

        int finalPower = (int)power;
        powerText.UpdatePowerText(finalPower.ToString());   // Update Power Text
    }

    public void ResetPreviousPowerUI()
    {
        prevPowerUI = null;
    }

    public void SpawnProjectile(Transform target)
    {
        GameObject go = Instantiate(GameManager.Instance.unitProjectile, projectileParent);
        go.transform.SetParent(projectileParent);

        // Set projectile to scale
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);

        go.transform.transform.localPosition = new Vector3(0, 0, 0);
        //go.transform.localScale = new Vector3(.75f, .75f);

        Projectile projectile = go.GetComponent<Projectile>();
        projectile.UpdateProjectileSprite(GameManager.Instance.GetActiveSkill().skillProjectile);
        projectile.UpdateProjectileAnimator(GameManager.Instance.GetActiveSkill().projectileAC);
        projectile.ToggleAllowSpin(GameManager.Instance.GetActiveSkill().projectileAllowSpin);

        if (curUnitType == UnitType.PLAYER)
            projectile.UpdateTeam(true);
        else
            projectile.UpdateTeam(false);

        projectile.LookAtTarget(target);
        projectile.UpdateSpeed(GameManager.Instance.GetActiveSkill().projectileSpeed);
    }


    public void ResetPosition()
    {
        //transform.position = Vector2.zero;
        rt.localPosition = Vector2.zero;
    }

    public void UpdateUnitName(string unitName)
    {
        gameObject.name = unitName;
    }

    public string GetUnitName()
    {
        return gameObject.name;
    }

    public void UpdateUnitSprite(GameObject spriteGO)
    {
        GameObject go = Instantiate(spriteGO, unitVisualsParent);

        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localScale = new Vector3(1, 1, 1);

        animator = go.GetComponent<Animator>();
    }

    public void UpdateUnitType(string unitType)
    {
        if (unitType == "Enemy")
            curUnitType = UnitType.ENEMY;
        else
            curUnitType = UnitType.PLAYER;
    }

    public void ToggleSelected(bool toggle)
    {
        isSelected = toggle;

        if (toggle)
            selectionCircle.UpdateAlpha(1);
        else
            selectionCircle.UpdateAlpha(0);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public bool CheckIfUnitIsDead()
    {
        // If unit's health is 0 or lower
        if (curHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetIsDead()
    {
        isDead = false;
    }

    IEnumerator EnsureUnitIsDead()
    {
        // If unit's health is 0 or lower
        if (curHealth <= 0 && !isDead)
        {
            isDead = true;

            curHealth = 0;

            animator.SetTrigger("DeathFlg");

            GameManager.Instance.RemoveUnit(this);

            yield return new WaitForSeconds(1.2f);



            DestroyUnit();
        }
    }

    public void UpdateUnitExp(int gainedExp)
    {
        StartCoroutine(UpdateUnitExpVisual(gainedExp));
    }

    void ToggleExpGainedText(bool toggle, string text)
    {
        unitExpGainText.gameObject.SetActive(toggle);
        unitExpGainText.text = "+ " + text;
    }

    public void ToggleUnitBG(bool toggle)
    {
        if (toggle)
            unitBg.UpdateAlpha(.1f);
        else
            unitBg.UpdateAlpha(0);
    }

    public IEnumerator UpdateUnitExpVisual(int gainedExp)
    {
        ToggleExpGainedText(true, gainedExp.ToString());

        float curFillAmount = GetCurExp() / GetMaxExp();

        // Update exp bar for current energy
        unitExpBarImage.fillAmount = curFillAmount;

        // Display exp visual
        ToggleUnitExpVisual(true);

        for (int i = 0; i < gainedExp; i++)
        {
            yield return new WaitForSeconds(GameManager.Instance.fillAmountIntervalTimeGap);

            // If unit leveled up
            if (GetCurExp() >= GetMaxExp())
            {
                int remainingExp = gainedExp - i;
                UpdateUnitLevel(1, remainingExp);

                TriggerTextAlert("LEVEL UP!", 1, false, "", true);

                UpdateUnitCurHealth((int)GetUnitMaxHealth(), false, true);
                yield break;
            }
            else
            {
                IncreaseCurExp(1);
                unitExpBarImage.fillAmount = GetCurExp() / GetMaxExp();
            }
        }

        yield return new WaitForSeconds(GameManager.Instance.timePostExp);
        //ToggleUnitExpVisual(false);
    }

    public void ToggleUnitExpVisual(bool toggle)
    {
        if (toggle)
            unitExpBar.UpdateAlpha(1);
        else
            unitExpBar.UpdateAlpha(0);
    }

    public void UpdateUnitLevel(int level, int extraExp = 0)
    {
        curlevel += level;
        UpdateUnitLevelVisual(GetUnitLevel());

        ResetUnitExp();
        UpdateUnitMaxExp();

        if (extraExp != 0)
        {
            //Debug.Log(gameObject.name + " extra exp = " + extraExp);
            UpdateUnitExp(extraExp);
        }
    }

    void UpdateUnitLevelVisual(int level)
    {
        unitLevelText.text = level.ToString();
    }

    public int GetUnitLevel()
    {
        return curlevel;
    }

    void ResetUnitExp()
    {
        curExp = 0;
    }

    void IncreaseCurExp(float exp)
    {
        curExp += exp;

        if (GetCurExp() >= GetMaxExp())
        {
            curExp = (int)GetMaxExp();
        }
    }

    public void UpdateUnitMaxExp()
    {
        float temp;
        if (GetUnitLevel() != 1)
        {
            temp = GameManager.Instance.maxExpStarting + (GameManager.Instance.expIncPerLv * (GetUnitLevel() - 1));
            //temp = (GameManager.instance.maxExpLevel1 + ((GameManager.instance.expIncPerLv / GameManager.instance.maxExpLevel1) * 100f)) * GetUnitLevel();
            maxExp = (int)temp;
            //Debug.Log(gameObject.name + " " + maxExp);
        }
        else
        {
            temp = GameManager.Instance.maxExpStarting * GetUnitLevel();
            maxExp = (int)temp;
        }
    }

    public float GetCurExp()
    {
        return curExp;
    }

    public float GetMaxExp()
    {
        UpdateUnitMaxExp();
        return maxExp;
    }

    public void UpdateUnitCurHealth(int power, bool damaging = false, bool setHealth = false)
    {
        if (isDead)
            return;

        //Debug.Log(gameObject.name + " " + power);

        float absPower = Mathf.Abs((float)power);

        if (!setHealth)
        {
            // Damaging
            if (damaging)
            {
                //float tempPower;
                //tempPower = (curRecieveDamageAmp / 100f) * absPower;
                //float newPower = absPower + tempPower;

                if (curHealth > 0)
                {
                    // Damage formula - Defense 
                    float temp = (curDefense / 100f) * absPower;
                    float powerIncArmor = absPower - temp;

                    curHealth -= (int)powerIncArmor;
                }


                if (curHealth < 0)
                    curHealth = 0;

                //if (curHealth > 0)
                animator.SetTrigger("DamageFlg");
            }
            // Healing
            else
            {
                if (curHealth < maxHealth)
                    curHealth += (int)absPower;

                if (curHealth > maxHealth)
                    curHealth = maxHealth;
            }
        }
        else
        {
            curHealth = (int)absPower;

            if (curHealth > maxHealth)
                curHealth = maxHealth;

            if (curHealth < 0)
                curHealth = 0;
        }


        UpdateUnitHealthVisual();
    }

    public void ToggleDamageAnimOff()
    {
        animator.SetBool("DamageFlg", false);
    }

    public void UpdateUnitMaxHealth(int newMaxHealth, bool set = false)
    {
        maxHealth = newMaxHealth;

        if (set)
            curHealth = maxHealth;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        UpdateUnitHealthVisual();
    }

    void UpdateUnitHealthVisual()
    {
        ToggleUnitHealthBar(true);

        unitHealthBar.fillAmount = (float)curHealth / (float)maxHealth;

        if (CheckIfUnitIsDead())
            StartCoroutine(EnsureUnitIsDead());
    }

    public int GetCurAttackCharge()
    {
        return curAttackCharge;
    }

    public void CalculateUnitAttackChargeTurnStart()
    {
        attackChargeTurnStart = curSpeed;
    }

    public void ResetUnitCurAttackCharge()
    {
        curAttackCharge = 0;

        UpdateUnitAttackBarVisual();
    }
    public void UpdateUnitCurAttackCharge()
    {
        curAttackCharge += attackChargeTurnStart;

        if (curAttackCharge > 100)
            curAttackCharge = 100;

        UpdateUnitAttackBarVisual();
    }

    void UpdateUnitAttackBarVisual()
    {
        ToggleUnitAttackBar(true);

        unitAttackBar.fillAmount = (float)curAttackCharge / 100f;
    }

    public void ToggleUnitHealthBar(bool toggle)
    {
        if (toggle)
            healthBarUIElement.UpdateAlpha(1);
        else
            healthBarUIElement.UpdateAlpha(0);  
    }

    public void ToggleUnitAttackBar(bool toggle)
    {
        if (toggle)
            attackBarUIElement.UpdateAlpha(1);
        else
            attackBarUIElement.UpdateAlpha(0);
    }

    public void UpdateUnitSpeed(int newSpeed)
    {
        curSpeed = newSpeed;
    }

    public void UpdateUnitOldSpeed(int oldSpeed)
    {
        oldCurSpeed = oldSpeed;
    }

    public float GetOldSpeed()
    {
        return oldCurSpeed;
    }

    public void ResetOldSpeed()
    {
        oldCurSpeed = 0;
    }

    public void UpdateUnitOldDefense(int def)
    {
        oldCurDefense = def;
    }

    public float GetOldDefense()
    {
        return oldCurDefense;
    }

    public void ResetOldDefense()
    {
        oldCurDefense = 0;
    }

    public bool GetIsSpeedUp()
    {
        return isSpeedUp;
    }

    public bool GetIsDefenseUp()
    {
        return isDefenseUp;
    }

    public bool GetIsDefenseDown()
    {
        return isDefenseDown;
    }

    public void ToggleIsDefenseUp(bool toggle)
    {
        if (toggle)
            isDefenseUp = true;
        else
            isDefenseUp = false;
    }

    public void ToggleIsDefenseDown(bool toggle)
    {
        if (toggle)
            isDefenseDown = true;
        else
            isDefenseDown = false;
    }

    public void ToggleIsSpeedUp(bool toggle)
    {
        if (toggle)
            isSpeedUp = true;
        else
            isSpeedUp = false;
    }

    public void UpdateUnitPower(int newPower)
    {
        curPower = newPower;
    }

    public void UpdateUnitDefense(int newArmor)
    {
        curDefense = newArmor;
    }

    public int GetCurDefense()
    {
        return curDefense;
    }
    
    /*
    public void AddUnitDefense(int armor)
    {
        curDefense += armor;
    }

    public void RemoveUnitArmor(int armor)
    {
        curDefense -= armor;
    }
    */

    public void UpdateUnitHealth(int newCurHealth, int newMaxHealth)
    {
        UpdateUnitMaxHealth(newMaxHealth, true);
        UpdateUnitCurHealth(newCurHealth, false, true);
    }

    public float GetUnitCurHealth()
    {
        return curHealth;
    }

    public float GetUnitMaxHealth()
    {
        return maxHealth;
    }

    /*
    public void UpdateUnitEnergy(int curEnergy, int maxEnergy)
    {
        this.curEnergy = curEnergy;
        this.maxEnergy = maxEnergy;
    }

    public void UpdateMaxEnergy(int maxEnergy)
    {
        this.maxEnergy = maxEnergy;
    }
    */

    /*
    public void UpdateUnitCurEnergy(int energy)
    {
        //effects.UpdateAlpha(1);

        this.curEnergy += energy;

        if (curEnergy > maxEnergy)
            curEnergy = maxEnergy;
    }
    */

    /*
    public void UpdateUnitStartTurnEnergy(int newUnitStartTurnEnergyGain)
    {
        unitStartTurnEnergyGain = newUnitStartTurnEnergyGain;
    }
    */

    void DestroyUnit()
    {
        unitVisuals.UpdateAlpha(0);
        //GameManager.instance.UpdateTurnOrder();
        //Destroy(gameObject);
    }

    public int GetUnitExpKillGained()
    {
        int expGained = (GetUnitLevel() * GameManager.Instance.expKillGainedPerLv) + GameManager.Instance.expKillGainedStarting;
        return expGained;
    }
}
