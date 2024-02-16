using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateLog : MonoBehaviour
{
    public static UpdateLog Instance;

    [SerializeField] private UIElement updateLog;
    [SerializeField] private Transform updateLogContents;
    [SerializeField] private GameObject updateLogTextAreaGO;
    [SerializeField] private UIElement updateLogButton;

    [SerializeField] private int smallTextSize;
    [SerializeField] private int mediumTextSize;
    [SerializeField] private int largeTextSize;
    [SerializeField] private int hugeTextSize;
    [SerializeField] private int maxTextSize;

    [SerializeField] private int mediumCharacterThreshold = 50;
    [SerializeField] private int largeCharacterThreshhold = 100;
    [SerializeField] private int hugeCharacterThreshhold = 150;
    [SerializeField] private int maxCharacterThreshHold = 200;

    [SerializeField] private List<UpdateLogTextData> updateLogTexts = new List<UpdateLogTextData>();

    [HideInInspector]
    public bool updateLogIsOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleUpdateLog(bool toggle = true)
    {
        if (toggle)
            updateLog.UpdateAlpha(1);
        else
            updateLog.UpdateAlpha(0);

        updateLogIsOpen = toggle;

        if (toggle)
            PopulateUpdateLog();
    }

    public void ToggleUpdateLogbutton(bool toggle = true)
    {
        if (toggle)
            updateLogButton.UpdateAlpha(1);
        else
            updateLogButton.UpdateAlpha(0);

        updateLogButton.ToggleButton(toggle);   
    }
    void PopulateUpdateLog()
    {
        ResetCurrentUpdateLogTexts();

        for (int i = 0; i < updateLogTexts.Count; i++)
        {
            GameObject go = Instantiate(updateLogTextAreaGO, updateLogContents.position, Quaternion.identity);
            go.transform.SetParent(updateLogContents);
            go.transform.localPosition = Vector2.zero;
            go.transform.localScale = new Vector2(0.7f, .7f);

            UIElement uiElement = go.GetComponent<UIElement>();

            uiElement.enabled = true;
            go.GetComponent<CanvasGroup>().enabled = true;
            go.GetComponent<TextMeshProUGUI>().enabled = true;

            uiElement.UpdateContentText2(updateLogTexts[i].updateLogPreText);
            uiElement.UpdateContentText(updateLogTexts[i].updateLogText);

            int index = 0;

            for (int x = 0; x < uiElement.contentText.text.Length; x++)
            {
                index++;
            }

            if (index >= maxCharacterThreshHold)
            {
                uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta.x, maxTextSize);
                //break;
            }
            else if (index >= hugeCharacterThreshhold)
            {
                uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta.x, hugeTextSize);
                //break;
            }
            else if (index >= largeCharacterThreshhold)
            {
                uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta.x, largeTextSize);
                //break;
            }
            else if (index > mediumCharacterThreshold && index < largeCharacterThreshhold)
            {
                uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta.x, mediumTextSize);
            }
            else if (index <= mediumCharacterThreshold)
            {
                uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(uiElement.contentText.gameObject.GetComponent<RectTransform>().sizeDelta.x, smallTextSize);
            }

            //Debug.Log("Characters = " + index);
        }
    }

    void ResetCurrentUpdateLogTexts()
    {
        /*
        for (int i = 0; i < updateLogContents.childCount; i++)
        {
            Destroy(updateLogContents.GetChild(0).gameObject);
            Debug.Log("Destroying thing");
        }
        */
        foreach (Transform trans in updateLogContents)
        {
            Destroy(trans.gameObject);
            //Debug.Log("Destroying thing");
        }
    }
}
