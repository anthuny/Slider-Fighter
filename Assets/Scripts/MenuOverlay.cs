using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOverlay : MonoBehaviour
{
    public static MenuOverlay Instance;

    [SerializeField] private UIElement menuOverlay;
    [SerializeField] private UIElement menuOverlayExitButton;
    [SerializeField] private UIElement killRunButton;
    [SerializeField] private UIElement killRunPrompt;
    [SerializeField] private UIElement killRunPromptButtonYes;
    [SerializeField] private UIElement killRunPrompButtonNo;
    [SerializeField] private UIElement exitPromptButton;

    [HideInInspector]
    public bool menuOverlayOpened = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ToggleMenuOverlay(false);
    }

    public void KillRunPrompt(bool killRun = false)
    {
        // Kill run
        //Debug.Log("Killing Run " + killRun);

        if (!killRun)
        {
            ToggleKillRunPrompt(false);
        }
        else
        {
            ToggleMenuOverlay(false);

            MapManager.Instance.ResetEntireRun(false);
        }
    }
  
    public void ToggleMenuOverlay(bool toggle = true)
    {
        menuOverlayOpened = toggle;

        if (toggle)
        {
            Time.timeScale = 0;

            menuOverlay.UpdateAlpha(1);
            menuOverlayExitButton.UpdateAlpha(1);
            killRunButton.UpdateAlpha(1);

            killRunPrompt.UpdateAlpha(0);
            killRunPromptButtonYes.UpdateAlpha(0);
            killRunPrompButtonNo.UpdateAlpha(0);
            exitPromptButton.UpdateAlpha(0);
        }
        else
        {
            Time.timeScale = 1;

            menuOverlay.UpdateAlpha(0);
            menuOverlayExitButton.UpdateAlpha(0);
            killRunPrompt.UpdateAlpha(0);
            killRunPromptButtonYes.UpdateAlpha(0);
            killRunPrompButtonNo.UpdateAlpha(0);
            exitPromptButton.UpdateAlpha(0);

            menuOverlayExitButton.ToggleButton(toggle);
            killRunButton.ToggleButton(toggle);
            killRunPrompButtonNo.ToggleButton(toggle);
            killRunPromptButtonYes.ToggleButton(toggle);
            exitPromptButton.ToggleButton(toggle);
        }
    }

    public void ToggleKillRunPrompt(bool toggle = true)
    {
        if (toggle)
        {
            killRunPrompt.UpdateAlpha(1);
            killRunPromptButtonYes.UpdateAlpha(1);
            killRunPrompButtonNo.UpdateAlpha(1);
            exitPromptButton.UpdateAlpha(1);
            menuOverlayExitButton.ToggleButton(true);
            killRunButton.ToggleButton(true);
        }
        else
        {
            killRunPrompt.UpdateAlpha(0);
            killRunPromptButtonYes.UpdateAlpha(0);
            killRunPrompButtonNo.UpdateAlpha(0);
            exitPromptButton.UpdateAlpha(0);
        }

        //menuOverlayExitButton.ToggleButton(toggle);

        killRunPrompButtonNo.ToggleButton(toggle);
        killRunPromptButtonYes.ToggleButton(toggle);
        exitPromptButton.ToggleButton(toggle);
    }
}
