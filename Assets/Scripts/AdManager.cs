using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    public static AdManager Instance;

    [SerializeField] private bool isTesting;
    [SerializeField] private bool clearPlayerPrefs;
    [SerializeField] private string androidID = "5512312";
    [SerializeField] private string iosID = "5512313";

    [SerializeField] string skippableVideoPlacementID = "Interstitial_Android";
    [SerializeField] string forcedVideoPlacementID = "Rewarded_Android";

    [SerializeField] private UIElement agePrompt;

    public void AgePromptAboveAge()
    {
        agePrompt.UpdateAlpha(0);
        agePrompt.ToggleButton(false);
        agePrompt.ToggleButton2(false, true);
        agePrompt.ToggleButton3(false, true);   // bg

        // If the user opts in to targeted advertising:
        MetaData gdprMetaData = new MetaData("gdpr");
        gdprMetaData.Set("consent", "true");
        Advertisement.SetMetaData(gdprMetaData);

        PlayerPrefs.SetInt("OverAgeRequirement", 1);
        
        CharacterCarasel.Instance.ResetMenu();

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
    }

    public void AgePromptBelowAge()
    {
        agePrompt.UpdateAlpha(0);
        agePrompt.ToggleButton(false);
        agePrompt.ToggleButton2(false, true);
        agePrompt.ToggleButton3(false, true);   // bg

        // If the user opts out of targeted advertising:
        MetaData gdprMetaData = new MetaData("gdpr");
        gdprMetaData.Set("consent", "false");
        Advertisement.SetMetaData(gdprMetaData);

        PlayerPrefs.SetInt("OverAgeRequirement", 0);

        CharacterCarasel.Instance.ResetMenu();

        // Button Click SFX
        AudioManager.Instance.Play("Button_Click");
    }

    void Start()
    {
        Instance = this;

        if (clearPlayerPrefs)
            PlayerPrefs.DeleteAll();

        // If player hasnt been prompt for age before, do it
        if (PlayerPrefs.GetInt("OverAgeRequirement") == 0)
        {
            // Prompt the player if they are over 18 or not
            agePrompt.UpdateAlpha(1);
            agePrompt.ToggleButton(true);
            agePrompt.ToggleButton2(true, true);
            agePrompt.ToggleButton3(true, true);   // bg

            //CharacterCarasel.Instance.ToggleMenu(false);
        }
        else
        {
            agePrompt.UpdateAlpha(0);
            agePrompt.ToggleButton(false);
            agePrompt.ToggleButton2(false, true);
            agePrompt.ToggleButton3(false, true);   // bg

            CharacterCarasel.Instance.ResetMenu();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            Advertisement.Initialize(androidID, isTesting, this);
            Advertisement.Load(androidID, this);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Advertisement.Initialize(iosID, isTesting, this);
            Advertisement.Load(iosID, this);
        }

        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //Debug.Log("Platform is Editor");
            Advertisement.Initialize(androidID, isTesting, this);
        }
    }

    void Update()
    {
        //AdStartManualInput();
    }

    void AdStartManualInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowSkippableAd();
    }

    public void ShowForcedAd()
    {
        //Debug.Log("input to show ad");
        Advertisement.Show(forcedVideoPlacementID, this);
    }

    public void ShowSkippableAd()
    {
        //Debug.Log("input to show ad");
        Advertisement.Show(skippableVideoPlacementID, this);
    }


    public void OnUnityAdsDidError(string message)
    {
        //Debug.Log("AD ERROR");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if (placementId == forcedVideoPlacementID)
            {
                Debug.Log("Ad Finished");
            }
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //Debug.Log("Ad Started");
    }

    public void OnUnityAdsReady(string placementId)
    {
        //Debug.Log("Ad Ready");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Showing");
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //throw new System.NotImplementedException();
        if (placementId == forcedVideoPlacementID && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Ads fully Watched");
        }
    }

    public void OnInitializationComplete()
    {
        
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }
}
