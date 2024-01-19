using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEngine.Advertisements;
using UnityEditor;
using UnityEditor.Advertisements;

public class UnityAdsBuildProcessor : Editor
{
    [PostProcessScene]
    public static void OnPostprocessScene()
    {
        AdvertisementSettings.enabled = true;
        AdvertisementSettings.initializeOnStartup = false;
    }
}

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    public static AdManager Instance;

    [SerializeField] private bool isTesting;
    [SerializeField] private bool clearPlayerPrefs;
    [SerializeField] private bool isDegugging;
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

            // If the user opts in to targeted advertising:
            MetaData gdprMetaData = new MetaData("gdpr");
            gdprMetaData.Set("consent", "true");
            Advertisement.SetMetaData(gdprMetaData);

            PlayerPrefs.SetInt("OverAgeRequirement", 1);

            CharacterCarasel.Instance.ResetMenu();
        }

#if UNITY_ANDROID && !UNITY_EDITOR
    Advertisement.Initialize(androidID, isTesting, this);
    Advertisement.Load(androidID, this);
#endif

#if UNITY_IOS
    Advertisement.Initialize(iosID, isTesting, this);
    Advertisement.Load(iosID, this);

#endif
#if UNITY_EDITOR
        Advertisement.Initialize(androidID, isTesting, this);
        Advertisement.Load(androidID, this);
#endif
            
    }

    void Update()
    {
        AdStartManualInputSkippable();
        AdStartManualInputForced();
    }

    void AdStartManualInputSkippable()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ShowSkippableAd();
    }

    void AdStartManualInputForced()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowForcedAd();
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
        if (isDegugging)
            Debug.Log("AD ERROR");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if (placementId == forcedVideoPlacementID)
            {
                if (isDegugging)
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
        if (isDegugging)
            Debug.Log("Ad Ready");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (isDegugging)
            Debug.Log("Ad Showing");
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (isDegugging)
            Debug.Log("failed to load ad");
        StartCoroutine(TryLoadAds());
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (isDegugging)
            Debug.Log("failed to show ad");
        StartCoroutine(TryLoadAds());
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
            if (isDegugging)
                Debug.Log("Ads fully Watched");
        }
    }

    public void OnInitializationComplete()
    {
        LoadAds();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        if (isDegugging)
            Debug.Log("failed to init ad");

        StartCoroutine(TryInitAds());
    }

    IEnumerator TryInitAds()
    {
        yield return new WaitForSeconds(3);
        InitAds();
    }

    IEnumerator TryLoadAds()
    {
        yield return new WaitForSeconds(3);
        LoadAds();
    }

    void LoadAds()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    Advertisement.Load(androidID, this);
#endif

#if UNITY_IOS
    Advertisement.Load(iosID, this);

#endif
#if UNITY_EDITOR
    Advertisement.Load(androidID, this);
#endif
    }

    void InitAds()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    Advertisement.Initialize(androidID, isTesting, this);
#endif

#if UNITY_IOS
    Advertisement.Initialize(iosID, isTesting, this);

#endif
#if UNITY_EDITOR
    Advertisement.Initialize(androidID, isTesting, this);
#endif
    }
}
