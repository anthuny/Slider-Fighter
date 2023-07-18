using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFunctionality : MonoBehaviour
{
    RectTransform rt;
    public enum UnitType { PLAYER, ENEMY };
    public UnitType curUnitType;

    public enum LastOpenedMastery { OFFENSE, DEFENSE, UTILITY };
    public LastOpenedMastery lastOpenedMastery;

    [SerializeField] private UIElement selectionCircle;
    [SerializeField] private UIElement unitVisuals;
    private Sprite unitIcon;
    [SerializeField] private Transform unitVisualsParent;
    [SerializeField] private Transform powerUIParent;
    [SerializeField] private UIElement statUI;
    [SerializeField] private Image unitHealth;
    public Image unitImage;
    public UIElement curUnitTurnArrow;
    public int curSpeed;
    public int curPower;
    public float curPowerInc = 1;
    public int curArmor;
    private int curHealth;
    private int maxHealth;
    private int curEnergy;
    private int maxEnergy;
    private int curlevel;
    private float curExp;
    private float maxExp;
    [HideInInspector]
    public int unitStartTurnEnergyGain;
    public EnergyCost energyCostImage;
    [SerializeField] private Transform projectileParent;
    [SerializeField] private Sprite projectileSprite;
    [SerializeField] private UIElement unitExpBar;
    [SerializeField] private Text unitLevelText;
    [SerializeField] private Image unitExpBarImage;
    [SerializeField] private Text unitExpGainText;
    [SerializeField] private float fillAmountInterval;
    [SerializeField] private UIElement unitBg;

    public UIElement healthBarUIElement;
    [SerializeField] private UIElement unitUIElement;
    [SerializeField] private Transform effectsParent;
    public UIElement effects;
    [SerializeField] private List<Effect> activeEffects = new List<Effect>();
    public int curRecieveDamageAmp = 100;
    [SerializeField] private Color unitColour;

    [SerializeField] private List<Item> equipItems = new List<Item>();

    [SerializeField] private List<Mastery> currentMasteries = new List<Mastery>();

    [SerializeField] private List<UIElement> offenseMasteries = new List<UIElement>();
    [SerializeField] private List<UIElement> defenseMasteries = new List<UIElement>();
    [SerializeField] private List<UIElement> utilityMasteries = new List<UIElement>();
    //[SerializeField] private List<UIElement> currentMasteries = new List<UIElement>();
    public UnitData unitData;

    [HideInInspector]
    public GameObject prevPowerUI;

    public bool isSelected;
    private int unitValue;
    private Animator animator;

    public int masteryOffenseL1AddedCount;
    public int masteryOffenseL2AddedCount;
    public int masteryOffenseL3AddedCount;
    public int masteryOffenseL4AddedCount;

    public int masteryOffenseR1AddedCount;
    public int masteryOffenseR2AddedCount;
    public int masteryOffenseR3AddedCount;
    public int masteryOffenseR4AddedCount;


    public int masteryDefenseL1AddedCount;
    public int masteryDefenseL2AddedCount;
    public int masteryDefenseL3AddedCount;
    public int masteryDefenseL4AddedCount;

    public int masteryDefenseR1AddedCount;
    public int masteryDefenseR2AddedCount;
    public int masteryDefenseR3AddedCount;
    public int masteryDefenseR4AddedCount;


    public int masteryUtilityL1AddedCount;
    public int masteryUtilityL2AddedCount;
    public int masteryUtilityL3AddedCount;
    public int masteryUtilityL4AddedCount;

    public int masteryUtilityR1AddedCount;
    public int masteryUtilityR2AddedCount;
    public int masteryUtilityR3AddedCount;
    public int masteryUtilityR4AddedCount;

    private int spentMasteryTotalPoints = 0;
    public int spentOffenseMasteryPoints = 0;
    public int spentDefenseMasteryPoints = 0;
    public int spentUtilityMasteryPoints = 0;

    public bool idleBattle;
    public bool isDead;
    public bool isTaunting;
    public bool isParrying;
    public bool attacked;
    public bool isVisible;

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

    public void UpdateLastOpenedMastery(TeamSetup.ActiveMasteryType masteryType)
    {
        if (masteryType == TeamSetup.ActiveMasteryType.OFFENSE)
            lastOpenedMastery = LastOpenedMastery.OFFENSE;
        else if (masteryType == TeamSetup.ActiveMasteryType.DEFENSE)
            lastOpenedMastery = LastOpenedMastery.DEFENSE;
        else if (masteryType == TeamSetup.ActiveMasteryType.UTILITY)
            lastOpenedMastery = LastOpenedMastery.UTILITY;
    }

    public LastOpenedMastery GetLastOpenedMastery()
    {
        return lastOpenedMastery;
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
        equipItems.Add(item);   
    }

    public List<Item> GetEquipItems()
    {
        return equipItems;
    }

    public void UpdateCurrentMasteries(List<Mastery> masteries)
    {
        this.currentMasteries = masteries;
    }

    public void UpdateOffenseMasteries(List<UIElement> masteries)
    {
        this.offenseMasteries = masteries;
    }

    public void UpdateDefenseMasteries(List<UIElement> masteries)
    {
        this.defenseMasteries = masteries;
    }

    public void UpdateUtilityMasteries(List<UIElement> masteries)
    {
        this.utilityMasteries = masteries;
    }

    public Mastery GetCurrentMastery(int count)
    {
        return currentMasteries[count];
    }
    public UIElement GetOffensiveMastery(int count)
    {
        return offenseMasteries[count];
    }
    public UIElement GetDefenseMastery(int count)
    {
        return defenseMasteries[count];
    }
    public UIElement GetUtilityMastery(int count)
    {
        return utilityMasteries[count];
    }

    public List<Mastery> GetAllCurrentMasteries()
    {
        return currentMasteries;
    }
    public List<UIElement> GetAllOffenseMastery()
    {
        return offenseMasteries;
    }
    public List<UIElement> GetAllDefenseMastery()
    {
        return defenseMasteries;
    }
    public List<UIElement> GetAllUtilityMastery()
    {
        return utilityMasteries;
    }

    public void ClearMasteries()
    {
        currentMasteries.Clear();
        defenseMasteries.Clear();
        utilityMasteries.Clear();
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

    public void ToggleIdleBattle(bool toggle)
    {
        idleBattle = toggle;
    }

    public void TriggerTextAlert(string name, float alpha, bool effect, string gradient = null)
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

    public void ResetSpentMasteryPoints()
    {
        spentMasteryTotalPoints = 0;
        spentOffenseMasteryPoints = 0;
        spentDefenseMasteryPoints = 0;
        spentUtilityMasteryPoints = 0;
    }

    public IEnumerator StartUnitTurn()
    {
        // Do unit's turn automatically if its on idle battle
        if (GetIdleBattle() && GameManager.Instance.activeRoomAllies.Count >= 1)
        {
            //yield return new WaitForSeconds(GameManager.instance.enemyThinkTime);

            // If unit has energy to choose a skill, choose one
            GameManager.Instance.UpdateActiveSkill(ChooseRandomSkill());

            // If the skill DOESNT cost any energy, make energy cost ui appear on casting unit DOESNT APPEAR
            if (GameManager.Instance.activeSkill.skillEnergyCost != 0)
            {
                // If unit has enough energy for skill
                if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.Instance.activeSkill.skillEnergyCost)
                {
                    // Trigger current unit's turn energy count to deplete for skill use
                    GameManager.Instance.UpdateActiveUnitEnergyBar(true, false, GameManager.Instance.activeSkill.skillEnergyCost, true);
                    GameManager.Instance.UpdateActiveUnitHealthBar(false);

                    // Select units
                    GameManager.Instance.UpdateUnitSelection(GameManager.Instance.activeSkill);

                    TriggerTextAlert(GameManager.Instance.GetActiveSkill().skillName, 1, false);

                    if (GameManager.Instance.GetActiveSkill().curRangedType == SkillData.SkillRangedType.RANGED)
                    {
                        animator.SetTrigger("SkillFlg");
                        yield return new WaitForSeconds(GameManager.Instance.enemyAttackWaitTime);
                    }
                    else
                    {
                        animator.SetTrigger("AttackFlg");
                        yield return new WaitForSeconds(GameManager.Instance.triggerSkillAlertTime / 2f);
                    }

                    // Adjust power based on skill effect amp on target then send it 
                    StartCoroutine(GameManager.Instance.WeaponAttackCommand(GameManager.Instance.activeSkill.skillPower, GameManager.Instance.activeSkill.skillAttackCount));
                }
                else
                {
                    // End turn
                    GameManager.Instance.ToggleEndTurnButton(false);
                    GameManager.Instance.UpdateTurnOrder();
                    yield break;
                }
            }
            else
            {
                GameManager.Instance.UpdateTurnOrder();
                yield break;
            }
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
                    return;
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


    public IEnumerator AttackAgain()
    {
        //IEnumerator co = StartUnitTurn();
        //StopCoroutine(co);

        yield return new WaitForSeconds(GameManager.Instance.enemyAttackWaitTime);

        StartCoroutine(StartUnitTurn());
    }
    SkillData ChooseRandomSkill()
    {
        int rand = Random.Range(1, 4);

        if (rand == 1)  // Skill 1
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.Instance.GetActiveUnit().GetSkill1().skillEnergyCost)
                return GameManager.Instance.GetActiveUnit().GetSkill1();
            else
                return GameManager.Instance.GetActiveUnit().basicSkill;
        }
        else if (rand == 2)  // Skill 2
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.Instance.GetActiveUnit().GetSkill2().skillEnergyCost)
                return GameManager.Instance.GetActiveUnit().GetSkill2();
            else
                return GameManager.Instance.GetActiveUnit().basicSkill;
        }
        else if (rand == 3)  // Skill 3
        {
            if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitCurEnergy() >= GameManager.Instance.GetActiveUnit().GetSkill3().skillEnergyCost)
                return GameManager.Instance.GetActiveUnit().GetSkill3();
            else
                return GameManager.Instance.GetActiveUnit().basicSkill;
        }
        else
        {
            return null;
        }
    }

    public List<Effect> GetEffects()
    {
        return activeEffects;
    }

    public void AddUnitEffect(EffectData addedEffect, UnitFunctionality targetUnit, int turnDuration = 1)
    {
        int rand = Random.Range(0, 100);
        //Debug.Log(rand);

        // If unit is already effected with this effect, stop
        
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (addedEffect.effectName == activeEffects[i].effectName)
            {
                TriggerTextAlert(GameManager.Instance.GetActiveSkill().effect.effectName, 1, true, "Inflict");
                activeEffects[i].AddTurnCountText(turnDuration);
                return;
            }
        }
        
        // Determining whether the effect hits, If it fails, stop

        if (rand < GameManager.Instance.GetActiveSkill().effectHitChance)
            return;

        //ToggleUnitHealthBar(false);
        TriggerTextAlert(GameManager.Instance.GetActiveSkill().effect.effectName, 1, true, "Inflict");

        GameObject go = Instantiate(EffectManager.instance.effectPrefab, effectsParent.transform);
        go.transform.SetParent(effectsParent);
        //go.transform.position = new Vector3(0,0,0);
        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        go.transform.localScale = new Vector3(1, 1, 1);

        Effect effect = go.GetComponent<Effect>();
        activeEffects.Add(effect);
        effect.Setup(addedEffect, targetUnit, turnDuration);
    }

    public void ResetEffects()
    {
        activeEffects.Clear();
        for (int i = 0; i < effectsParent.childCount; i++)
        {
            Destroy(effectsParent.GetChild(i).gameObject);
        }
    }

    public void UpdateUnitProjectileSprite(Sprite sprite)
    {
        projectileSprite = sprite;
    }

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
        curPowerInc = newPowerInc;
    }

    public void ToggleTaunt(bool toggle)
    {
        isTaunting = toggle;
    }

    public void SpawnPowerUI(float power = 10f, bool isParrying = false, bool offense = false, Effect effect = null, int powerUICount = 1, bool isLuckyHit = false, bool isHeal = false)
    {
        if (!isLuckyHit)
        {
            // If this is NOT the first power text UI
            if (prevPowerUI != null)
                prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.Instance.powerUIHeightLvInc), Quaternion.identity);
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

        powerText.UpdateSortingOrder(powerUICount);

        // Otherwise, display the power
        powerText.UpdatePowerTextFontSize(GameManager.Instance.powerHitFontSize);

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
        go.transform.localPosition = new Vector3(0, 0, 0);

        Projectile projectile = go.GetComponent<Projectile>();
        projectile.UpdateProjectileSprite(projectileSprite);

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
    public bool CheckIfUnitIsDead()
    {
        // If unit's health is 0 or lower
        if (curHealth <= 0)
        {
            return true;
        }
        else
            return false;
    }

    IEnumerator EnsureUnitIsDead()
    {
        // If unit's health is 0 or lower
        if (curHealth <= 0 && !isDead)
        {
            isDead = true;

            curHealth = 0;

            animator.SetTrigger("DeathFlg");

            yield return new WaitForSeconds(1f);

            GameManager.Instance.RemoveUnit(this);
                        
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

            if (GetCurExp() >= GetMaxExp())
            {
                int remainingExp = gainedExp - i;
                UpdateUnitLevel(1, remainingExp);
                yield break;
            }
            else
            {
                IncreaseCurExp(1);
                unitExpBarImage.fillAmount = GetCurExp() / GetMaxExp();
            }
        }

        yield return new WaitForSeconds(GameManager.Instance.timePostExp);
        ToggleUnitExpVisual(false);
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
            temp = GameManager.Instance.maxExpStarting + (GameManager.Instance.expIncPerLv * (GetUnitLevel()-1));
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

    public void UpdateUnitCurHealth(int power, bool damaging = false)
    {
        if (isDead)
            return;

        float absPower = Mathf.Abs((float)power);

        // Damaging
        if (damaging)
        {
            //float tempPower;
            //tempPower = (curRecieveDamageAmp / 100f) * absPower;
            //float newPower = absPower + tempPower;
            curHealth -= (int)absPower;
            animator.SetTrigger("DamageFlg");
        }
        // Healing
        else
            curHealth += (int)absPower;

        UpdateUnitHealthVisual();
    }

    public void ToggleDamageAnimOff()
    {
        animator.SetBool("DamageFlg", false);
    }

    public void UpdateUnitMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        UpdateUnitHealthVisual();
    }

    void UpdateUnitHealthVisual()
    {
        ToggleUnitHealthBar(true);

        unitHealth.fillAmount = (float)curHealth / (float)maxHealth;

        if (CheckIfUnitIsDead())
            StartCoroutine(EnsureUnitIsDead());
    }

    public void ToggleUnitHealthBar(bool toggle)
    {
        if (toggle)
            healthBarUIElement.UpdateAlpha(1);
        else
            healthBarUIElement.UpdateAlpha(0);  
    }

    public void UpdateUnitSpeed(int newSpeed)
    {
        curSpeed = newSpeed;
    }
    
    public void UpdateUnitPower(int newPower)
    {
        curPower = newPower;
    }

    public void UpdateUnitArmor(int newArmor)
    {
        curArmor = newArmor;
    }
    
    public void AddUnitArmor(int armor)
    {
        curArmor += armor;
    }

    public void RemoveUnitArmor(int armor)
    {
        curArmor -= armor;
    }

    public void UpdateUnitHealth(int newCurHealth, int newMaxHealth)
    {
        UpdateUnitCurHealth(newCurHealth);
        UpdateUnitMaxHealth(newMaxHealth);
    }

    public float GetUnitCurHealth()
    {
        return curHealth;
    }

    public float GetUnitMaxHealth()
    {
        return maxHealth;
    }

    public float GetUnitCurEnergy()
    {
        return curEnergy;
    }

    public float GetUnitMaxEnergy()
    {
        return maxEnergy;
    }

    public void UpdateUnitEnergy(int curEnergy, int maxEnergy)
    {
        this.curEnergy = curEnergy;
        this.maxEnergy = maxEnergy;
    }

    public void UpdateMaxEnergy(int maxEnergy)
    {
        this.maxEnergy = maxEnergy;
    }

    public void UpdateUnitCurEnergy(int energy)
    {
        //effects.UpdateAlpha(1);

        this.curEnergy += energy;

        if (curEnergy > maxEnergy)
            curEnergy = maxEnergy;
    }

    public void UpdateUnitStartTurnEnergy(int newUnitStartTurnEnergyGain)
    {
        unitStartTurnEnergyGain = newUnitStartTurnEnergyGain;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

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
