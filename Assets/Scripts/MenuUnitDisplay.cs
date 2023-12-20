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

    [Header("Stats")]
    [SerializeField] private UIElement statHealth;
    [SerializeField] private UIElement statDamageHits;
    [SerializeField] private UIElement statHealingHits;
    [SerializeField] private UIElement statDefense;
    [SerializeField] private UIElement statSpeed;

    private int prevStatHealth;
    private int prevStatDamageHits;
    private int prevStatHealingHits;
    private int prevStatDefense;
    private int prevStatSpeed;

    // todo am here

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUnitDisplay(string unitName)
    {
        if (unitName == "Warrior")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
            // Adjust size of unit
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        }

        else if (unitName == "Archer")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.archerAnimator;
            // Adjust size of unit
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 140);
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, 20);
        }

        else if (unitName == "Locked")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
            // Adjust size of unit
            animator.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

            ToggleUnitLocked(true, true);
        }

        StartIdleAnim();
    }

    public void UpdateUnitStats(UnitFunctionality unitFunc)
    {
        // If new gear has a higher stat then current, make the stat bounce

        if (prevStatHealth < unitFunc.GetUnitMaxHealth())
        {
            statHealth.AnimateUI();
            StartCoroutine(statHealth.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (prevStatDamageHits < unitFunc.GetUnitDamageHits())
        {
            statDamageHits.AnimateUI();
            StartCoroutine(statDamageHits.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (prevStatHealingHits < unitFunc.GetUnitHealingHits())
        {
            statHealingHits.AnimateUI();
            StartCoroutine(statHealingHits.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (prevStatDefense < unitFunc.GetCurDefense())
        {
            statDefense.AnimateUI();
            StartCoroutine(statDefense.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }

        if (prevStatSpeed < unitFunc.GetUnitSpeed())
        {
            statSpeed.AnimateUI();
            StartCoroutine(statSpeed.ChangeTextColourTime(TeamGearManager.Instance.statIncreasedColour, TeamGearManager.Instance.timeStatIncColour));
        }


        // Decrease stat - colour behaviour
        if (prevStatHealth > unitFunc.GetUnitMaxHealth())
        {
            //statHealth.AnimateUI();
            StartCoroutine(statHealth.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (prevStatDamageHits > unitFunc.GetUnitDamageHits())
        {
            //statDamageHits.AnimateUI();
            StartCoroutine(statDamageHits.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (prevStatHealingHits > unitFunc.GetUnitHealingHits())
        {
            //statHealingHits.AnimateUI();
            StartCoroutine(statHealingHits.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (prevStatDefense > unitFunc.GetCurDefense())
        {
            //statDefense.AnimateUI();
            StartCoroutine(statDefense.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        if (prevStatSpeed > unitFunc.GetUnitSpeed())
        {
            //statSpeed.AnimateUI();
            StartCoroutine(statSpeed.ChangeTextColourTime(TeamGearManager.Instance.statDecreasedColour, TeamGearManager.Instance.timeStatDecColour));
        }

        statHealth.UpdateContentText(unitFunc.GetUnitMaxHealth().ToString());
        statDamageHits.UpdateContentText(unitFunc.GetUnitDamageHits().ToString());
        statHealingHits.UpdateContentText(unitFunc.GetUnitHealingHits().ToString());
        statDefense.UpdateContentText(unitFunc.GetCurDefense().ToString());
        statSpeed.UpdateContentText(unitFunc.GetUnitSpeed().ToString());

        prevStatHealth = (int)unitFunc.GetUnitMaxHealth();
        prevStatDamageHits = (int)unitFunc.GetUnitDamageHits();
        prevStatHealingHits = (int)unitFunc.GetUnitHealingHits();
        prevStatDefense = (int)unitFunc.GetCurDefense();
        prevStatSpeed = (int)unitFunc.GetUnitSpeed();


    }

    public void UpdateStatHealth(int health)
    {
        statHealth.UpdateContentText(health.ToString());
        statHealth.AnimateUI();
    }

    public void UpdateStatDamageHits(int dmgHits)
    {
        statDamageHits.UpdateContentText(dmgHits.ToString());
        statDamageHits.AnimateUI();
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
