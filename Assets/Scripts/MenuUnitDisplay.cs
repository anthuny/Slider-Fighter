using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUnitDisplay : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Image unitImage;
    [SerializeField] private UIElement unitQuestionMarkImage;
    [SerializeField] private bool unitLocked;
    [SerializeField] private UIElement unitLevel;

    [Header("Stats")]
    [SerializeField] private UIElement statHealth;
    [SerializeField] private UIElement statPower;
    [SerializeField] private UIElement statHealingPower;
    [SerializeField] private UIElement statDefense;
    [SerializeField] private UIElement statSpeed;

    // todo am here

    // Start is called before the first frame update
    void Start()
    {
        ToggleUnitLevelImage(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void UpdateUnitDisplay(string unitName)
    {
        if (unitName == "Knight")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
            // Adjust size of unit
            //animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 4.087784f);
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(10, 0);
        }

        else if (unitName == "Ranger")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.archerAnimator;

            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab)
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 5.8f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-5, 40);
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 4.087784f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-5, 55);
            }
        }

        else if (unitName == "Cleric")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.clericAnimator;

            // Adjust size of unit
            if (TeamGearManager.Instance.playerInGearTab || TeamItemsManager.Instance.playerInItemTab)
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, -26);
            }
            else
            {
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(5.3f, 5.3f);
                animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(20, 55);
            }
        }

        else if (unitName == "Locked")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
            // Adjust size of unit
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(4.087784f, 4.087784f);
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, 0);

            ToggleUnitLocked(true, true);
        }

        StartIdleAnim();
    }

    public void ResetUnitStats()
    {
        for (int i = 0; i < GameManager.Instance.activeRoomAllies.Count; i++)
        {
            GameManager.Instance.activeRoomAllies[i].prevStatHealth = 0;
            GameManager.Instance.activeRoomAllies[i].prevStatPower = 0;
            GameManager.Instance.activeRoomAllies[i].prevStatHealingPower = 0;
            GameManager.Instance.activeRoomAllies[i].prevStatDefense = 0;
            GameManager.Instance.activeRoomAllies[i].prevStatSpeed = 0;
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
        animator.SetBool("AttackFlg", true);

        StartCoroutine(startAttackAnimCo());
    }

    IEnumerator startAttackAnimCo()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("AttackFlg", false);
    }
}
