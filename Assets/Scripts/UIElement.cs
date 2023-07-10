using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIElement : MonoBehaviour
{
    private CanvasGroup cg;
    private Text mainText;

    public enum MasteryType { L1, L2, L3, L4, R1, R2, R3, R4, BG };
    public MasteryType curMasteryType;

    public enum ActiveMasteryType { OFFENSE, DEFENSE, UTILITY };
    public ActiveMasteryType activeMasteryType;

    [SerializeField] private Image contentImage;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Text contentSubText;

    [SerializeField] private bool startHidden;

    [SerializeField] private bool doScalePunch;
    [SerializeField] private bool dieAfterDisplay;
    [SerializeField] private float scaleIncSize = 1;
    [SerializeField] private float scaleIncTime = .25f;
    [SerializeField] private int vibrato = 5;
    [SerializeField] private float elasticity = .25f;

    [SerializeField] private bool selectable;
    [SerializeField] private UIElement selectBorder;
    [SerializeField] private int masteryPointsAdded;

    [SerializeField] private UIElement lockedImage;
    [SerializeField] private int masteryPointsThreshhold;
    [SerializeField] bool isLocked;

    private RectTransform rt;
    private float originalScale;
    private float originalYPos;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        mainText = GetComponent<Text>();
        rt = GetComponent<RectTransform>();

        if (startHidden)
            UpdateAlpha(0);

        if (selectable)
            UpdateIsLocked(true);

        originalScale = transform.GetComponent<RectTransform>().localScale.x;

        GetOriginalYPosition();
    }

    public bool GetIsLocked()
    {
        return isLocked;
    }

    public void UpdateIsLocked(bool toggle)
    {
        isLocked = toggle;
        ToggleLockedImage(toggle);
    }

    void Start()
    {
        CheckIfThreshholdPassed();
    }

    public void ResetYPosition()
    {
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(gameObject.GetComponent<RectTransform>().localPosition.x, originalYPos);
    }

    public void GetOriginalYPosition()
    {
        originalYPos = gameObject.GetComponent<RectTransform>().localPosition.y;
    }

    public void CheckIfThreshholdPassed(bool toggle = false)
    {
        if (GetIsSelectable())
        {
            // If locked, display as locked
            if (TeamSetup.Instance.GetActiveMasteryTypeSpentPoints() < GetMasteryPointThreshhold())
                UpdateIsLocked(true);
            else
            {
                if (toggle)
                {
                    if (GetIsLocked())
                        UpdateIsLocked(false);
                }
            }
        }
    }

    public void UpdateMasteryPoindsAdded(bool adding, bool isReset = false, int bulkPointsAdding = 0, bool bulkAdding = false, string masteryType = "OFFENSE")
    {
        if (isReset)
        {
            if (TeamSetup.Instance.GetActiveUnit().GetSpentMasteryPoints() < GetMasteryPointThreshhold())
                ToggleLockedImage(true);
            else
                ToggleLockedImage(false);

            masteryPointsAdded = 0;
            return;
        }

        if (adding)
        {
            if (TeamSetup.Instance.CalculateUnspentPoints() <= 0)
                return;

            // If bulk adding,
            if (bulkAdding)
                masteryPointsAdded = bulkPointsAdding;
            else
            {
                masteryPointsAdded++;
                TeamSetup.Instance.UpdateUnspentMasteryPoints(true);
            }

            // Adds
            if (masteryType == "OFFENSE")
            {
                if (curMasteryType == MasteryType.L1)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseL1AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L2)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseL2AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L3)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseL3AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L4)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseL4AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R1)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseR1AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R2)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseR2AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R3)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseR3AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R4)
                    TeamSetup.Instance.GetActiveUnit().masteryOffenseR4AddedCount = GetMasteryPointsAdded();
            }
            else if (masteryType == "DEFENSE")
            {
                if (curMasteryType == MasteryType.L1)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseL1AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L2)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseL2AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L3)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseL3AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L4)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseL4AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R1)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseR1AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R2)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseR2AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R3)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseR3AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R4)
                    TeamSetup.Instance.GetActiveUnit().masteryDefenseR4AddedCount = GetMasteryPointsAdded();
            }
            else if (masteryType == "UTILITY")
            {
                if (curMasteryType == MasteryType.L1)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityL1AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L2)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityL2AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L3)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityL3AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.L4)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityL4AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R1)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityR1AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R2)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityR2AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R3)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityR3AddedCount = GetMasteryPointsAdded();
                else if (curMasteryType == MasteryType.R4)
                    TeamSetup.Instance.GetActiveUnit().masteryUtilityR4AddedCount = GetMasteryPointsAdded();
            }
        }
        else
        {
            masteryPointsAdded--;
            TeamSetup.Instance.UpdateUnspentMasteryPoints(false);
        }

        if (TeamSetup.Instance.GetActiveUnit().GetSpentMasteryPoints() < GetMasteryPointThreshhold())
            ToggleLockedImage(true);
        else
            ToggleLockedImage(false);

    }

    public void ToggleLockedImage(bool toggle)
    {
        if (toggle)
            lockedImage.UpdateAlpha(1);
        else
            lockedImage.UpdateAlpha(0);
    }

    public int GetMasteryPointThreshhold()
    {
        return masteryPointsThreshhold;
    }

    public int GetMasteryPointsAdded()
    {
        return masteryPointsAdded;
    }

    public void UpdateContentImage(Sprite sprite)
    {
        contentImage.sprite = sprite;
    }

    public bool GetIsSelectable()
    {
        return selectable;
    }

    public void ToggleSelected(bool toggle)
    {
        if (toggle)
            selectBorder.UpdateAlpha(1);
        else
            selectBorder.UpdateAlpha(0);
    }

    public void UpdateContentText(string text)
    {
        //Debug.Log(gameObject.name);

        //if (contentText == null)
        //    contentText = transform.GetComponentInChildren<TextMeshProUGUI>();

        contentText.text = text;
        //AnimateUI();
    }

    public void UpdateContentTextColour(TMP_ColorGradient gradient)
    {
        contentText.colorGradientPreset = gradient;
    }

    public void AnimateUI()
    {
        if (doScalePunch)
            contentText.gameObject.transform.DOPunchScale(new Vector3(scaleIncSize, scaleIncSize), scaleIncTime, vibrato, elasticity);

        if (!dieAfterDisplay)
            StartCoroutine(HideUIOvertime(scaleIncTime + GameManager.Instance.skillAlertAppearTime));
    }

    IEnumerator HideUIOvertime(float time = 0)
    {
        ResetAnimateScale();

        yield return new WaitForSeconds(time);

        UpdateAlpha(0);

    }

    public void ResetAnimateScale()
    {
        contentText.gameObject.transform.localScale = new Vector3(originalScale, originalScale);
    }
    public void UpdateContentTextColour(Color colour)
    {
        contentText.color = colour;
    }

    public void UpdateContentSubText(string text)
    {
        //Debug.Log(gameObject.name);
        contentSubText.text = text;
    }

    public void UpdateContentSubTextColour(Color colour)
    {
        contentSubText.color = colour;
    }

    public void UpdateRectPos(Vector2 pos)
    {
        rt.sizeDelta = pos;
    }

    public void UpdateColour(Color colour)
    {
        contentImage.color = colour;
    }

    public void UpdateAlpha(float alpha)
    {
        //StopCoroutine(HideUIOvertime());

        //Debug.Log(gameObject.name);
        cg = GetComponent<CanvasGroup>();

        //Debug.Log(gameObject.name);
        cg.alpha = alpha;   // Update UI Alpha



        // Make UI element selectable/unselectable
        if (alpha == 1)
        {
            if (doScalePunch)
                AnimateUI();

            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void DisableAlertUI()
    {
        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0);
    }

    public IEnumerator TriggerUIAlert(float duration, string alertText, Color color)
    {
        mainText.text = alertText;
        mainText.color = mainText.color = color;

        yield return new WaitForSeconds(duration);

        DisableAlertUI();
    }
}
