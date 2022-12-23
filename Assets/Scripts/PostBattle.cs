using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostBattle : MonoBehaviour
{
    [SerializeField] private UIElement postBattleConditionText;
    [SerializeField] private Color winConditionTextColour;
    [SerializeField] private Color loseConditionTextColour;
    private UIElement postBattleUI;

    private void Awake()
    {
        postBattleUI = GetComponent<UIElement>();
    }
    void Start()
    {
        TogglePostBattleUI(false);
    }

    public void TogglePostBattleUI(bool toggle)
    {
        GameManager.instance.ToggleUIElement(postBattleUI, toggle);
    }

    public void TogglPostBattleConditionText(bool playerWin)
    {
        if (playerWin)
        {
            postBattleConditionText.UpdateContentText("VICTORY");
            postBattleConditionText.UpdateContentTextColour(winConditionTextColour);
        }
        else
        {
            postBattleConditionText.UpdateContentText("DEFEAT");
            postBattleConditionText.UpdateContentTextColour(loseConditionTextColour);
        }
    }
}
