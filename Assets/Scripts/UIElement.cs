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

    public enum StatType { STATSTANDARD1, STATSTANDARD2, STATSTANDARD3, STATSTANDARD4, STATSTANDARD5, STATADVANCED1, STATADVANCED2, STATADVANCED3, STATADVANCED4, BG };
    public StatType curStatType;

    public enum ActiveMasteryType { STANDARD, ADVANCED };
    public ActiveMasteryType activeStatType;

    public Image contentImage;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Text contentSubText;
    [SerializeField] private CanvasGroup buttonCG;

    [SerializeField] private bool startHidden;

    [SerializeField] private bool doScalePunch;
    [SerializeField] private bool dieAfterDisplay;
    [SerializeField] private float scaleIncSize = 1;
    [SerializeField] private float scaleIncTime = .25f;
    [SerializeField] private int vibrato = 5;
    [SerializeField] private float elasticity = .25f;

    [SerializeField] private bool selectable;
    [SerializeField] private UIElement selectBorder;
    [SerializeField] private int statPointsAdded;

    [SerializeField] private UIElement lockedImage;
    [SerializeField] private int statPointsThreshhold;
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

        /*
        if (selectable)
            UpdateIsLocked(true);
        */
        
        if (gameObject.GetComponent<RectTransform>() != null)
        {
            originalScale = transform.GetComponent<RectTransform>().localScale.x;
            GetOriginalYPosition();
        }
    }

    public bool GetIsLocked()
    {
        return isLocked;
    }

    public void UpdateIsLocked(bool toggle)
    {
        /*
        isLocked = toggle;
        ToggleLockedImage(toggle);
        */
    }

    void Start()
    {
        //CheckIfThreshholdPassed();
    }

    public void SetYPosition()
    {
        //gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, originalYPos);
        //gameObject.transform.position = new Vector3(0, originalYPos, transform.position.z);
    }

    public void GetOriginalYPosition()
    {
        //originalYPos = gameObject.GetComponent<RectTransform>().localPosition.y;
        originalYPos = gameObject.transform.position.y;
    }

    public void CheckIfThreshholdPassed(bool toggle = false)
    {
        if (GetIsSelectable())
        {
            // If locked, display as locked
            if (TeamSetup.Instance.GetActiveStatTypeSpentPoints() < GetMasteryPointThreshhold())
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

    public void UpdateStatPoindsAdded(bool adding, bool isReset = false, int bulkPointsAdding = 0, bool bulkAdding = false, string statType = "OFFENSE")
    {
        if (isReset)
        {
            if (TeamSetup.Instance.GetActiveUnit().GetSpentMasteryPoints() < GetMasteryPointThreshhold())
                ToggleLockedImage(true);
            else
                ToggleLockedImage(false);

            statPointsAdded = 0;
            return;
        }

        if (adding)
        {
            if (TeamSetup.Instance.CalculateUnspentStatPoints() <= 0)
                return;

            // If bulk adding,
            if (bulkAdding)
                statPointsAdded = bulkPointsAdding;
            else
            {
                statPointsAdded++;
                TeamSetup.Instance.UpdateUnspentStatPoints(true);
            }

            // Adds
            if (statType == "STANDARD")
            {
                if (curStatType == StatType.STATSTANDARD1)
                    TeamSetup.Instance.GetActiveUnit().statsBase1Added = GetStatPointsAdded();
                else if (curStatType == StatType.STATSTANDARD2)
                    TeamSetup.Instance.GetActiveUnit().statsBase2Added = GetStatPointsAdded();
                else if (curStatType == StatType.STATSTANDARD3)
                    TeamSetup.Instance.GetActiveUnit().statsBase3Added = GetStatPointsAdded();
                else if (curStatType == StatType.STATSTANDARD4)
                    TeamSetup.Instance.GetActiveUnit().statsBase4Added = GetStatPointsAdded();
                else if (curStatType == StatType.STATSTANDARD5)
                    TeamSetup.Instance.GetActiveUnit().statsBase5Added = GetStatPointsAdded();
            }
            else if (statType == "ADVANCED")
            {
                if (curStatType == StatType.STATADVANCED1)
                    TeamSetup.Instance.GetActiveUnit().statsAdv1Added = GetStatPointsAdded();
                else if (curStatType == StatType.STATADVANCED2)
                    TeamSetup.Instance.GetActiveUnit().statsAdv2Added = GetStatPointsAdded();
                else if (curStatType == StatType.STATADVANCED3)
                    TeamSetup.Instance.GetActiveUnit().statsAdv3Added = GetStatPointsAdded();
                else if (curStatType == StatType.STATADVANCED4)
                    TeamSetup.Instance.GetActiveUnit().statsAdv4Added = GetStatPointsAdded();
            }
        }
        else
        {
            statPointsAdded--;
            TeamSetup.Instance.UpdateUnspentStatPoints(false);
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
        return statPointsThreshhold;
    }

    public int GetStatPointsAdded()
    {
        return statPointsAdded;
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

        // Shit fix for rewards finding their content text
        if (contentText == null)
            contentText = transform.GetComponentInChildren<TextMeshProUGUI>();

        contentText.text = text;
        //AnimateUI();
    }

    public void UpdateContentTextColour(TMP_ColorGradient gradient)
    {
        contentText.colorGradientPreset = gradient;
    }

    public void AnimateUI()
    {
        if (!doScalePunch)
            return;

        ResetAnimateScale();

        //contentText.gameObject.transform.DOLocalMoveY()
        if (doScalePunch)
            contentText.gameObject.transform.DOPunchScale(new Vector3(scaleIncSize, scaleIncSize), scaleIncTime, vibrato, elasticity);

        if (dieAfterDisplay)
            StartCoroutine(HideUIOvertime(scaleIncTime + GameManager.Instance.skillAlertAppearTime));
    }

    IEnumerator HideUIOvertime(float time = 0)
    {
        ResetAnimateScale();

        yield return new WaitForSeconds(time);

        UpdateAlpha(0);

    }

    public IEnumerator ChangeTextColourTime(Color newColor, float time = 1)
    {
        contentText.color = newColor;

        yield return new WaitForSeconds(time);

        contentText.color = TeamGearManager.Instance.statDefaultColour;
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

    public void UpdateAlpha(float alpha, bool difAlpha = false, float difAlphaNum = 0)
    {
        cg = GetComponent<CanvasGroup>();

        cg.alpha = alpha;   // Update UI Alpha

        if (difAlpha)
        {
            cg.alpha = difAlphaNum;
        }
        // Make UI element selectable/unselectable
        if (alpha == 1)
        {
            AnimateUI();

            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }


        if (GetComponent<Image>())
        {
            if (alpha == 1)
                GetComponent<Image>().raycastTarget = true;
            else
                GetComponent<Image>().raycastTarget = false;
        }
    }

    public void UpdateImage(bool toggle)
    {
        contentImage.raycastTarget = toggle;

        cg.interactable = toggle;
        cg.blocksRaycasts = toggle;

    }

    public void ToggleButton(bool toggle)
    {
        if (toggle)
            buttonCG.alpha = 1;
        else
            buttonCG.alpha = 0;

        buttonCG.interactable = toggle;
        buttonCG.blocksRaycasts = toggle;
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
