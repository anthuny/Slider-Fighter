using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostBattle : MonoBehaviour
{
    public static PostBattle Instance;

    [SerializeField] private UIElement postBattleConditionText;
    [SerializeField] private Color winConditionTextColour;
    [SerializeField] private Color loseConditionTextColour;
    [SerializeField] private UIElement buttonPostBattleMap;
    [SerializeField] private int timeToSpawnToMapButton;

    [SerializeField] private UIElement expGainedUI;
    [SerializeField] private UIElement rewardsUI;
    [SerializeField] private ButtonFunctionality toMapButton;

    private UIElement postBattleUI;
    [HideInInspector] public bool isInPostBattle;

    private void Awake()
    {
        Instance = this;

        postBattleUI = GetComponent<UIElement>();
    }
    void Start()
    {
        ToggleToMapButton(false);
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

    void ToggleToMapButton(bool toggle)
    {
        toMapButton.ToggleButton(toggle);
        //Handheld.Vibrate();
    }

    public void TogglePostBattleUI(bool toggle)
    {
        //ToggleToMapButtonInteractable(toggle);

        GameManager.Instance.ToggleUIElement(postBattleUI, toggle);

        isInPostBattle = toggle;
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

        postBattleConditionText.AnimateUI();
    }

    public IEnumerator ToggleButtonPostBattleMap(bool toggle)
    {
        yield return new WaitForSeconds(0);

        ToggleToMapButton(toggle);
    }
}
