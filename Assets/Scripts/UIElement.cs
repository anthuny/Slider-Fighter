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

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        mainText = GetComponent<Text>();
        rt = GetComponent<RectTransform>();

        if (startHidden)
            UpdateAlpha(0);

        if (selectable)
            UpdateIsLocked(true);
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

    public void CheckIfThreshholdPassed(bool toggle = false)
    {
        if (GetIsSelectable())
        {
            // If locked, display as locked
            if (TeamSetup.Instance.GetSpendMasteryPoints() < GetMasteryPointThreshhold())
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

    public void UpdateMasteryPoindsAdded(bool adding, bool isReset = false)
    {
        if (isReset)
        {
            if (TeamSetup.Instance.GetSpendMasteryPoints() < GetMasteryPointThreshhold())
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

            masteryPointsAdded++;
            TeamSetup.Instance.UpdateUnspentMasteryPoints(true);

            if (TeamSetup.Instance.GetSpendMasteryPoints() < GetMasteryPointThreshhold())
                ToggleLockedImage(true);
            else
                ToggleLockedImage(false);
        }
        else
        {
            masteryPointsAdded--;
            TeamSetup.Instance.UpdateUnspentMasteryPoints(false);
        }
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
        yield return new WaitForSeconds(time);

        UpdateAlpha(0);

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
