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

    private RectTransform rt;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        mainText = GetComponent<Text>();
        rt = GetComponent<RectTransform>();

        if (startHidden)
            UpdateAlpha(0);
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
