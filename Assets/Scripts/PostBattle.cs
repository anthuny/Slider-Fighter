using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostBattle : MonoBehaviour
{
    [SerializeField] private UIElement postBattleConditionText;
    [SerializeField] private Color winConditionTextColour;
    [SerializeField] private Color loseConditionTextColour;
    [SerializeField] private UIElement buttonPostBattleMap;
    [SerializeField] private int timeToSpawnToMapButton;

    [SerializeField] private UIElement expGainedUI;
    [SerializeField] private UIElement rewardsUI;
    [SerializeField] private ButtonFunctionality toMapButton;

    private UIElement postBattleUI;

    private void Awake()
    {
        postBattleUI = GetComponent<UIElement>();
    }
    void Start()
    {
        TogglePostBattleUI(false);
    }

    public void ToggleExpGainedUI(bool toggle)
    {
        if (toggle)
            expGainedUI.UpdateAlpha(1);
        else
            expGainedUI.UpdateAlpha(0);
    }

    public void ToggleRewardsUI(bool toggle)
    {
        if (toggle)
            rewardsUI.UpdateAlpha(1);
        else
            rewardsUI.UpdateAlpha(0);
    }

    void ToggleToMapButtonInteractable(bool toggle)
    {
        toMapButton.ToggleButton(toggle);
        Handheld.Vibrate();
    }

    public void TogglePostBattleUI(bool toggle)
    {
        ToggleToMapButtonInteractable(toggle);

        GameManager.Instance.ToggleUIElement(postBattleUI, toggle);
    }

    public void TogglePostBattleConditionText(bool playerWin)
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

        StartCoroutine(ToggleButtonPostBattleMap(true));
    }

    public IEnumerator ToggleButtonPostBattleMap(bool toggle)
    {
        yield return new WaitForSeconds(timeToSpawnToMapButton);

        if (toggle)
        {
            buttonPostBattleMap.UpdateAlpha(1);
        }
        else
        {
            buttonPostBattleMap.UpdateAlpha(0);
        }
    }
}
