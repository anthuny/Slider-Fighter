using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElement : MonoBehaviour
{
    private CanvasGroup cg;
    private Text mainText;

    [SerializeField] private Image contentImage;
    [SerializeField] private Text contentText;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        mainText = GetComponent<Text>();
    }

    public void UpdateContentImage(Sprite sprite)
    {
        contentImage.sprite = sprite;
    }

    public void UpdateContentText(string text)
    {
        contentText.text = text;
    }

    public void UpdateContentTextColour(Color colour)
    {
        contentText.color = colour;
    }

    public void UpdateAlpha(float alpha)
    {
        //Debug.Log(gameObject.name);
        cg.alpha = alpha;   // Update UI Alpha

        // Make UI element selectable/unselectable
        if (alpha == 1)
        {
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
