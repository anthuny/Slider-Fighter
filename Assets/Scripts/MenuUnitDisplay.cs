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
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
        else if (unitName == "Archer")
            animator.runtimeAnimatorController = CharacterCarasel.Instance.archerAnimator;
        else if (unitName == "Locked")
        {
            animator.runtimeAnimatorController = CharacterCarasel.Instance.warriorAnimator;
            ToggleUnitLocked(true, true);
        }

        StartIdleAnim();
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
