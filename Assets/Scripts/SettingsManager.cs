using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [SerializeField] private UIElement uiElement;
    [SerializeField] private Image sfxImage;
    [SerializeField] private Image musicImage;

    [SerializeField] private Sprite sfxOnImage;
    [SerializeField] private Sprite sfxOffImage;
    [SerializeField] private Sprite musicOnImage;
    [SerializeField] private Sprite musicOffImage;

    [SerializeField] private Image sfxSliderFill;
    [SerializeField] private Image musicSliderFill;

    [SerializeField] private Color soundOnColour;
    [SerializeField] private Color soundOffColour;

    bool settingsOpened = false;

    bool sfxOn = true;
    bool musicOn = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sfxSliderFill.color = soundOnColour;
        musicSliderFill.color = soundOnColour;
    }

    public void ToggleSettingsButton(bool toggle)
    {
        uiElement.ToggleButton(toggle);
    }

    public void ToggleSettingsTab()
    {
        if (!settingsOpened)
        {
            settingsOpened = true;
            uiElement.UpdateAlpha(1);
        }
        else
        {
            settingsOpened = false;
            uiElement.UpdateAlpha(0);
        }
    }

    public void ToggleSFX()
    {
        if (!sfxOn)
        {
            sfxOn = true;
            sfxImage.sprite = sfxOnImage;
            AudioManager.Instance.ToggleMuteSFXVolume();

            // Adjust slider fill colour
            sfxSliderFill.color = soundOnColour;
        }
        else
        {
            sfxOn = false;
            sfxImage.sprite = sfxOffImage;
            AudioManager.Instance.ToggleMuteSFXVolume();

            // Adjust slider fill colour
            sfxSliderFill.color = soundOffColour;
        }
    }

    public void ToggleMusic()
    {
        if (!musicOn)
        {
            musicOn = true;
            musicImage.sprite = musicOnImage;
            AudioManager.Instance.ToggleMuteMusicVolume();

            // Adjust slider fill colour
            musicSliderFill.color = soundOnColour;
        }
        else
        {
            musicOn = false;
            musicImage.sprite = musicOffImage;
            AudioManager.Instance.ToggleMuteMusicVolume();

            // Adjust slider fill colour
            musicSliderFill.color = soundOffColour;
        }
    }
}
