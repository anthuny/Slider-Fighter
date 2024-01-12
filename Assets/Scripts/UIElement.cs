using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIElement : MonoBehaviour
{
    private CanvasGroup cg;
    private TextMeshProUGUI mainText;

    public enum StatType { STATSTANDARD1, STATSTANDARD2, STATSTANDARD3, STATSTANDARD4, STATSTANDARD5, STATADVANCED1, STATADVANCED2, STATADVANCED3, STATADVANCED4, BG };
    public StatType curStatType;

    public enum ActiveMasteryType { STANDARD, ADVANCED };
    public ActiveMasteryType activeStatType;

    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public Image contentImage;
    [SerializeField] private UIElement contentImageUI;
    [SerializeField] private UIElement contentImage2UI;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Text contentSubText;
    [SerializeField] private TextMeshProUGUI contentSubTextTMP;
    [SerializeField] private CanvasGroup buttonCG;
    [SerializeField] private CanvasGroup button2CG;
    [SerializeField] private CanvasGroup button3CG;

    [SerializeField] private bool startHidden;

    public bool doScalePunch;
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
    public string itemName;
    [SerializeField] private UIElement rarityBorderUI;

    private RectTransform rt;
    public float originalScaleText;
    private float originalScaleImage;
    private float originalYPos;
    public bool isEnabled = false;

    public void UpdateItemName(string name)
    {
        itemName = name;
    }

    public string GetItemName()
    {
        return itemName;
    }

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        mainText = GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();

        if (startHidden)
            UpdateAlpha(0);

        /*
        if (selectable)
            UpdateIsLocked(true);
        */
        
        if (gameObject.GetComponent<RectTransform>() != null)
        {
            originalScaleText = transform.GetComponent<RectTransform>().localScale.x;
            GetOriginalYPosition();
        }

        if (contentImage != null)
        {
            originalScaleImage = contentImage.gameObject.transform.localScale.x;
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

    public void UpdateContentUI(Sprite sprite)
    {
        contentImageUI.UpdateContentImage(sprite);
    }
    public void UpdateContent2UI(Sprite sprite)
    {
        contentImage2UI.UpdateContentImage(sprite);
    }

    public bool GetIsSelectable()
    {
        return selectable;
    }

    public void ToggleSelected(bool toggle, bool clearItemRewardsSelection = false)
    {
        if (toggle)
        {
            if (clearItemRewardsSelection)
                ItemRewardManager.Instance.ClearItemSelection();

            selectBorder.UpdateAlpha(1);
        }

        else
            selectBorder.UpdateAlpha(0);
    }

    public void UpdateRarityBorderColour(Color colour)
    {
        rarityBorderUI.UpdateColour(colour);
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

    public void AnimateUI(bool text = true)
    {
        if (!doScalePunch)
            return;

        if (text)
        {
            if (contentText == null)
                return;

            ResetAnimateScaleText();

            if (doScalePunch)
                contentText.gameObject.transform.DOPunchScale(new Vector3(scaleIncSize, scaleIncSize), scaleIncTime, vibrato, elasticity);

            AnimateUIMaxCap();

            ResetAnimateScaleText();
        }
        else
        {
            ResetAnimateScaleImage();

            if (doScalePunch)
                contentImage.gameObject.transform.DOPunchScale(new Vector3(scaleIncSize, scaleIncSize), scaleIncTime, vibrato, elasticity);
            
            ResetAnimateScaleImage();
        }

        if (dieAfterDisplay)
            StartCoroutine(HideUIOvertime(scaleIncTime + GameManager.Instance.skillAlertAppearTime));
    }

    void AnimateUIMaxCap()
    {
        if (contentText.gameObject.transform.localScale.x >= 1.3f)
            contentText.gameObject.transform.localScale = new Vector2(1.3f, 1.3f);
    }

    IEnumerator HideUIOvertime(float time = 0)
    {
        ResetAnimateScaleText();

        yield return new WaitForSeconds(time);

        UpdateAlpha(0);

    }

    public IEnumerator ChangeTextColourTime(Color newColor, float time = 1)
    {
        contentText.color = newColor;

        yield return new WaitForSeconds(time);

        contentText.color = TeamGearManager.Instance.statDefaultColour;
    }

    public void ResetAnimateScaleText()
    {
        contentText.gameObject.transform.localScale = new Vector3(originalScaleText, originalScaleText);

        contentText.gameObject.transform.DORestart();
        //contentText.gameObject.transform.DOKill(true);
    }
    public void ResetAnimateScaleImage()
    {
        contentImage.gameObject.transform.localScale = new Vector3(originalScaleImage, originalScaleImage);

        contentImage.gameObject.transform.DORestart();
        //contentText.gameObject.transform.DOKill(true);
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

    public void UpdateContentSubTextTMPColour(Color colour)
    {
        contentSubTextTMP.color = colour;
    }

    public void UpdateContentSubTextTMP(string text)
    {
        contentSubTextTMP.text = text;
    }
    public void ToggleContentSubTextTMP(bool toggle)
    {
        if (toggle)
            contentSubTextTMP.gameObject.GetComponent<UIElement>().UpdateAlpha(1);
        else
            contentSubTextTMP.gameObject.GetComponent<UIElement>().UpdateAlpha(0);
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

            isEnabled = true;

            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            isEnabled = false;

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

        if (GetComponent<Button>())
        {
            if (alpha == 1)
                GetComponent<Button>().interactable = true;
            else
                GetComponent<Button>().interactable = false;
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

        buttonCG.GetComponent<Button>().interactable = toggle;
    }

    public void ToggleButton2(bool toggle, bool bypass = false)
    {
        if (bypass)
        {
            if (toggle)
                button2CG.alpha = 1;
            else
                button2CG.alpha = 0;
        }

        button2CG.interactable = toggle;
        button2CG.blocksRaycasts = toggle;

        button2CG.GetComponent<Button>().interactable = toggle; 
    }

    public void ToggleButton3(bool toggle, bool bypass = false)
    {
        if (bypass)
        {
            if (toggle)
                button3CG.alpha = 1;
            else
                button3CG.alpha = 0;
        }

        button3CG.interactable = toggle;
        button3CG.blocksRaycasts = toggle;

        if (button3CG.GetComponent<Button>())
            button3CG.GetComponent<Button>().interactable = toggle;
    }

    public void DisableAlertUI()
    {
        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0);
    }

    public IEnumerator TriggerUIAlert(float duration, string alertText, TMP_ColorGradient gradient)
    {
        mainText.text = alertText;
        //mainText.color = mainText.color = color;
        mainText.colorGradientPreset = gradient;
        mainText.color = Weapon.Instance.defaultColour;

        AnimateUI();

        yield return new WaitForSeconds(duration);

        DisableAlertUI();
    }
}
