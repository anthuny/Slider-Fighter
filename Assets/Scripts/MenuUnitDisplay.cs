using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUnitDisplay : MonoBehaviour
{

    [SerializeField] private UIElement raceIcon;
    [SerializeField] private UIElement fallenHeroCostText;
    public Animator animator;
    [SerializeField] private Image unitImage;
    public string unitName;
    [SerializeField] private UIElement unitQuestionMarkImage;
    [SerializeField] private bool unitLocked;
    [SerializeField] private UIElement unitLevel;

    [Header("Stats")]
    [SerializeField] private UIElement statHealth;
    [SerializeField] private UIElement statPower;
    [SerializeField] private UIElement statHealingPower;
    [SerializeField] private UIElement statDefense;
    [SerializeField] private UIElement statSpeed;

    public int cost;

    public void UpdateFighterRaceIcon(string fighterRace)
    {
        if (fighterRace == "HUMAN")
        {
            raceIcon.UpdateContentImage(GameManager.Instance.humanRaceIcon);
            raceIcon.tooltipStats.UpdateTooltipStatsText("HUMAN");
        }           
        else if (fighterRace == "BEAST")
        {
            raceIcon.UpdateContentImage(GameManager.Instance.beastRaceIcon);
            raceIcon.tooltipStats.UpdateTooltipStatsText("BEAST");
        }          
        else if (fighterRace == "ETHEREAL")
        {
            raceIcon.UpdateContentImage(GameManager.Instance.etherealRaceIcon);
            raceIcon.tooltipStats.UpdateTooltipStatsText("ETHEREAL");
        }       
    }

    public void ToggleFighterRaceIcon(bool toggle = true)
    {
        if (toggle)
        {
            raceIcon.UpdateAlpha(1);
        }
        else
        {
            raceIcon.UpdateAlpha(0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ToggleUnitLevelImage(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFallenHeroCost(int cost)
    {
        this.cost = cost;

        fallenHeroCostText.UpdateContentText(cost.ToString());
    }

    public void UpdateFallenHeroCostColour()
    {
        if (cost <= ShopManager.Instance.GetPlayerGold())
        {
            // turn text normal
            fallenHeroCostText.UpdateContentTextColour(ShopManager.Instance.shopItemCostAllow);
        }
        else
        {
            // turn text red
            fallenHeroCostText.UpdateContentTextColour(ShopManager.Instance.shopItemCostDeny);
        }
    }

    public void ToggleUnitLevelImage(bool toggle, int newLevel = 0)
    {
        if (unitLevel == null)
            return;

        if (toggle)
            unitLevel.UpdateAlpha(1);
        else
            unitLevel.UpdateAlpha(0);

        unitLevel.UpdateContentSubTextTMP(newLevel.ToString());
    }

    public void UpdateUnitDisplay(string unitName, bool fallenFighter = false)
    {
        this.unitName = unitName;

        if (unitName == "Knight")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab || ShopManager.Instance.playerInShopRoom)
            {
                if (ShopManager.Instance.playerInShopRoom)
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 4.087784f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(10, -57);
                }
                else
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(3.5f, 3.5f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -72);
                }
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 4.087784f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(10, 0);
            }

            if (!fallenFighter)
                UpdateFighterRaceIcon("HUMAN");
        }

        else if (unitName == "Necromancer")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.necromancerAnimator;

            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab || ShopManager.Instance.playerInShopRoom)
            {
                if (ShopManager.Instance.playerInShopRoom)
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -21);
                }
                else
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.5f, 4.5f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -40);
                }
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(5, 55);
            }

            if (!fallenFighter)
                UpdateFighterRaceIcon("ETHEREAL");
        }
        else if (unitName == "Ranger")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.archerAnimator;

            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab || ShopManager.Instance.playerInShopRoom)
            {
                if (ShopManager.Instance.playerInShopRoom)
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 5.8f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-5, -17);
                }
                else
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(3.6f, 5f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(15, -25);
                }
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 4.087784f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-5, 55);
            }

            if (!fallenFighter)
                UpdateFighterRaceIcon("HUMAN");
        }

        else if (unitName == "Cleric")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.clericAnimator;

            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab || ShopManager.Instance.playerInShopRoom)
            {
                if (ShopManager.Instance.playerInShopRoom)
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -21);
                }
                else
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.5f, 4.5f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -40);
                }
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, 55);
            }

            if (!fallenFighter)
                UpdateFighterRaceIcon("HUMAN");
        }

        else if (unitName == "Monk")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.monkAnimator;

            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab || ShopManager.Instance.playerInShopRoom)
            {
                if (ShopManager.Instance.playerInShopRoom)
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -21);
                }
                else
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.5f, 4.5f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -40);
                }
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(5, 5);
            }

            if (!fallenFighter)
                UpdateFighterRaceIcon("ETHEREAL");
        }

        else if (unitName == "Dragonborn")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.dragonbornAnimator;

            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab || ShopManager.Instance.playerInShopRoom)
            {
                if (ShopManager.Instance.playerInShopRoom)
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(-15f, 15f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, -65);
                }
                else
                {
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(-15f, 15f);
                    animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, -65);
                }
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(-15f, 15f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-13f, -25);
            }

            if (!fallenFighter)
                UpdateFighterRaceIcon("BEAST");
        }

        else if (unitName == "Locked")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
            // Adjust size of unit
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 4.087784f);
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

            ToggleUnitLocked(true, true);

            ToggleFighterRaceIcon(false);
        }

        StartIdleAnim();
    }

    public void ResetUnitStats()
    {
        for (int i = 0; i < GameManager.Instance.activeRoomHeroes.Count; i++)
        {
            GameManager.Instance.activeRoomHeroes[i].prevStatHealth = 0;
            GameManager.Instance.activeRoomHeroes[i].prevStatPower = 0;
            GameManager.Instance.activeRoomHeroes[i].prevStatHealingPower = 0;
            GameManager.Instance.activeRoomHeroes[i].prevStatDefense = 0;
            GameManager.Instance.activeRoomHeroes[i].prevStatSpeed = 0;
        }
    }

    public void UpdateUnitStats(UnitFunctionality unitFunc)
    {
        // If new gear has a higher stat then current, make the stat bounce

        if (unitFunc.prevStatHealth < unitFunc.GetUnitMaxHealth())
        {
            statHealth.AnimateUI();
            StartCoroutine(statHealth.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (unitFunc.prevStatPower < unitFunc.curPower)
        {
            statPower.AnimateUI();
            StartCoroutine(statPower.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (unitFunc.prevStatHealingPower < unitFunc.curHealingPower)
        {
            statHealingPower.AnimateUI();
            StartCoroutine(statHealingPower.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (unitFunc.prevStatDefense < unitFunc.GetCurDefense())
        {
            statDefense.AnimateUI();
            StartCoroutine(statDefense.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (unitFunc.prevStatSpeed < unitFunc.GetUnitSpeed())
        {
            statSpeed.AnimateUI();
            StartCoroutine(statSpeed.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }


        // Decrease stat - colour behaviour
        if (unitFunc.prevStatHealth > unitFunc.GetUnitMaxHealth())
        {
            //statHealth.AnimateUI();
            StartCoroutine(statHealth.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (unitFunc.prevStatPower > unitFunc.curPower)
        {
            //statDamageHits.AnimateUI();
            StartCoroutine(statPower.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (unitFunc.prevStatHealingPower > unitFunc.curHealingPower)
        {
            //statHealingHits.AnimateUI();
            StartCoroutine(statHealingPower.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (unitFunc.prevStatDefense > unitFunc.GetCurDefense())
        {
            //statDefense.AnimateUI();
            StartCoroutine(statDefense.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (unitFunc.prevStatSpeed > unitFunc.GetUnitSpeed())
        {
            //statSpeed.AnimateUI();
            StartCoroutine(statSpeed.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        statHealth.UpdateContentText(unitFunc.GetUnitMaxHealth().ToString());
        statPower.UpdateContentText(unitFunc.curPower.ToString());
        statHealingPower.UpdateContentText(unitFunc.curHealingPower.ToString());
        statDefense.UpdateContentText(unitFunc.GetCurDefense().ToString());
        statSpeed.UpdateContentText(unitFunc.GetUnitSpeed().ToString());

        unitFunc.prevStatHealth = (int)unitFunc.GetUnitMaxHealth();
        unitFunc.prevStatPower = (int)unitFunc.curPower;
        unitFunc.prevStatHealingPower = (int)unitFunc.curHealingPower;
        unitFunc.prevStatDefense = (int)unitFunc.GetCurDefense();
        unitFunc.prevStatSpeed = (int)unitFunc.GetUnitSpeed();
    }

    public void UpdateStatHealth(int health)
    {
        statHealth.UpdateContentText(health.ToString());
        statHealth.AnimateUI();
    }

    public void UpdateStatDamageHits(int dmgHits)
    {
        statPower.UpdateContentText(dmgHits.ToString());
        statPower.AnimateUI();
    }

    public void ToggleUnitLocked(bool toggle, bool displayQuestionMark = false)
    {
        unitLocked = toggle;

        if (unitLocked)
            unitImage.color = CharacterCarasel.Instance.lockedUnitColour;
        else
            unitImage.color = CharacterCarasel.Instance.unlockedUnitColour;

        if (displayQuestionMark)
            unitQuestionMarkImage.UpdateAlpha(1);
        else
            unitQuestionMarkImage.UpdateAlpha(0);
    }

    public bool GetLocked()
    {
        return unitLocked;
    }
    public void StartIdleAnim()
    {
        animator.SetBool("MoveFlg", false);
        animator.SetBool("AttackFlg", false);
    }

    public void StartWalkAnim()
    {
        animator.SetBool("AttackFlg", false);
        animator.SetBool("MoveFlg", true);
    }

    public void StartAttackAnim()
    {
        animator.SetBool("MoveFlg", false);

        animator.SetBool("Skill1", true);
        animator.SetBool("AttackFlg", true);

        StartCoroutine(startAttackAnimCo());
    }

    IEnumerator startAttackAnimCo()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("AttackFlg", false);
        animator.SetBool("Skill1", false);
    }
}
