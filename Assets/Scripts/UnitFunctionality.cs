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

    public ButtonFunctionality selectUnitButton;
    [SerializeField] private Color effectTitleColour;
    [SerializeField] private UIElement tooltipStats;
    [SerializeField] private UIElement tooltipEffect;
    [SerializeField] private UIElement statHealth;
    [SerializeField] private UIElement statPower;
    [SerializeField] private UIElement statHealingPower;
    [SerializeField] private UIElement statDefense;
    [SerializeField] private UIElement statSpeed;
    [SerializeField] private UIElement unitLevelImage;
    [SerializeField] private UIElement selectionCircle;
    [SerializeField] private UIElement unitVisuals;
    private Sprite unitIcon;
    [SerializeField] private Transform unitVisualsParent;
    [SerializeField] private Transform powerUIParent;
    [SerializeField] private UIElement statUI;
    [SerializeField] private UIElement hitsRemainingText;
    [SerializeField] private Image unitHealthBar;
    [SerializeField] private Image unitAttackBar;
    [SerializeField] private Image unitAttackBarNext;
    public UIElement healthBarUIElement;
    public UIElement attackBarUIElement;
    public UIElement unitAttackBarNextUIElement;

    public Image unitImage;
    public UIElement curUnitTurnArrow;
    [Tooltip("Healing amplification this unit recives. This value is multiplied by the healing recieved. 1 = normal, 0 = nothing, 2 = double")]
    public float curHealingRecieved = 1;
    public int curSpeed;
    public int curPower;
    public int curHealingPower;
    public float curDefense;
    [SerializeField] private int curHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int curLevel;

    public int curDamageHits;
    private int curHealingHits;
    private float curExp;
    private float maxExp;
    [SerializeField] private int curSpeedIncPerLv = 0;
    [SerializeField] private int curPowerIncPerLv = 0;
    [SerializeField] private int curHealingPowerIncPerLv = 0;
    [SerializeField] private float curDefenseIncPerLv = 0;
    [SerializeField] private int maxHealthIncPerLv = 0;
    public int startingSpeed;
    public int startingHealth;
    public int startingDamage;
    public int startingHealing;
    public int startingDefense;
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
    public List<Effect> activeEffects = new List<Effect>();
    public int curRecieveDamageAmp = 100;
    [SerializeField] private Color unitColour;

    [SerializeField] private List<ItemPiece> equiptItems = new List<ItemPiece>();

    [SerializeField] private List<SkillData> curSkillBaseSlots = new List<SkillData>();

    [SerializeField] private List<SkillData> startingSkills = new List<SkillData>();
    [SerializeField] private List<SkillData> skills = new List<SkillData>();

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

    public int spentMasteryTotalPoints = 0;
    public int SkillSpentPoints = 0;
    public int statsSpentAdvPoints = 0;

    [SerializeField] private int skill0CurCooldown = 0;
    [SerializeField] private int skill1CurCooldown = 0;
    [SerializeField] private int skill2CurCooldown = 0;
    [SerializeField] private int skill3CurCooldown = 0;

    public CombatUnitFocus unitFocus;

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
    public bool isPoison;
    public bool isPoisonLeaching;
    public int bonusDmgLines;
    public int bonusHealingLines;
    public int reducedCooldownsCount;
    public int rerollItemCount;

    public bool heroRoomUnit;

    SimpleFlash hitFlash;
    public UIElement uiElement;

    [HideInInspector]
    public AudioClip deathClip, hitRecievedClip;

    [HideInInspector]
    public int hitsRemaining;

    public int prevStatHealth;
    public int prevStatPower;
    public int prevStatHealingPower;
    public int prevStatDefense;
    public int prevStatSpeed;
    public string selectedEffect;

    //public bool unitDouble;

    public void ToggleTooltipStats(bool toggle)
    {
        //Debug.Log("toggling " + toggle);

        if (toggle)
        {
            //Debug.Log(tooltipStats.isEnabled);

            if (!tooltipStats.isEnabled)
            {
                UpdateTooltipStats();
                tooltipStats.UpdateAlpha(1);
            }
        }
        else
            tooltipStats.UpdateAlpha(0);
    }

    public void ToggleTooltipEffect(bool toggle, string effectSelectedName = null)
    {
        if (toggle)
        {
            if (!tooltipEffect.isEnabled)
            {
                //Debug.Log("enabling");
                tooltipEffect.isEnabled = true;

                tooltipEffect.UpdateAlpha(1);
                UpdateTooltipEffect(effectSelectedName);
            }
        }
        else
        {
            //Debug.Log("disabling");
            tooltipEffect.UpdateAlpha(0);
        }
    }

    public void UpdateTooltipStats()
    {
        statHealth.UpdateContentText(GetUnitCurHealth().ToString());
        statPower.UpdateContentText(curPower.ToString());
        statHealingPower.UpdateContentText(GetUnitHealingHits().ToString());
        statDefense.UpdateContentText(GetCurDefense().ToString());
        statSpeed.UpdateContentText(GetUnitSpeed().ToString());
    }

    public void UpdateTooltipEffect(string effectSelectedName)
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (effectSelectedName == activeEffects[i].effectName)
            {
                tooltipEffect.UpdateContentText(activeEffects[i].effectName);
                tooltipEffect.UpdateContentSubTextTMP(activeEffects[i].effectDesc);
                tooltipEffect.UpdateContentTextColourTMP(activeEffects[i].titleTextColour);
            }
        }
    }

    public void UpdateHitsRemainingText(int remaining)
    {
        hitsRemainingText.UpdateContentText(remaining.ToString());
        hitsRemainingText.AnimateUI();
    }

    public void ToggleHitsRemainingText(bool toggle)
    {
        if (toggle)
        {
            hitsRemainingText.UpdateAlpha(1);
        }
        else
        {
            hitsRemainingText.UpdateAlpha(0);
        }
    }

    public void UpdateSpeed(float newVal)
    {
        curSpeed += (int)newVal;
    }
    public void UpdatePower(float newVal)
    {
        curPower += (int)newVal;
    }
    public void UpdateHealingPower(int newPower, bool setting = true, bool adding = true)
    {
        if (adding)
        {
            if (setting)
                curHealingPower = newPower;
            else
                curHealingPower += newPower;
        }
        else
        {
            if (setting)
                curHealingPower = newPower;
            else
                curHealingPower -= newPower;
        }
    }
    public void UpdateDefense(float newVal)
    {
        curDefense += newVal;
    }
    public void UpdateMaxHealth(float newVal)
    {
        maxHealth += (int)newVal;
    }

    public int GetSpeedIncPerLv()
    {
        return curSpeedIncPerLv;
    }
    public void UpdateSpeedIncPerLv(int newVal)
    {
        curSpeedIncPerLv += newVal;
    }

    public int GetPowerIncPerLv()
    {
        return curPowerIncPerLv;
    }
    public void UpdatePowerIncPerLv(int newVal)
    {
        curPowerIncPerLv += newVal;
    }

    public int GetHealingPowerIncPerLv()
    {
        return curHealingPowerIncPerLv;
    }
    public void UpdateHealingPowerIncPerLv(int newVal)
    {
        curHealingPowerIncPerLv += newVal;
    }

    public float GetDefenseIncPerLv()
    {
        return curDefenseIncPerLv;
    }
    public void UpdateDefenseIncPerLv(float newVal)
    {
        curDefenseIncPerLv += newVal;
    }

    public int GetMaxHealthIncPerLv()
    {
        return maxHealthIncPerLv;
    }
    public void UpdateMaxHealthIncPerLv(int newVal)
    {
        maxHealthIncPerLv += newVal;
    }
    public void StartFocusUnit()
    {
        unitFocus.FocusUnit();
    }

    public void IncCooldownReducBonus()
    {
        reducedCooldownsCount++;
    }

    public void RerollItemCount()
    {
        rerollItemCount++;
    }

    public int GetCooldownReducBonus()
    {
        return reducedCooldownsCount;
    }

    public int GetRerollItemCount()
    {
        return rerollItemCount;
    }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // If player is holding down on unit, display stats


    }

    public void ResetUnitSkillOrder()
    {
        //GetSkillBaseSlot(0).
    }

    private void Start()
    {
        ToggleUnitExpVisual(false);
        ToggleUnitBG(false);
        ResetEffects();
        ToggleHitsRemainingText(false);

        //UpdateUnitPowerInc(1);
        //sUpdateUnitHealingPowerInc(1);

        if (!heroRoomUnit)
        {
            //Debug.Log("ddd");
            UpdateIsVisible(true);
        }

        UpdateUnitLevelImage();
        ToggleUnitLevelImage(true);
        //ToggleIsPoisonLeaching(false);

        SetupFlashHit();
    }

    void SetupFlashHit()
    {
        hitFlash = GetComponentInChildren<SimpleFlash>();
        uiElement = hitFlash.gameObject.GetComponent<UIElement>();
    }

    public SimpleFlash GetHitFlash()
    {
        return hitFlash;
    }
    public void ToggleUnitPoisoned(bool toggle)
    {
        isPoison = toggle;
    }

    public bool GetUnitPoisoned()
    {
        return isPoison;
    }

    public float GetUnitHealingRecieved()
    {
        return curHealingRecieved;
    }

    public void UpdateUnitHealingRecieved(float newVal)
    {
        curHealingRecieved = newVal;
    }

    public void ResetUnitHealingRecieved()
    {
        curHealingRecieved = 1;
    }

    public void ToggleUnitLevelImage(bool toggle)
    {
        UpdateUnitLevelImage();

        if (unitLevelImage == null)
            return;

        if (toggle)
            unitLevelImage.UpdateAlpha(1);
        else
            unitLevelImage.UpdateAlpha(0);
    }

    public void UpdateUnitLevelColour(Color color)
    {
        unitLevelImage.UpdateContentSubTextTMPColour(color);
    }

    public void UpdateUnitLevelImage()
    {
        if (unitLevelImage == null)
            return;

        unitLevelImage.UpdateContentSubTextTMP(curLevel.ToString());
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
            //Debug.Log(toggle);
            if (unitUIElement == null)
                return;

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
    public void AddOwnedItems(ItemPiece item)
    {
        equiptItems.Add(item);
    }

    public List<ItemPiece> GetEquipItems()
    {
        return equiptItems;
    }

    public void UpdateCurrentSkills(List<SkillData> skillBaseSlots)
    {
        this.curSkillBaseSlots = skillBaseSlots;
    }

    public void UpdateUnitSkills(List<SkillData> skills)
    {
        //this.skills = skills;

        /*
        this.skills.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (skills[i].originalIndex == 0)
                skills.Insert(0, skills[i]);
            else if (skills[i].originalIndex == 1)
                skills.Insert(1, skills[i]);
            else if (skills[i].originalIndex == 2)
                skills.Insert(2, skills[i]);
            else if (skills[i].originalIndex == 3)
                skills.Insert(3, skills[i]);
        }
        */

        this.skills = skills;
    }

    public SkillData GetBaseSelectSkill()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].skillCooldown == 0)
            {
                return skills[i];
            }
        }

        return skills[0];
    }

    public SkillData GetSkill(int count)
    {
        if (skills.Count == 0)
            return null;
        else
            return skills[count];
    }

    public List<SkillData> GetAllSkills()
    {
        return skills;
    }

    public void ClearSkillBaseSlots()
    {
        curSkillBaseSlots.Clear();
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

    public void ToggleIsPoisonLeaching(bool toggle)
    {
        isPoisonLeaching = toggle;
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

    public int GetSpentSkillPoints()
    {
        return spentMasteryTotalPoints;
    }

    public void UpdateSpentSkillPoints(int add)
    {
        spentMasteryTotalPoints += add;
    }

    public void ResetSpentStatPoints()
    {
        spentMasteryTotalPoints = 0;
        SkillSpentPoints = 0;
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

            if (GameManager.Instance.GetActiveSkill().curAnimType == SkillData.SkillAnimType.DEFAULT)
            {
                animator.SetTrigger("AttackFlg");
            }
            else if (GameManager.Instance.GetActiveSkill().curAnimType == SkillData.SkillAnimType.SKILL)
            {
                animator.SetTrigger("SkillFlg");
            }

            // Adjust power based on skill effect amp on target then send it 

            int totalPower = GameManager.Instance.activeSkill.GetCalculatedSkillPowerStat() + GameManager.Instance.GetActiveUnitFunctionality().curPower;

            //totalPower += //GameManager.Instance.randomBaseOffset*2;
            totalPower = GameManager.Instance.RandomisePower(totalPower);

            if (GameManager.Instance.activeSkill.curSkillPower == 0)
                totalPower = 0;

            int effectCount;

            if (GameManager.Instance.GetActiveSkill().baseEffectApplyCount == 0)
                effectCount = 1;
            else
                effectCount = GameManager.Instance.GetActiveSkill().baseEffectApplyCount;

            int skillAttackCount;

            if (GameManager.Instance.GetActiveSkill().curSkillType == SkillData.SkillType.OFFENSE)
                skillAttackCount = GameManager.Instance.GetActiveSkill().skillAttackCount + GetUnitDamageHits();
            else
                skillAttackCount = GameManager.Instance.GetActiveSkill().skillAttackCount + GetUnitHealingHits();

            StartCoroutine(GameManager.Instance.WeaponAttackCommand(totalPower, skillAttackCount, effectCount));

            /*
            // End turn 
            GameManager.Instance.ToggleEndTurnButton(false);
            GameManager.Instance.UpdateTurnOrder();
            yield break;
            */
        }
    }

    public IEnumerator DecreaseEffectTurnsLeft(bool turnStart, bool parry = false)
    {
        // If no effects remain on the unit, stop
        if (activeEffects.Count >= 1)
        {
            if (activeEffects[0] == null)
                yield return null;
        }

        for (int i = 0; i < activeEffects.Count; i++)
        {

            if (parry)
            {
                if (activeEffects[i].curEffectName == Effect.EffectName.PARRY)
                {
                    activeEffects[i].TriggerPowerEffect(this);
                    TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                    activeEffects[i].ReduceTurnCountText(this);
                    //return;
                }
            }

            if (turnStart)
            {
                if (activeEffects[i].curEffectTrigger == Effect.EffectTrigger.TURNSTART)
                {
                    activeEffects[i].TriggerPowerEffect(this);
                    TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                    activeEffects[i].ReduceTurnCountText(this);

                    /*
                    if (activeEffects[i].GetTurnCountRemaining() == 1)
                    {
                        i--;
                        if (i < 0)
                            i = 0;
                    }
                    */

                    yield return new WaitForSeconds(.75f);
                }
            }
            else
            {
                if (activeEffects[i].curEffectTrigger == Effect.EffectTrigger.TURNEND)
                {
                    activeEffects[i].TriggerPowerEffect(this);
                    TriggerTextAlert(activeEffects[i].effectName, 1, true, "Trigger");
                    activeEffects[i].ReduceTurnCountText(this);
                }
            }
        }

        // If effect was removed, remove it here after all done
        for (int x = 0; x < activeEffects.Count; x++)
        {
            if (activeEffects[x] == null)
                GetEffects().RemoveAt(x);
        }
    }


    public IEnumerator UnitEndTurn()
    {
        yield return new WaitForSeconds(GameManager.Instance.enemyAttackWaitTime);

        // End turn
        GameManager.Instance.UpdateTurnOrder();
    }

    public List<SkillData> GetStartingSkills()
    {
        return startingSkills;
    }

    public void SetStartingSkills(List<SkillData> skills)
    {
        List<SkillData> newStartingSkills = skills;
        startingSkills.Clear();

        for (int i = 0; i < skills.Count; i++)
        {
            //skills[i]
        }
        startingSkills = newStartingSkills;
    }

    public SkillData GetNoCDSkill()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].skillCooldown == 0)
            {
                return skills[i];
            }
        }

        return null;
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
        GameManager.Instance.skill0IconCooldownUIText.UpdateUIText(skill0CurCooldown.ToString());
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
        skill0CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0).skillCooldown + 1;
        GameManager.Instance.skill0IconCooldownUIText.UpdateUIText(skill0CurCooldown.ToString());
    }
    public void SetSkill1CooldownMax()
    {
        skill1CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1).skillCooldown + 1;
        GameManager.Instance.skill1IconCooldownUIText.UpdateUIText((skill1CurCooldown).ToString());
    }
    public void SetSkill2CooldownMax()
    {
        skill2CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2).skillCooldown + 1;
        GameManager.Instance.skill2IconCooldownUIText.UpdateUIText((skill2CurCooldown).ToString());
    }
    public void SetSkill3CooldownMax()
    {
        skill3CurCooldown = GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3).skillCooldown + 1;
        GameManager.Instance.skill3IconCooldownUIText.UpdateUIText((skill3CurCooldown).ToString());
    }

    public void DecreaseSkill0Cooldown()
    {
        skill0CurCooldown--;

        if (skill0CurCooldown <= 0)
            skill0CurCooldown = 0;

        GameManager.Instance.skill0IconCooldownUIText.UpdateUIText(skill0CurCooldown.ToString());
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
        Debug.Log("skill 1 cd " + skill0CurCooldown);
        Debug.Log("skill 2 cd " + skill1CurCooldown);
        Debug.Log("skill 3 cd " + skill2CurCooldown);
        Debug.Log("skill 4 cd " + skill3CurCooldown);

        int unitEnemyIntelligence = 10;
        for (int i = 0; i < unitEnemyIntelligence; i++)
        {
            int rand = Random.Range(1, 5);
            //Debug.Log(rand);
            if (rand == 1)  // Skill 1
            {
                if (skill1CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1).isPassive)
                    return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(1);
                else
                    continue;
            }
            else if (rand == 2)  // Skill 2
            {
                if (skill2CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2).isPassive)
                    return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2);
                else
                    continue;
            }
            else if (rand == 3)  // Skill 3
            {
                if (skill3CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(3).isPassive)
                    return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(2);
                else
                    continue;
            }
            // Base skill
            else if (rand == 4)
            {
                if (skill0CurCooldown == 0 && !GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0).isPassive)
                    return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0);
            }
        }

        return GameManager.Instance.GetActiveUnitFunctionality().GetSkill(0);
    }

    public List<Effect> GetEffects()
    {
        return activeEffects;
    }

    public Effect GetEffect(string name)
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i].effectName == name)
                return activeEffects[i];
        }

        return null;
    }

    public void AddUnitEffect(EffectData addedEffect, UnitFunctionality targetUnit, int turnDuration = 1, int effectHitAcc = -1, bool byPassAcc = true)
    {
        // If player miss, do not apply effect
        if (effectHitAcc == 0 || targetUnit.isParrying)
            return;

        // If unit is already effected with this effect, add to the effect
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (addedEffect.effectName == activeEffects[i].effectName)
            {
                // Determining whether the effect hits, If it fails, stop
                // Add more stacks to the effect that the unit already has
                for (int x = 0; x < effectHitAcc; x++)
                {
                    // Determining whether the effect hits, If it fails, stop
                    if (GameManager.Instance.GetActiveSkill().curEffectHitChance != 0 && byPassAcc)
                    {
                        int rand = Random.Range(1, 101);
                        if (rand <= GameManager.Instance.GetActiveSkill().GetCalculatedSkillEffectStat())
                        {
                            // Cause Effect. Do not trigger text alert if its casting a skill on self. (BECAUSE: Skill announce overtakes this).
                            activeEffects[i].AddTurnCountText(1);
                            TriggerTextAlert(addedEffect.effectName, 1, true, "Inflict");
                        }
                        else
                            continue;
                    }
                    else
                    {
                        activeEffects[i].AddTurnCountText(1);
                        TriggerTextAlert(addedEffect.effectName, 1, true, "Inflict");
                    }
                }
            }
        }

        // If unit DOES NOT currently have this effect, create it, and start the loop
        if (GetEffect(addedEffect.effectName) == null)
        {
            for (int m = 0; m < effectHitAcc; m++)
            {
                GameObject go = null;

                // Determining whether the effect hits, If it fails, stop
                if (GameManager.Instance.GetActiveSkill().curEffectHitChance != 0 && byPassAcc)
                {
                    if (m == 0)
                    {
                        int rand = Random.Range(1, 101);
                        if (rand <= GameManager.Instance.GetActiveSkill().GetCalculatedSkillEffectStat())
                        {
                            // Spawn new effect on target unit
                            go = Instantiate(EffectManager.instance.effectPrefab, effectsParent.transform);
                            go.transform.SetParent(effectsParent);
                            go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                            go.transform.localScale = new Vector3(1, 1, 1);

                            Effect effect = go.GetComponent<Effect>();
                            activeEffects.Add(effect);
                            effect.Setup(addedEffect, targetUnit, 1);

                            TriggerTextAlert(addedEffect.effectName, 1, true, "Inflict");

                            AddUnitEffect(addedEffect, targetUnit, 1, effectHitAcc - 1);
                            break;
                        }
                    }
                    else
                        continue;

                }
                else
                {
                    if (m == 0)
                    {
                        // Spawn new effect on target unit
                        go = Instantiate(EffectManager.instance.effectPrefab, effectsParent.transform);
                        go.transform.SetParent(effectsParent);
                        go.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                        go.transform.localScale = new Vector3(1, 1, 1);

                        Effect effect = go.GetComponent<Effect>();
                        activeEffects.Add(effect);
                        effect.Setup(addedEffect, targetUnit, 1);

                        TriggerTextAlert(addedEffect.effectName, 1, true, "Inflict");

                        AddUnitEffect(addedEffect, targetUnit, 1, effectHitAcc - 1);
                        break;
                    }
                }
            }
        }

    }

    public void ResetEffects()
    {
        activeEffects.Clear();

        if (effectsParent == null)
            return;

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
        prevPowerUI = null;
    }

    public IEnumerator SpawnPowerUI(float power = 10f, bool isParrying = false, bool offense = false, Effect effect = null, bool isBlocked = false)
    {
        if (GameManager.Instance.GetActiveSkill() == null)
            yield break;

        if (offense)
        {
            if (effect != null)
            {
                // If posion is about to tick, allow all other units to leach if they can
                if (effect.effectName == "POISON")
                {
                    // Loop through all units in current room
                    for (int i = 0; i < GameManager.Instance.activeRoomAllUnitFunctionalitys.Count; i++)
                    {
                        UnitFunctionality targetUnit = GameManager.Instance.activeRoomAllUnitFunctionalitys[i];

                        // If unit is able to leach, give them effects they should get.
                        if (targetUnit.isPoisonLeaching)
                        {
                            targetUnit.AddUnitEffect(EffectManager.instance.GetEffect("HEALTH UP"), targetUnit, 1, 1, false);

                            // Spawn new effect on target unit
                            targetUnit.AddUnitEffect(EffectManager.instance.GetEffect("RECOVER"), targetUnit, 1, 1, false);

                            targetUnit.TriggerTextAlert("Poison Leach", 1, false);
                        }
                    }
                }
            }
        }

        // Play Audio
        if (offense)
        {
            if (GameManager.Instance.GetActiveSkill().skillHit != null)
                AudioManager.Instance.Play(GameManager.Instance.GetActiveSkill().skillHit.name);

            if (effect != null)
            {
                if (effect.curEffectName == Effect.EffectName.BLEED)
                    AudioManager.Instance.Play("Bleed");
                else if (effect.curEffectName == Effect.EffectName.POISON)
                    AudioManager.Instance.Play("Poison");
            }
        }
        else
        {
            AudioManager.Instance.Play("Heal");
        }


        // LAST power UI hit
        if (damageCount >= GameManager.Instance.maxPowerUICount || healCount >= GameManager.Instance.maxPowerUICount)
        {
            if (offense)
                damageCount = 1;
            else
                healCount = 1;

            prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, powerUIParent.position, Quaternion.identity);
            prevPowerUI.transform.SetParent(powerUIParent);
            prevPowerUI.transform.localScale = Vector2.one;
        }
        // Starting power UI
        else if (damageCount == 0 && offense || healCount == 0 && !offense)
        {
            prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, powerUIParent.position, Quaternion.identity);
            prevPowerUI.transform.SetParent(powerUIParent);
            prevPowerUI.transform.localScale = Vector2.one;

            if (offense)
                damageCount++;
            else
                healCount++;
        }
        // Subsequent power UI hits
        else if (damageCount != 0 && offense || healCount != 0 && !offense)
        {
            if (offense)
            {
                // If power UI count has been reached from heal / damage, reset back to original Y position.

                prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.Instance.powerUIHeightLvInc), Quaternion.identity);
                damageCount++;

                prevPowerUI.transform.SetParent(powerUIParent);
                prevPowerUI.transform.localScale = Vector2.one;
            }
            else
            {
                // If power UI count has been reached from heal / damage, reset back to original Y position.
                prevPowerUI = Instantiate(GameManager.Instance.powerUITextPrefab, prevPowerUI.transform.position + new Vector3(0, GameManager.Instance.powerUIHeightLvInc), Quaternion.identity);
                healCount++;

                prevPowerUI.transform.SetParent(powerUIParent);
                prevPowerUI.transform.localScale = Vector2.one;
            }
        }

        prevPowerUI.transform.SetParent(powerUIParent);
        prevPowerUI.transform.localScale = Vector2.one;

        PowerText powerText = prevPowerUI.GetComponent<PowerText>();

        if (prevPowerUI == null)
        {
            yield break;
        }

        if (powerText != null)
        {
            if (isParrying || isBlocked)
            {
                powerText.UpdatePowerTextFontSize(GameManager.Instance.powerSkillParryFontSize);
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillParry);
                powerText.UpdatePowerText(GameManager.Instance.parryPowerText);
                yield break;
            }

            // If power is 0, display that it missed
            if (power <= 0)
            {
                //Debug.Log(power);
                powerText.UpdatePowerTextFontSize(GameManager.Instance.powerMissFontSize);
                powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillMiss);
                powerText.UpdatePowerText(GameManager.Instance.missPowerText);
                //powerText.UpdatePowerText(power.ToString());   // Update Power Text
                yield break;
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
                if (offense)
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

                if (effect.curEffectName == Effect.EffectName.RECOVER && !offense)
                    powerText.UpdatePowerTextColour(GameManager.Instance.gradientSkillSupport);
            }

            int finalPower = (int)power;
            powerText.UpdatePowerText(finalPower.ToString());   // Update Power Text
        }
        else
            Destroy(prevPowerUI.gameObject);
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

    public void ResetDamageHealCount()
    {
        healCount = 0;
        damageCount = 0;
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

            ResetEffects();

            curHealth = 0;

            animator.SetTrigger("DeathFlg");

            GameManager.Instance.RemoveUnit(this);

            AudioManager.Instance.Play(deathClip.name);

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
        if (unitBg == null)
            return;

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

                // Level up SFX
                AudioManager.Instance.Play("LevelUp");

                // level up bonus
                UpdatePower(GetPowerIncPerLv());
                UpdateHealingPower(GetHealingPowerIncPerLv());
                UpdateSpeed(GetSpeedIncPerLv());
                UpdateDefense(GetDefenseIncPerLv());
                UpdateMaxHealth(GetMaxHealthIncPerLv());

                float levelUpHeal = ((float)GameManager.Instance.levelupHealPerc / 100f) * GetUnitMaxHealth();

                ResetPowerUI();

                UpdateUnitCurHealth((int)levelUpHeal, false, false);
                SpawnPowerUI(levelUpHeal, false, false, null, false);

                // This isnt working
                //SpawnPowerUI(levelUpHeal, false, false, null, false);

                UpdateUnitDamageHits(1, true);
                UpdateUnitHealingHits(1, true);

                UpdateUnitLevelImage();
                yield break;
            }
            else
            {
                IncreaseCurExp(1);
                unitExpBarImage.fillAmount = GetCurExp() / GetMaxExp();
            }
        }

        yield return new WaitForSeconds(GameManager.Instance.timePostExp);

        // Enable post battle to map button for next post battle scene
        StartCoroutine(PostBattle.Instance.ToggleButtonPostBattleMap(true));
    }

    public void ToggleUnitExpVisual(bool toggle)
    {
        if (unitExpBar == null)
            return;

        if (toggle)
            unitExpBar.UpdateAlpha(1);
        else
            unitExpBar.UpdateAlpha(0);
    }

    public void UpdateUnitLevel(int level, int extraExp = 0, bool set = false)
    {
        if (!set)
            curLevel += level;
        else
            curLevel = level;

        UpdateUnitLevelVisual(curLevel);

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
        //Debug.Log(level);
        unitLevelText.text = level.ToString();
    }

    public int GetUnitLevel()
    {
        return curLevel;
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

    public void UpdateUnitCurHealth(int power, bool damaging = false, bool setHealth = false, bool doExtras = true)
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
                    // Damage formula - Defense. Negate some of the damage
                    //float temp = ((GetCurDefense() * GetCurDefenseInc()) / 100f) * absPower;
                    //Debug.Log(temp);
                    //float powerIncArmor = absPower - temp;

                    curHealth -= (int)absPower;
                }


                if (curHealth < 0)
                    curHealth = 0;

                //if (curHealth > 0)

                // If the hit wasnt a miss, or 0 dmg, cause hit recieved animation
                if (power != 0)
                {
                    animator.SetBool("DamageFlg", true);

                    GetHitFlash().Flash();
                    uiElement.AnimateUI(false);
                    
                    if (doExtras)
                    {
                        CameraShake.instance.EnableCanShake();
                        AudioManager.Instance.Play(hitRecievedClip.name);
                    }
                }
                StartCoroutine(PlayIdleAnimation());
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

    IEnumerator PlayIdleAnimation()
    {
        yield return new WaitForSeconds(1.25f);
        ToggleDamageAnimOff();
    }
    public void ToggleDamageAnimOff()
    {
        animator.SetBool("DamageFlg", false);
    }

    public void UpdateUnitMaxHealth(int newMaxHealth, bool set = false, bool inc = true)
    {
        //maxHealth = newMaxHealth;

        if (inc)
        {
            if (set)
            {
                maxHealth = newMaxHealth;
                curHealth = maxHealth;
            }

            else
                maxHealth += newMaxHealth;
        }
        else
        {
            if (set)
                maxHealth = newMaxHealth;
            else
                maxHealth -= newMaxHealth;
        }

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

        attackChargeTurnStart = 0;

        UpdateUnitAttackBarVisual();
        UpdateUnitAttackBarNextVisual();
    }

    public void ResetAttackChargeTurnStart()
    {
        attackChargeTurnStart = 0;
    }
    public void UpdateUnitCurAttackCharge()
    {
        ResetAttackChargeTurnStart();

        CalculateUnitAttackChargeTurnStart();

        // If unit is player, give more exp the lower allies there are on team
        if (curUnitType == UnitType.PLAYER)
        {
            if (GameManager.Instance.activeRoomAllies.Count == 1)
                attackChargeTurnStart *= 6;
            else if (GameManager.Instance.activeRoomAllies.Count == 2)
                attackChargeTurnStart *= 4;
            else if (GameManager.Instance.activeRoomAllies.Count == 3)
                attackChargeTurnStart *= 2;
        }
        // If unit is enemy, give more exp the lower allies there are on team
        else
        {
            if (GameManager.Instance.activeRoomEnemies.Count == 1)
                attackChargeTurnStart *= 7;
            else if (GameManager.Instance.activeRoomEnemies.Count == 2)
                attackChargeTurnStart *= 6;
            else if (GameManager.Instance.activeRoomEnemies.Count == 3)
                attackChargeTurnStart *= 5;
            else if (GameManager.Instance.activeRoomEnemies.Count == 4)
                attackChargeTurnStart *= 4;
            else if (GameManager.Instance.activeRoomEnemies.Count == 5)
                attackChargeTurnStart *= 3;
            else if (GameManager.Instance.activeRoomEnemies.Count == 6)
                attackChargeTurnStart *= 2;
        }

        attackChargeTurnStart /= 3;

        //Debug.Log(GetUnitName() + " 's attack charge = " + attackChargeTurnStart);
        
        curAttackCharge += attackChargeTurnStart;

        if (curAttackCharge > 100)
            curAttackCharge = 100;

        UpdateUnitAttackBarVisual();
        UpdateUnitAttackBarNextVisual();
    }

    void UpdateUnitAttackBarVisual()
    {
        ToggleUnitAttackBar(true);

        unitAttackBar.fillAmount = (float)curAttackCharge / 100f;
    }
    void UpdateUnitAttackBarNextVisual()
    {
        //ToggleUnitAttackBar(true);

        unitAttackBarNext.fillAmount = unitAttackBar.fillAmount + ((float)attackChargeTurnStart / 100f);
    }

    public void ToggleUnitHealthBar(bool toggle)
    {
        if (toggle)
            healthBarUIElement.UpdateAlpha(1);
        else
            healthBarUIElement.UpdateAlpha(0);  
    }

    public void ToggleActionNextBar(bool toggle)
    {
        if (toggle)
            unitAttackBarNextUIElement.UpdateAlpha(1);
        else
            unitAttackBarNextUIElement.UpdateAlpha(0);
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

    public void UpdateUnitSpeedChange(int newSpeed, bool inc)
    {
        if (inc)
            curSpeed += newSpeed;
        else
            curSpeed -= newSpeed;
    }

    public int GetUnitSpeed()
    {
        return curSpeed;
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

    public void UpdateUnitPower(int newPower, bool setting = true, bool adding = true)
    {
        if (adding)
        {
            if (setting)
                curPower = newPower;
            else
                curPower += newPower;
        }
        else
        {
            if (setting)
                curPower = newPower;
            else
                curPower -= newPower;
        }
    }

    public void UpdateUnitHealingPower(int newHealingPower)
    {
        curHealingPower = newHealingPower;
    }

   
    public void UpdateUnitDefense(int newDefense)
    {
        curDefense = newDefense;
    }

    public void UpdateUnitDefenseChange(int newDef, bool inc)
    {
        if (inc)
            curDefense += newDef;
        else
            curDefense -= newDef;
    }

    public float GetCurDefense()
    {
        return curDefense;
    }
    
    public void UpdateUnitDamageHits(int newDmgHits, bool inc = true)
    {
        if (inc)
            curDamageHits += newDmgHits;
        else
            curDamageHits -= newDmgHits;
    }

    public int GetUnitDamageHits()
    {
        return curDamageHits;
    }

    public void UpdateUnitHealingHits(int newHealHits, bool inc = true)
    {
        if (inc)
            curHealingHits += newHealHits;
        else
            curHealingHits -= newHealHits;
    }

    public int GetUnitHealingHits()
    {
        return curHealingHits;
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
