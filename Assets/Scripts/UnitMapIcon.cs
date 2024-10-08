using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMapIcon : MonoBehaviour
{
    public Animator iconAnimator;
    [SerializeField] private float timeTillWalkStops = 1f;
    [SerializeField] private string unitName;

    public void UpdateUnitPosition(Vector2 pos)
    {
        //gameObject.transform.position = new Vector3(pos.x, pos.y, 0);
        transform.localPosition = new Vector3(pos.x, pos.y, 0);
    }

    public void UpdateIconAnimator(RuntimeAnimatorController ac)
    {
        iconAnimator.runtimeAnimatorController = ac;

        UpdateIconSize();
        StartWalkAnim();
    }

    void UpdateIconSize()
    {
        if (unitName == "Ranger")
        {
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 140);
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(-5, -20);
        }
        else if (unitName == "Knight")
        {
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(5, -60);
        }

        else if (unitName == "Cleric")
        {
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(125, 125);
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(5, -35);
        }
        else if (unitName == "Necromancer")
        {
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(125, 125);
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, -35);
        }
        else if (unitName == "Monk")
        {
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(125, 125);
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, -55);
        }
    }

    public void UpdateUnitName(string newName)
    {
        unitName = newName;
    }

    public void StartIdleAnim()
    {
        iconAnimator.SetBool("MoveFlg", false);
        iconAnimator.SetBool("AttackFlg", false);
    }

    public void StartWalkAnim()
    {
        iconAnimator.SetBool("AttackFlg", false);
        iconAnimator.SetBool("MoveFlg", true);

        StartCoroutine(StopWalkAnim());
    }

    public void StartAttackAnim()
    {
        iconAnimator.SetBool("MoveFlg", false);
        iconAnimator.SetBool("AttackFlg", true);
    }

    IEnumerator StopWalkAnim()
    {
        yield return new WaitForSeconds(timeTillWalkStops);
        iconAnimator.SetBool("MoveFlg", false);

        StartIdleAnim();
    }
}
