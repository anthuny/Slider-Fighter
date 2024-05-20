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

    public enum StatType { SKILLSLOT1, SKILLSLOT2, SKILLSLOT3, SKILLSLOT4, BG };
    public StatType curStatType;

    public enum Rarity { COMMON, RARE, EPIC, LEGENDARY }
    public Rarity curRarity;

    public Image contentImage;
    public UIElement contentImageUI;
    [SerializeField] private UIElement contentImage2UI;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI contentText2;
    [SerializeField] private TextMeshProUGUI contentText3;
    [SerializeField] private Text contentSubText;
    [SerializeField] private TextMeshProUGUI contentSubTextTMP;
    public CanvasGroup buttonCG;
    [SerializeField] private CanvasGroup button2CG;
    [SerializeField] private CanvasGroup button3CG;
    [SerializeField] private UIElement skillLevelText;

    [SerializeField] private bool startHidden;

    public bool doScalePunch;
    [SerializeField] private bool dieAfterDisplay;
    [SerializeField] private float scaleIncSize = 1;
    [SerializeField] private float scaleIncTime = .25f;
    [SerializeField] private int vibrato = 5;
    [SerializeField] private float elasticity = .25f;

    [SerializeField] private bool selectable;
    public UIElement selectBorder;
    [SerializeField] private int statPointsAdded;

    [SerializeField] private UIElement lockedImage;
    [SerializeField] private int unlockedPointsThreshhold;
    [SerializeField] bool isLocked;
    public string itemName;
    [SerializeField] private UIElement rarityBorderUI;
    public Slot slot;
    public UIElement skillUpgradesUI;
    public UIElement tooltipStats;
    [SerializeField] private string statText;


    private RectTransform rt;
    public float originalScaleText;
    private float originalScaleImage;
    private float originalYPos;
    public bool isEnabled = false;
    public bool displayingAlert = false;


    public void UpdateTooltipStatsText(string text)
    {
        statText = text;

        UpdateTooltipText();
    }


    public void ToggleTooltipStats(bool toggle)
    {
        //Debug.Log("toggling " + toggle);

        if (toggle)
        {
            //Debug.Log(tooltipStats.isEnabled);

            if (!tooltipStats.isEnabled)
            {
                UpdateTooltipText();
                tooltipStats.UpdateAlpha(1);
            }
        }
        else
            tooltipStats.UpdateAlpha(0);
    }

    public void UpdateTooltipText()
    {
        tooltipStats.UpdateContentText(statText);
    }

    public void UpdateItemName(string name)
    {
        itemName = name;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public void UpdateSkillLevelText(int level)
    {
        skillLevelText.UpdateContentText(level.ToString());
    }

    public UIElement GetSkillLevelText()
    {
        return skillLevelText;
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
        isLocked = toggle;
        //ToggleLockedImage(toggle);     
    }

    void Start()
    {

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

    /*
    public void CheckIfThreshholdPassed(bool toggle = false)
    {
        if (GetIsSelectable())
        {
            // If locked, display as locked
            if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitLevel() > GetSkillPointThreshhold())
                UpdateIsLocked(false);
            else
            {
                if (toggle)
                {
                    if (GetIsLocked())
                        UpdateIsLocked(true);
                }
            }
        }
    }
    */

    public void UpdateStatPoindsAdded(bool adding, bool isReset = false, int bulkPointsAdding = 0, bool bulkAdding = false)
    {
        if (isReset)
        {
            /*
            if (SkillsTabManager.Instance.GetActiveUnit().GetSpentSkillPoints() < GetSkillPointThreshhold())
                ToggleLockedImage(true);
            else
                ToggleLockedImage(false);
            */

            statPointsAdded = 0;
            return;
        }

        if (adding)
        {
            if (SkillsTabManager.Instance.CalculateUnspentSkillPoints() <= 0)
                return;

            // If bulk adding,
            if (bulkAdding)
                statPointsAdded = bulkPointsAdding;
            else
            {
                statPointsAdded++;
                SkillsTabManager.Instance.UpdateUnspentSkillPoints(true);
            }

            // Adds

            if (curStatType == StatType.SKILLSLOT1)
                SkillsTabManager.Instance.GetActiveUnit().statsBase1Added = GetStatPointsAdded();
            else if (curStatType == StatType.SKILLSLOT2)
                SkillsTabManager.Instance.GetActiveUnit().statsBase2Added = GetStatPointsAdded();
            else if (curStatType == StatType.SKILLSLOT3)
                SkillsTabManager.Instance.GetActiveUnit().statsBase3Added = GetStatPointsAdded();
            else if (curStatType == StatType.SKILLSLOT4)
                SkillsTabManager.Instance.GetActiveUnit().statsBase4Added = GetStatPointsAdded();
        }
        else
        {
            statPointsAdded--;
            SkillsTabManager.Instance.UpdateUnspentSkillPoints(false);
        }

        /*
        if (SkillsTabManager.Instance.GetActiveUnit().GetSpentSkillPoints() < GetSkillPointThreshhold())
            ToggleLockedImage(true);
        else
            ToggleLockedImage(false);
        */
    }

    public void ToggleLockedMainSlot()
    {
        // In Combat
        if (GameManager.Instance.playerInCombat)
        {
            if (GameManager.Instance.isSkillsMode)
            {
                //Debug.Log(GetSkillPointThreshhold());
                if (GameManager.Instance.GetActiveUnitFunctionality().GetUnitLevel() < GetSkillPointThreshhold() && !GameManager.Instance.GetActiveUnitFunctionality().reanimated)
                {
                    //Debug.Log("Unit Level = " + GameManager.Instance.GetActiveUnitFunctionality().GetUnitLevel() + " " + GetSkillPointThreshhold());
                    //Debug.Log(toggle);
                    ToggleLockedImage(true);
                    UpdateIsLocked(true);
                    //ToggleLockedImage(toggle);
                    buttonCG.gameObject.GetComponent<ButtonFunctionality>().isLocked = true;
                }
                else
                {
                    //Debug.Log(toggle);
                    ToggleLockedImage(false);
                    UpdateIsLocked(false);
                    //ToggleLockedImage(toggle);
                    buttonCG.gameObject.GetComponent<ButtonFunctionality>().isLocked = false;
                }
            }
            else
            {
                //Debug.Log(toggle);
                ToggleLockedImage(false);
                UpdateIsLocked(false);
                //ToggleLockedImage(toggle);
                buttonCG.gameObject.GetComponent<ButtonFunctionality>().isLocked = false;
            }
        }
        // In skills tab
        else if (SkillsTabManager.Instance.playerInSkillTab)
        {
            //Debug.Log(GetSkillPointThreshhold());
            if (GameManager.Instance.GetActiveAlly().GetUnitLevel() < GetSkillPointThreshhold())
            {
                //Debug.Log(toggle);
                ToggleLockedImage(true);
                UpdateIsLocked(true);
                //ToggleLockedImage(toggle);
                buttonCG.gameObject.GetComponent<ButtonFunctionality>().isLocked = true;
            }
            else
            {
                //Debug.Log(toggle);
                ToggleLockedImage(false);
                UpdateIsLocked(false);
                //ToggleLockedImage(toggle);
                buttonCG.gameObject.GetComponent<ButtonFunctionality>().isLocked = false;
            }
        }
    }

    void ToggleLockedImage(bool toggle)
    {
        if (toggle)
            lockedImage.UpdateAlpha(1);
        else
            lockedImage.UpdateAlpha(0);
    }

    public int GetSkillPointThreshhold()
    {
        return unlockedPointsThreshhold;
    }

    public void UpdateSkillPointThreshhold(int newThresh)
    {
        unlockedPointsThreshhold = newThresh;
    }

    public int GetStatPointsAdded()
    {
        return statPointsAdded;
    }

    public void UpdateContentImage(Sprite sprite)
    {
        if (sprite == null)
            contentImage.sprite = MapManager.Instance.invisSprite;
        else
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

    public void UpdateSlider(float maxCharges = 1f, float curCharges = 0f)
    {
        contentImage2UI.GetComponent<Image>().fillAmount = (curCharges / maxCharges);
    }

    public bool GetIsSelectable()
    {
        return selectable;
    }

    public void ToggleSelected(bool toggle, bool clearItemRewardsSelection = false)
    {
        //Debug.Log("UIELEMENT Toggling " + toggle);

        if (toggle)
        {
            if (clearItemRewardsSelection)
                ItemRewardManager.Instance.ClearItemSelection();

            selectBorder.UpdateAlpha(1);

            //if (selectBorder.GetComponent<Animator>())
            //    selectBorder.GetComponent<Animator>();
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
    public void UpdateContentText2(string text)
    {
        //Debug.Log(gameObject.name);

        // Shit fix for rewards finding their content text
        if (contentText2 == null)
            contentText2 = transform.GetComponentInChildren<TextMeshProUGUI>();

        contentText2.text = text;
        //AnimateUI();
    }
    public void UpdateContentText3(string text)
    {
        //Debug.Log(gameObject.name);

        // Shit fix for rewards finding their content text
        if (contentText3 == null)
            contentText3 = transform.GetComponentInChildren<TextMeshProUGUI>();

        contentText3.text = text;
        //AnimateUI();
    }

    public void UpdateContentTextColour(TMP_ColorGradient gradient)
    {
        contentText.colorGradientPreset = gradient;
    }

    public void UpdateContentTextColourTMP(Color color)
    {
        contentText.color = color;
    }

    public void AnimateUI(bool text = true, bool depleteEffect = false)
    {
        if (!doScalePunch)
            return;

        if (text)
        {
            if (contentText == null)
                return;

            ResetAnimateScaleText();

            //StartCoroutine(HideUIOvertime(0, false, false));
            //ResetAnimateScaleText();

            if (doScalePunch)
                contentText.gameObject.transform.DOPunchScale(new Vector3(scaleIncSize, scaleIncSize), scaleIncTime, vibrato, elasticity);
            //contentText.gameObject.transform.DORestart();
            AnimateUIMaxCap();
            ResetAnimateScaleText();
        }
        else
        {
            if (contentImage == null)
                return;

            ResetAnimateScaleImage();

            if (doScalePunch)
                contentImage.gameObject.transform.DOPunchScale(new Vector3(scaleIncSize, scaleIncSize), scaleIncTime, vibrato, elasticity);

            ResetAnimateScaleImage();
        }

        if (dieAfterDisplay)
        {
            if (!text)
                StartCoroutine(HideUIOvertime(GameManager.Instance.skillAlertAppearTime / 1.25f, true));
            else if (depleteEffect)
                StartCoroutine(HideUIOvertime(scaleIncTime + GameManager.Instance.skillEffectDepleteAppearTime));
            else
                StartCoroutine(HideUIOvertime(scaleIncTime + GameManager.Instance.skillAlertAppearTime));
        }
    }

    void AnimateUIMaxCap()
    {
        if (contentText.gameObject.transform.localScale.x >= 1.3f)
            contentText.gameObject.transform.localScale = new Vector2(1.3f, 1.3f);
    }

    IEnumerator HideUIOvertime(float time = 0, bool skipResetText = false, bool turnOff = true)
    {
        if (!skipResetText)
            ResetAnimateScaleText();

        yield return new WaitForSeconds(time);

        if (turnOff)
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
        //Debug.Log("")
        contentText.gameObject.transform.DORestart();
        //contentText.gameObject.transform.DOKill(true);
    }
    public void ResetAnimateScaleImage()
    {
        if (contentImage == null)
            return;

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
        if (rt != null)
            rt.sizeDelta = pos;
    }

    public void UpdateColour(Color colour)
    {
        contentImage.color = colour;
    }

    public void UpdateAlpha(float alpha, bool difAlpha = false, float difAlphaNum = 0, bool depletingEffect = false, bool text = true)
    {
        if (this == null)
            return;

        cg = GetComponent<CanvasGroup>();

        cg.alpha = alpha;   // Update UI Alpha

        if (difAlpha)
        {
            cg.alpha = difAlphaNum;
        }

        //Debug.Log("updating alpha " + alpha);
        // Make UI element selectable/unselectable
        if (alpha == 1)
        {
            AnimateUI(text);

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
        if (buttonCG == null)
        {
            Debug.Log(gameObject.name + " error");
        }
        else
        {
            if (toggle)
                buttonCG.alpha = 1;
            else
                buttonCG.alpha = 0;

            buttonCG.interactable = toggle;
            buttonCG.blocksRaycasts = toggle;

            buttonCG.GetComponent<Button>().interactable = toggle;
        }



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


    public void AllowMovingUp()
    {

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
        displayingAlert = false;

        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0);
    }

    public IEnumerator TriggerUIAlert(float duration, string alertText, TMP_ColorGradient gradient)
    {
        displayingAlert = true;
        mainText.text = alertText;
        //mainText.color = mainText.color = color;
        mainText.colorGradientPreset = gradient;
        mainText.color = WeaponManager.Instance.defaultColour;

        AnimateUI();

        yield return new WaitForSeconds(duration);

        DisableAlertUI();
    }
}
